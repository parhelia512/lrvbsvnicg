﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D;
using Apoc3D.Collections;
using Apoc3D.Core;
using Apoc3D.Graphics;
using Apoc3D.Graphics.Effects;
using Apoc3D.MathLib;
using Apoc3D.Vfs;
using Code2015.Effects;
using Code2015.World;

namespace Code2015.EngineEx
{
    unsafe class TerrainMesh : Resource, IRenderable
    {
        public const int TerrainBlockSize = 33;
        public const int LocalLodCount = 4;

        const float PlanetRadius = PlanetEarth.PlanetRadius;

        struct TerrainVertex
        {
            public Vector3 Position;

            static VertexElement[] elements;

            static TerrainVertex()
            {
                elements = new VertexElement[1];
                elements[0] = new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position);
                // elements[1] = new VertexElement(Vector3.SizeInBytes, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0);
            }

            public static VertexElement[] Elements
            {
                get { return elements; }
            }

            public static int Size
            {
                get { return Vector3.SizeInBytes; }
            }
        }


        FileLocation resLoc;

        int terrEdgeSize;


        VertexDeclaration vtxDecl;
        VertexBuffer vtxBuffer;

        IndexBuffer[] indexBuffer = new IndexBuffer[LocalLodCount];


        int dataLevel;

        RenderSystem renderSystem;
        ObjectFactory factory;

        GeomentryData noDataGeo;

        Queue<TerrainTreeNode> bfsQueue;
        TerrainTreeNode rootNode;

        FastList<RenderOperation> opBuffer;

        Material material;

        public BoundingSphere BoundingSphere;
        public Matrix Transformation = Matrix.Identity;

        /// <summary>
        ///  地形块的数量
        /// </summary>
        int blockCount;

        /// <summary>
        ///  地形分块的在地形的一条边上的数量
        /// </summary>
        int blockEdgeCount;

        /// <summary>
        ///  不同的lod级别下一个地形分块的长度
        /// </summary>
        int[] levelLengths;

        /// <summary>
        ///  在不同lod级别下一个单元的跨度
        /// </summary>
        int[] cellSpan;

        /// <summary>
        ///  lod 权值
        /// </summary>
        float[] lodLevelThreshold;

        /// <summary>
        ///  不同的lod级别下一个地形分块的三角形数量
        /// </summary>
        int[] levelPrimConut;

        /// <summary>
        ///  不同的lod级别下一个地形分块的顶点数量
        /// </summary>
        int[] levelVertexCount;

        /// <summary>
        ///  地形的上边（纬度较高的边）的长度
        /// </summary>
        float topLen;
        /// <summary>
        ///  地形的下边（纬度较低的边）的长度
        /// </summary>
        float bottomLen;
        /// <summary>
        ///  地形的侧边（沿经度方向）的长度
        /// </summary>
        float heightLen;

        float tileCol;
        float tileLat;

        /// <summary>
        ///  经度
        /// </summary>
        public float TileCol
        {
            get { return tileCol; }
        }
        /// <summary>
        ///  纬度
        /// </summary>
        public float TileLat
        {
            get { return tileLat; }
        }


        public static string GetHashString(int x, int y, int lod)
        {
            return "TM" + x.ToString() + y.ToString() + lod.ToString();
        }

        public TerrainMesh(RenderSystem rs, int x, int y, int lod)
            : base(TerrainMeshManager.Instance, GetHashString(x, y, lod))
        {
            this.bfsQueue = new Queue<TerrainTreeNode>();
            this.opBuffer = new FastList<RenderOperation>();

            resLoc = FileSystem.Instance.TryLocate(
                "tile_" + x.ToString("D2") + "_" + y.ToString("D2") + "_" + lod.ToString() + TDMPIO.Extension, GameFileLocs.Terrain);
            dataLevel = lod;
            renderSystem = rs;
            factory = rs.ObjectFactory;

            material = new Material(rs);
            material.CullMode = CullMode.None;

            //material.Ambient = terrData.MaterialAmbient;
            //material.Diffuse = terrData.MaterialDiffuse;
            //material.Emissive = terrData.MaterialEmissive;
            //material.Specular = terrData.MaterialSpecular;
            //material.Power = terrData.MaterialPower;
            material.SetEffect(EffectManager.Instance.GetModelEffect(TerrainEffectFactory.Name));

            tileCol = x * 5;
            tileLat = 60 + (y - 13) * 5;

            float radtc = MathEx.Degree2Radian(tileCol);
            float radtl = MathEx.Degree2Radian(tileLat);
            terrEdgeSize = 1025;

            UpdateTransformation(radtc, radtl, terrEdgeSize, 10);
        }

        /// <summary>
        ///  计算该地形的变换矩阵
        /// </summary>
        /// <param name="radtc">经度</param>
        /// <param name="radtl">纬度</param>
        /// <param name="terrSize">原始地形大小</param>
        /// <param name="span">边所占的度数</param>
        void UpdateTransformation(float radtc, float radtl, float terrSize, float span)
        {
            float rad10 = MathEx.Degree2Radian(span);
            float rad5 = MathEx.Degree2Radian(span * 0.5f);
            topLen = PlanetEarth.GetTileWidth(radtl + rad10, rad10);
            bottomLen = PlanetEarth.GetTileWidth(radtl, rad10);
            heightLen = PlanetEarth.GetTileHeight(rad10);

            float poscol = radtc + rad5;
            float poslat = radtl + rad5;

            terrSize--;
            float hs = terrSize * 0.5f;
            Matrix b1 = Matrix.Translation(-hs, 0, -hs);
            Matrix facing = Matrix.Identity;
            facing.Up = PlanetEarth.GetNormal(poscol, poslat);

            Vector3 v = Vector3.Cross(Vector3.UnitY, facing.Up);
            v.Normalize();
            facing.Right = v;// PlanetEarth.GetTangentX(poscol, poslat);

            v = Vector3.Cross(facing.Right, facing.Up);
            v.Normalize();

            facing.Forward = -v;// PlanetEarth.GetTangentY(poscol, poslat);            

            Matrix scaling = Matrix.Scaling(1, 1, heightLen / terrSize);


            Transformation = b1 * scaling * facing * Matrix.Translation(PlanetEarth.GetInnerPosition(poscol, poslat, rad10));

            //Console.WriteLine(PlanetEarth.GetPosition(radtc, radtl));
            //Console.WriteLine(Vector3.TransformSimple(new Vector3(0, 0, terrSize), Transformation));

        }


        #region Resource实现
        public override int GetSize()
        {
            int size = 0;
            if (vtxBuffer != null)
            {
                size += vtxBuffer.Size;
            }

            for (int i = 0; i < indexBuffer.Length; i++)
            {
                if (indexBuffer[i] != null)
                {
                    size += indexBuffer[i].Size;
                }
            }
            return size;
        }

        protected override void load()
        {
            // 判断是否有地形数据
            if (resLoc == null)
            {
                // 如果没有则使用空白地形
                ResourceInterlock.EnterAtomicOp();
                try
                {
                    vtxDecl = factory.CreateVertexDeclaration(TerrainVertex.Elements);
                    vtxBuffer = factory.CreateVertexBuffer(4, vtxDecl, BufferUsage.WriteOnly);
                    TerrainVertex* vertices = (TerrainVertex*)vtxBuffer.Lock(LockMode.None);

                    vertices[0].Position = new Vector3(0, 0, 0);
                    vertices[2].Position = new Vector3(terrEdgeSize, 0, terrEdgeSize);
                    vertices[1].Position = new Vector3(terrEdgeSize, 0, 0);
                    vertices[3].Position = new Vector3(0, 0, terrEdgeSize);

                    vtxBuffer.Unlock();
                }
                finally
                {
                    ResourceInterlock.ExitAtomicOp();
                }

                noDataGeo = new GeomentryData(this);
                noDataGeo.VertexDeclaration = vtxDecl;

                noDataGeo.VertexSize = TerrainVertex.Size;
                noDataGeo.VertexBuffer = vtxBuffer;
                noDataGeo.PrimCount = 2;
                noDataGeo.VertexCount = 4;

                noDataGeo.PrimitiveType = RenderPrimitiveType.TriangleStrip;

                noDataGeo.BaseVertex = 0;

            }
            else
            {
                // 读取地形数据
                TDMPIO data = new TDMPIO();
                data.Load(resLoc);
                tileCol = (float)Math.Round(data.Xllcorner);
                tileLat = (float)Math.Round(data.Yllcorner);

                float radtc = MathEx.Degree2Radian(tileCol);
                float radtl = MathEx.Degree2Radian(tileLat);
                UpdateTransformation(radtc, radtl, terrEdgeSize, data.XSpan);

                MeshData meshData = new MeshData(renderSystem);


                terrEdgeSize = data.Width;
                float halfTerrSize = terrEdgeSize * 0.5f;

                int edgeVtxCount = terrEdgeSize;
                int vertexCount = edgeVtxCount * edgeVtxCount;



                #region 索引数据
                int blockEdgeLen = TerrainBlockSize - 1;
                int edgeLen = edgeVtxCount - 1;
                this.blockEdgeCount = edgeLen / blockEdgeLen;
                this.blockCount = MathEx.Sqr(blockEdgeCount);

                levelLengths = new int[LocalLodCount];
                cellSpan = new int[LocalLodCount];
                lodLevelThreshold = new float[LocalLodCount];

                levelPrimConut = new int[LocalLodCount];
                levelVertexCount = new int[LocalLodCount];

                for (int k = 0, levelLength = blockEdgeLen; k < LocalLodCount; k++, levelLength /= 2)
                {
                    int cellLength = blockEdgeLen / levelLength;


                    lodLevelThreshold[k] = (edgeVtxCount * MathEx.Root2 * 0.25f) / (float)(k + 1);
                    lodLevelThreshold[k] = MathEx.Sqr(lodLevelThreshold[k]);

                    cellSpan[k] = cellLength;
                    levelLengths[k] = levelLength;

                    int indexCount = MathEx.Sqr(levelLength) * 2 * 3;

                    levelPrimConut[k] = MathEx.Sqr(levelLength) * 2;
                    levelVertexCount[k] = MathEx.Sqr(levelLength + 1);

                    ResourceInterlock.EnterAtomicOp();
                    try
                    {
                        indexBuffer[k] = factory.CreateIndexBuffer(IndexBufferType.Bit32, indexCount, BufferUsage.WriteOnly);

                        int* iptr = (int*)indexBuffer[k].Lock(0, 0, LockMode.None);

                        for (int i = 0; i < levelLength; i++)
                        {
                            for (int j = 0; j < levelLength; j++)
                            {
                                int x = i * cellLength;
                                int y = j * cellLength;

                                (*iptr) = y * edgeVtxCount + x;
                                iptr++;
                                (*iptr) = y * edgeVtxCount + (x + cellLength);
                                iptr++;
                                (*iptr) = (y + cellLength) * edgeVtxCount + (x + cellLength);
                                iptr++;

                                (*iptr) = y * edgeVtxCount + x;
                                iptr++;
                                (*iptr) = (y + cellLength) * edgeVtxCount + (x + cellLength);
                                iptr++;
                                (*iptr) = (y + cellLength) * edgeVtxCount + x;
                                iptr++;
                            }
                        }
                        indexBuffer[k].Unlock();
                    }
                    finally
                    {
                        ResourceInterlock.ExitAtomicOp();
                    }
                }
                #endregion

                #region 顶点数据

                //Vector3 lastPosition = new Vector3();
                ResourceInterlock.EnterAtomicOp();
                try
                {
                    vtxDecl = factory.CreateVertexDeclaration(TerrainVertex.Elements);

                    vtxBuffer = factory.CreateVertexBuffer(vertexCount, vtxDecl, BufferUsage.WriteOnly);
                    TerrainVertex* vertices = (TerrainVertex*)vtxBuffer.Lock(LockMode.None);

                    //float latCellSize = heightLen / (float)(terrEdgeSize);
                    float cellAngle = MathEx.Degree2Radian(data.XSpan) / (terrEdgeSize - 1);

                    for (int i = 0; i < edgeVtxCount; i++)
                    {
                        float lerp = i / (float)(terrEdgeSize);
                        float colCellWidth = MathEx.LinearInterpose(topLen, bottomLen, lerp) / (float)terrEdgeSize;
                        float colOfs = (1 - colCellWidth) * halfTerrSize;

                        for (int j = 0; j < edgeVtxCount; j++)
                        {
                            Vector3 pos = new Vector3(j * TerrainMeshManager.TerrainScale * colCellWidth + colOfs,
                                            0,
                                            i * TerrainMeshManager.TerrainScale);


                            float height = data.Data[i * edgeVtxCount + j] * TerrainMeshManager.HeightScale - TerrainMeshManager.ZeroLevel;


                            vertices[i * edgeVtxCount + j].Position = pos;
                              //  PlanetEarth.GetNormal(radtc + Math.Abs(j * cellAngle), radtl + Math.Abs(i * cellAngle)) * height;
                                

                            //lastPosition = vertices[i * edgeVtxCount + j].Position;
                        }
                    }
                    BuildTerrainTree(vertices);

                    vtxBuffer.Unlock();

                    //lastPosition.Y = 0;
                    //lastPosition = Vector3.TransformSimple(lastPosition, Transformation);
                    //Console.WriteLine(lastPosition.ToString());

                }
                finally
                {
                    ResourceInterlock.ExitAtomicOp();
                }

                #endregion

            }
        }

        protected override void unload()
        {
            vtxBuffer.Dispose();
            vtxDecl.Dispose();
            vtxBuffer = null;
            vtxDecl = null;

            for (int i = 0; i < LocalLodCount; i++)
            {
                indexBuffer[i].Dispose();
                indexBuffer[i] = null;
            }
        }
        #endregion

        /// <summary>
        ///  构造地形树
        /// </summary>
        /// <param name="vertices">顶点数据</param>
        void BuildTerrainTree(TerrainVertex* vertices)
        {
            // 地块边的长度，边定点数减1
            int blockEdgeLen = TerrainBlockSize - 1;
            TerrainBlock[] blocks = new TerrainBlock[blockCount];

            float halfTerrSize = terrEdgeSize * 0.5f;

            int index = 0;

            // 枚举每个地块
            for (int i = 0; i < blockEdgeCount; i++)
            {
                for (int j = 0; j < blockEdgeCount; j++)
                {
                    Vector3 center = new Vector3();

                    blocks[index] = new TerrainBlock(j * blockEdgeLen, i * blockEdgeLen);

                    // 检查该块中是否有特殊单元
                    if (!false)
                    {
                        blocks[index].IndexBuffers = indexBuffer;
                    }
                    else
                    {
                        // 为这个block创建特殊的IB
                    }

                    GeomentryData gd = new GeomentryData(this);
                    gd.VertexDeclaration = vtxDecl;

                    gd.VertexSize = TerrainVertex.Size;
                    gd.VertexBuffer = vtxBuffer;
                    gd.IndexBuffer = indexBuffer[0];
                    gd.PrimCount = levelPrimConut[0];// levelLengths[0] * levelLengths[0] * 2;
                    gd.VertexCount = levelVertexCount[0];// MathEx.Sqr(levelLengths[0] + 1);

                    gd.PrimitiveType = RenderPrimitiveType.TriangleList;

                    int x = (j == 0) ? 0 : j * blockEdgeLen;
                    int y = (i == 0) ? 0 : i * blockEdgeLen;

                    gd.BaseVertex = y * terrEdgeSize + x;

                    blocks[index].GeoData = gd;

                    #region 计算包围球中心点
                    for (int ii = 0; ii < TerrainBlockSize; ii++)
                    {
                        for (int jj = 0; jj < TerrainBlockSize; jj++)
                        {
                            int dmY = i * blockEdgeLen + ii;
                            int dmX = j * blockEdgeLen + jj;

                            center += vertices[dmY * terrEdgeSize + dmX].Position;
                        }
                    }

                    float invVtxCount = 1f / (float)(TerrainBlockSize * TerrainBlockSize);
                    center.X *= invVtxCount;
                    center.Y *= invVtxCount;
                    center.Z *= invVtxCount;

                    #endregion

                    #region 计算包围球半径

                    float radius = 0;
                    for (int ii = 0; ii < TerrainBlockSize; ii++)
                    {
                        for (int jj = 0; jj < TerrainBlockSize; jj++)
                        {
                            int dmY = i * blockEdgeLen + ii;
                            int dmX = j * blockEdgeLen + jj;

                            Vector3 vtxPos = vertices[dmY * terrEdgeSize + dmX].Position;

                            float dist = Vector3.Distance(vtxPos, center);
                            if (dist > radius)
                            {
                                radius = dist;
                            }
                        }
                    }
                    blocks[index].Radius = radius;
                    blocks[index].Center = center;

                    #endregion
                    index++;
                }
            }
            rootNode = new TerrainTreeNode(new FastList<TerrainBlock>(blocks), (terrEdgeSize - 1) / 2, (terrEdgeSize - 1) / 2, 1, terrEdgeSize);

            BoundingSphere = rootNode.BoundingVolume;
            BoundingSphere.Center = Vector3.TransformSimple(BoundingSphere.Center, Transformation);
        }

        /// <summary>
        ///  准备特定lod级别下的可见物体
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="level"></param>
        public void PrepareVisibleObjects(ICamera cam, int level)
        {
            if (resLoc == null)
                return;

            if (State == ResourceState.Loaded)
            {
                opBuffer.Clear();

                Matrix invTrans;
                Matrix.Invert(ref Transformation, out invTrans);



                Frustum frus = cam.Frustum.Transform(invTrans);
                Vector3 camPos = Vector3.TransformSimple(cam.Position, invTrans);

                Vector3 c = rootNode.BoundingVolume.Center;
                //Vector3.TransformSimple(ref c, ref Transformation, out c);

                if (frus.IntersectsSphere(ref c, rootNode.BoundingVolume.Radius))
                {
                    bfsQueue.Enqueue(rootNode);

                    while (bfsQueue.Count > 0)
                    {
                        TerrainTreeNode node = bfsQueue.Dequeue();
                        TerrainTreeNode[] nodes = node.Children;

                        if (nodes != null)
                        {
                            // 遍历子节点
                            for (int i = 0; i < node.Children.Length; i++)
                            {
                                c = node.Children[i].BoundingVolume.Center;
                                //Vector3.TransformSimple(ref c, ref Transformation, out c);

                                if (frus.IntersectsSphere(ref c, node.Children[i].BoundingVolume.Radius))
                                {
                                    bfsQueue.Enqueue(node.Children[i]);
                                }
                            }
                        }
                        else
                        {
                            if (node.Block != null)
                            {
                                c = node.BoundingVolume.Center;
                                //Vector3.TransformSimple(ref c, ref Transformation, out c);

                                if (frus.IntersectsSphere(ref c, node.BoundingVolume.Radius))
                                {
                                    float dist = MathEx.DistanceSquared(ref c, ref camPos);

                                    RenderOperation op;

                                    op.Material = material;
                                    op.Geomentry = node.Block.GeoData;

                                    int lodLevel = LocalLodCount - 1;

                                    for (int lod = 0; lod < LocalLodCount; lod++)
                                    {
                                        if (dist <= lodLevelThreshold[LocalLodCount - lod - 1])
                                        {
                                            lodLevel = lod;
                                            break;
                                        }
                                    }

                                    op.Geomentry.IndexBuffer = node.Block.IndexBuffers[lodLevel];
                                    op.Geomentry.PrimCount = levelPrimConut[lodLevel];
                                    op.Geomentry.VertexCount = levelVertexCount[lodLevel];

                                    op.Transformation = Matrix.Identity;// Transformation;

                                    opBuffer.Add(op);
                                }
                            }
                        }

                    }
                }
            }
        }

        #region IRenderable 成员

        public RenderOperation[] GetRenderOperation()
        {
            if (State == ResourceState.Loaded)
            {
                if (resLoc == null)
                {
                    opBuffer.Clear();
                    RenderOperation op;

                    op.Material = material;
                    op.Geomentry = noDataGeo;

                    op.Transformation = Matrix.Identity;// Transformation;

                    opBuffer.Add(op);
                    return opBuffer.Elements;
                }

                return opBuffer.Elements;
            }
            return null;
        }

        public RenderOperation[] GetRenderOperation(int level)
        {
            return GetRenderOperation();
        }

        #endregion
    }
}
