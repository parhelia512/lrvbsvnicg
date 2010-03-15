﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
    delegate void ObjectSpaceChangedHandler(Matrix matrix, BoundingSphere sphere);

    unsafe class TerrainMesh : Resource, IRenderable
    {
        public const int TerrainBlockSize = 33;
        public const int LocalLodCount = 4;

        const float PlanetRadius = PlanetEarth.PlanetRadius;

        struct TerrainVertex
        {
            public Vector3 Position;
            public float u;
            public float v;
            public float Index;

            static VertexElement[] elements;
            static int size = sizeof(TerrainVertex);
            static TerrainVertex()
            {
                elements = new VertexElement[3];
                elements[0] = new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position);
                elements[1] = new VertexElement(elements[0].Size,
                    VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0);
                elements[2] = new VertexElement(elements[1].Size + elements[1].Offset,
                    VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1);
            }

            public static VertexElement[] Elements
            {
                get { return elements; }
            }

            public static int Size
            {
                get { return size; }
            }
        }

        bool hasData;
        //FileLocation resLoc;
        FileLocation nrmMapLoc;
        Texture normalMap;

        /// <summary>
        ///  地形一条边上的顶点数
        /// </summary>
        int terrEdgeSize;


        VertexDeclaration vtxDecl;
        VertexBuffer vtxBuffer;

        IndexBuffer[] indexBuffer;

        /// <summary>
        ///  世界LOD级别
        /// </summary>
        int dataLevel;

        RenderSystem renderSystem;
        ObjectFactory factory;


        FastList<RenderOperation> opBuffer;

        Material material;

        public BoundingSphere BoundingSphere;
        public Matrix Transformation = Matrix.Identity;

        /// <summary>
        ///  表示地形是否是分块的
        /// </summary>
        bool isBlockTerrain;

        #region 分块地形的数据
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

        Queue<TerrainTreeNode> bfsQueue;
        TerrainTreeNode rootNode;
        #endregion

        /// <summary>
        ///  不是分块地形时，
        /// </summary>
        GeomentryData defGeometryData;

        float tileCol;
        float tileLat;
        int tileX;
        int tileY;

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

        public int TerrainSize
        {
            get { return terrEdgeSize; }
        }


        public event ObjectSpaceChangedHandler ObjectSpaceChanged;
        
        public static string GetHashString(int x, int y, int lod)
        {
            return "TM" + x.ToString("D2") + y.ToString("D2") + lod.ToString("D1");
        }



        public TerrainMesh(RenderSystem rs, int x, int y, int lod)
            : base(TerrainMeshManager.Instance, GetHashString(x, y, lod))
        {
            this.bfsQueue = new Queue<TerrainTreeNode>();
            this.opBuffer = new FastList<RenderOperation>();

            this.tileX = x;
            this.tileY = y;

            hasData = TerrainData.Instance.HasData(x, y);
            //resLoc = FileSystem.Instance.TryLocate(
            //    "tile_" + x.ToString("D2") + "_" + y.ToString("D2") + "_" + lod.ToString() + TDMPIO.Extension, GameFileLocs.Terrain);
            nrmMapLoc = FileSystem.Instance.TryLocate(
                "tile_" + x.ToString("D2") + "_" + y.ToString("D2") + "_0" + TextureData.Extension, GameFileLocs.TerrainNormal);

            dataLevel = lod;
            renderSystem = rs;
            factory = rs.ObjectFactory;

            material = new Material(rs);
            material.CullMode = CullMode.CounterClockwise;

            //material.Ambient = terrData.MaterialAmbient;
            //material.Diffuse = terrData.MaterialDiffuse;
            //material.Emissive = terrData.MaterialEmissive;
            //material.Specular = terrData.MaterialSpecular;
            //material.Power = terrData.MaterialPower;
            material.Ambient = new Color4F(1, 0.5f, 0.5f, 0.5f);
            material.Diffuse = new Color4F(1f, 1f, 1f, 1f);
            material.Specular = new Color4F(0, 0, 0, 0);
            material.Power = 1;
            material.PriorityHint = RenderPriority.Second;
            material.SetTexture(0, TerrainMaterialLibrary.Instance.GlobalIndexTexture);

            PlanetEarth.TileCoord2CoordNew(x, y, out tileCol, out tileLat);

            switch (lod)
            {
                case 0:
                    terrEdgeSize = 513;
                    break;
                case 1:
                    terrEdgeSize = 129;
                    break;
                case 2:
                    terrEdgeSize = 33;
                    break;
                default:
                    terrEdgeSize = 513;
                    break;
            }

            // 估算包围球
            {
                float radtc = MathEx.Degree2Radian(tileCol);
                float radtl = MathEx.Degree2Radian(tileLat);
                float rad5 = PlanetEarth.DefaultTileSpan * 0.5f;

                BoundingSphere.Center = PlanetEarth.GetPosition(radtc + rad5, radtl - rad5);
                BoundingSphere.Radius = MathEx.Root2 * PlanetEarth.GetTileHeight(rad5 * 2);

                if (ObjectSpaceChanged != null)
                    ObjectSpaceChanged(Transformation, BoundingSphere);
            }
        }

        #region Resource实现
        public override int GetSize()
        {
            int size = 0;
            if (hasData)
            {
                switch (dataLevel)
                {
                    case 0:
                        size += TerrainVertex.Size * 1025 * 1025;
                        size += sizeof(int) * (32 * 32) * 6 * LocalLodCount;
                        break;
                    case 1:
                        size += TerrainVertex.Size * 257 * 257;
                        size += sizeof(int) * (8 * 8) * 6 * LocalLodCount;
                        break;
                    case 2:
                        size += TerrainVertex.Size * 65 * 65;
                        size += sizeof(int) * (2 * 2) * 6 * LocalLodCount;
                        break;
                }
            }
            if (nrmMapLoc != null)
            {
                if (normalMap != null)
                    size += normalMap.ContentSize;
            }

            return size;
        }

        protected override void load()
        {
            if (!hasData)
                return;

            if (nrmMapLoc != null)
            {
                normalMap = TextureManager.Instance.CreateInstanceUnmanaged(nrmMapLoc);
            }
            else
            {
                normalMap = PlanetEarth.DefaultNormalMap;
            }
            material.SetTexture(1, new ResourceHandle<Texture>(normalMap, true));


            // 读取地形数据
            //TDMPIO data = new TDMPIO();
            //data.Load(resLoc);
            //tileCol = data.Xllcorner;// (float)Math.Round(data.Xllcorner);
            //tileLat = data.Yllcorner;// (float)Math.Round(data.Yllcorner);
            float[] data = TerrainData.Instance.GetData(tileX, tileY, dataLevel);

            float radtc = MathEx.Degree2Radian(tileCol);
            float radtl = MathEx.Degree2Radian(tileLat);
            //terrEdgeSize = data.Width;

            float radSpan = MathEx.Degree2Radian(10);

            int vertexCount = terrEdgeSize * terrEdgeSize;
            int terrEdgeLen = terrEdgeSize - 1;
            isBlockTerrain = terrEdgeSize >= TerrainBlockSize;

            switch (terrEdgeSize)
            {
                case 33:
                    material.SetEffect(EffectManager.Instance.GetModelEffect(TerrainEffect33Factory.Name));
                    break;
                case 129:
                    material.SetEffect(EffectManager.Instance.GetModelEffect(TerrainEffect129Factory.Name));
                    break;
                case 513:
                default:
                    material.SetEffect(EffectManager.Instance.GetModelEffect(TerrainEffect513Factory.Name));
                    break;
            }

            #region 顶点数据

            vtxDecl = factory.CreateVertexDeclaration(TerrainVertex.Elements);

            vtxBuffer = factory.CreateVertexBuffer(vertexCount, vtxDecl, BufferUsage.WriteOnly);

            TerrainVertex[] vtxArray = new TerrainVertex[vertexCount];


            float cellAngle = radSpan / (float)terrEdgeLen;
            #region 计算顶点坐标

            // i为纬度方向
            for (int i = 0; i < terrEdgeSize; i++)
            {
                // j为经度方向
                for (int j = 0; j < terrEdgeSize; j++)
                {
                    Vector3 pos = PlanetEarth.GetPosition(radtc + j * cellAngle, radtl - i * cellAngle);

                    int index = i * terrEdgeSize + j;

                    // 计算海拔高度
                    float height = (data[index] - TerrainMeshManager.PostZeroLevel) * TerrainMeshManager.PostHeightScale;

                    //if (height > 0)
                    //{
                    //    height = (height - 0) * TerrainMeshManager.PostHeightScale;
                    //}
                    //else
                    //{
                    //    height *= TerrainMeshManager.PostHeightScale;
                    //    height -= 10;
                    //    //if (height < -30)
                    //    //    height = -30;
                    //}

                    Vector3 normal = pos;
                    normal.Normalize();
                    vtxArray[index].Position = pos + normal * height;


                    vtxArray[index].Index = index;
                    float curCol = radtc + j * cellAngle;
                    float curLat = radSpan + radtl - i * cellAngle;

                    curCol += MathEx.PIf;
                    curLat -= MathEx.Degree2Radian(10);

                    vtxArray[index].u = 0.5f * curCol / MathEx.PIf;
                    vtxArray[index].v = (-curLat + MathEx.PiOver2) / MathEx.PIf;
                }
            }
            #endregion

            #endregion

            if (isBlockTerrain)
            {
                #region 索引数据
                int blockEdgeLen = TerrainBlockSize - 1;
                this.blockEdgeCount = terrEdgeLen / blockEdgeLen;
                this.blockCount = MathEx.Sqr(blockEdgeCount);

                SharedBlockIndexData sharedData = TerrainMeshManager.Instance.GetSharedIndexData(terrEdgeSize);

                levelLengths = sharedData.LevelLength;
                cellSpan = sharedData.CellSpan;
                lodLevelThreshold = sharedData.LodLevelThreshold;
                levelPrimConut = sharedData.LevelPrimCount;
                levelVertexCount = sharedData.LevelVertexCount;
                indexBuffer = sharedData.IndexBuffers;

                #endregion

                BuildTerrainTree(vtxArray);
            }
            else
            {
                indexBuffer = new IndexBuffer[1];

                #region 索引数据
                this.blockEdgeCount = 1;
                this.blockCount = 1;

                levelLengths = new int[1];
                cellSpan = new int[1];
                lodLevelThreshold = new float[1];

                levelPrimConut = new int[1];
                levelVertexCount = new int[1];


                cellSpan[0] = 1;
                levelLengths[0] = terrEdgeLen;

                int indexCount = MathEx.Sqr(terrEdgeLen) * 2 * 3;

                levelPrimConut[0] = MathEx.Sqr(terrEdgeLen) * 2;
                levelVertexCount[0] = MathEx.Sqr(terrEdgeSize);

                indexBuffer[0] = factory.CreateIndexBuffer(IndexBufferType.Bit32, indexCount, BufferUsage.WriteOnly);

                int[] indexArray = new int[indexCount];

                for (int i = 0, index = 0; i < terrEdgeLen; i++)
                {
                    for (int j = 0; j < terrEdgeLen; j++)
                    {
                        int x = i;
                        int y = j;

                        indexArray[index++] = y * terrEdgeSize + x;
                        indexArray[index++] = y * terrEdgeSize + (x + 1);
                        indexArray[index++] = (y + 1) * terrEdgeSize + (x + 1);

                        indexArray[index++] = y * terrEdgeSize + x;
                        indexArray[index++] = (y + 1) * terrEdgeSize + (x + 1);
                        indexArray[index++] = (y + 1) * terrEdgeSize + x;
                    }
                }
                indexBuffer[0].SetData<int>(indexArray);
                #endregion

                #region 构造GeomentryData
                defGeometryData = new GeomentryData();
                defGeometryData.VertexDeclaration = vtxDecl;

                defGeometryData.VertexSize = TerrainVertex.Size;
                defGeometryData.VertexBuffer = vtxBuffer;
                defGeometryData.IndexBuffer = indexBuffer[0];
                defGeometryData.PrimCount = levelPrimConut[0];
                defGeometryData.VertexCount = levelVertexCount[0];

                defGeometryData.PrimitiveType = RenderPrimitiveType.TriangleList;

                defGeometryData.BaseVertex = 0;

                #endregion
            }
            vtxBuffer.SetData<TerrainVertex>(vtxArray);
        }

        protected override void unload()
        {
            if (!object.ReferenceEquals(vtxBuffer, null))
            {
                vtxBuffer.Dispose();
                vtxBuffer = null;
            }
            if (!object.ReferenceEquals(vtxDecl, null))
            {
                vtxDecl.Dispose();
                vtxDecl = null;
            }
            if (nrmMapLoc != null && normalMap != null)
            {
                normalMap.Dispose();
                normalMap = null;
            }
            indexBuffer = null;
            if (rootNode != null)
            {
                rootNode.Dispose();
                rootNode = null;
            }
        }
        #endregion

        /// <summary>
        ///  构造地形树
        /// </summary>
        /// <param name="vertices">顶点数据</param>
        void BuildTerrainTree(TerrainVertex[] vertices)
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

                    GeomentryData gd = new GeomentryData();
                    gd.VertexDeclaration = vtxDecl;

                    gd.VertexSize = TerrainVertex.Size;
                    gd.VertexBuffer = vtxBuffer;
                    gd.IndexBuffer = indexBuffer[0];
                    gd.PrimCount = levelPrimConut[0];
                    gd.VertexCount = levelVertexCount[0];

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

            //BoundingSphere = rootNode.BoundingVolume;
            //BoundingSphere.Center = Vector3.TransformSimple(rootNode.BoundingVolume.Center, Transformation);
            //if (ObjectSpaceChanged != null)
            //    ObjectSpaceChanged(Transformation, BoundingSphere);
        }

        /// <summary>
        ///  准备特定lod级别下的可见物体
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="level"></param>
        public void PrepareVisibleObjects(ICamera cam, int level)
        {
            if (!hasData)
                return;

            if (State == ResourceState.Loaded)
            {
                opBuffer.Clear();

                if (isBlockTerrain)
                {
                    #region 分块地形的可见性测试

                    Frustum frus = cam.Frustum;
                    Vector3 camPos = cam.Position;

                    Vector3 c = rootNode.BoundingVolume.Center;

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

                                        op.Transformation = Matrix.Identity;
                                        op.Sender = this;
                                        opBuffer.Add(op);
                                    }
                                }
                            }

                        }
                    }
                    #endregion
                }
                else
                {
                    RenderOperation op;

                    op.Material = material;
                    op.Geomentry = defGeometryData;

                    op.Transformation = Matrix.Identity;
                    op.Sender = this; 
                    opBuffer.Add(op);
                }
            }
        }

        #region IRenderable 成员

        public RenderOperation[] GetRenderOperation()
        {
            if (State == ResourceState.Loaded)
            {
                if (hasData)
                {
                    return opBuffer.Elements;
                }
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
