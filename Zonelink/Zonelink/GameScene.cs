﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Code2015.EngineEx;
using Code2015.World;
using CustomModelAnimation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zonelink.Graphics;
using Zonelink.MathLib;
using Zonelink.World;
using Apoc3D;
using Apoc3D.Graphics;
using Apoc3D.Collections;

namespace Zonelink
{
    class GameScene : DrawableGameComponent
    {
        public const float OldModelScale = 3;
        Game1 game;
        GameState state;
        SpriteBatch postSprite;

        #region  Terrain
        TerrainTile[] terrainTiles;
        MaterialLibrary terrainLibrary;
        Effect terrainEffect;
        #endregion

        #region CityModel
        /// <summary>
        ///  游戏场景中的City与RigidModel一一对应的表
        /// </summary>
        Dictionary<City, RigidModel> cityModelStopped;
        Dictionary<City, RigidModel> cityModelIdle;
        Dictionary<City, RigidModel> cityModelSend;
        Dictionary<City, RigidModel> cityModelReceive;


        #endregion

        #region NaturalResources
        Model[] oilDerrick;
        Model[] oilDerrickSea;
        Matrix oilDerrickPremult;
        #endregion

        #region Harvester
        Model[] harvester;
        Matrix harvesterPremult;
        #endregion

        #region FX
        RenderTarget2D prepassBuffer;
        ShadowMap vsm;
        Effect stdSMGen;
        Effect skinSMGen;
        #endregion

        RtsCamera camera;

        bool isCityModelLoaded;

        bool isLoaded;


        public RtsCamera Camera { get { return camera; } }

        public GameScene(Game1 game, GameState state)
            : base(game)
        {
            this.game = game;
            this.state = state;
           
        }

        public override void Initialize()
        {
            terrainTiles = new TerrainTile[PlanetEarth.ColTileCount * PlanetEarth.LatTileCount];

            Viewport vp = game.GraphicsDevice.Viewport;
            float aspectRatio = vp.Width / (float)vp.Height;

            camera = new RtsCamera(45, aspectRatio);
            camera.NearPlane = 20;
            camera.FarPlane = 6000;

            base.Initialize();
        }

        #region 加载资源
        protected override void LoadContent()
        {
            LoadEffect();
            LoadTerrain();
            LoadResourceModel();

            isLoaded = true;
        }
        private void LoadEffect() 
        {
            postSprite = new SpriteBatch(game.GraphicsDevice);
            vsm = new ShadowMap(game);

            terrainEffect = game.Content.Load<Effect>(Path.Combine(GameFileLocs.CEffect, "TerrainEffect"));
            terrainEffect.CurrentTechnique = terrainEffect.Techniques[0];

            stdSMGen = game.Content.Load<Effect>(Path.Combine(GameFileLocs.CEffectSMGen, "StdGen"));
            stdSMGen.CurrentTechnique = stdSMGen.Techniques[0];
           
            skinSMGen = game.Content.Load<Effect>(Path.Combine(GameFileLocs.CEffectSMGen, "SkinGen"));
            skinSMGen.CurrentTechnique = skinSMGen.Techniques[0];

        }


        private void LoadTerrain()
        {
            terrainLibrary = new MaterialLibrary(game);
            terrainLibrary.LoadTextureSet(Path.Combine(GameFileLocs.Configs, "terrainMaterial.xml"));

            TerrainData.Initialize();

            for (int i = 1, index = 0; i < PlanetEarth.ColTileCount * 2; i += 2)
            {
                for (int j = 1; j < PlanetEarth.LatTileCount * 2; j += 2)
                {
                    terrainTiles[index++] = new TerrainTile(game, i, j + PlanetEarth.LatTileStart);
                }
            }

        }

        private void LoadResourceModel()
        {
            oilDerrick = new Model[NatureResource.OilFrameCount];
            oilDerrickSea = new Model[NatureResource.OilFrameCount];

            for (int i = 0; i < NatureResource.OilFrameCount; i++)
            {
                oilDerrick[i] = game.Content.Load<Model>("Model\\OilDerrick\\oilderrick" + i.ToString("D2"));
                oilDerrickSea[i] = game.Content.Load<Model>("Model\\OilDerrick\\oilderricksea" + i.ToString("D2"));
            }
            oilDerrickPremult = Matrix.CreateScale(2.2f * OldModelScale, 2.2f * OldModelScale, 2.2f * OldModelScale) *
                   Matrix.CreateTranslation(0, 18, 0) * Matrix.CreateRotationY(-MathHelper.PiOver4);


            harvester = new Model[Harvester.NumModels];
            for (int i = 0; i < Harvester.NumModels; i++)
            {
                harvester[i] = game.Content.Load<Model>("Model\\Harvester\\cow" + i.ToString("D2"));
            }

            harvesterPremult = Matrix.CreateRotationY(MathHelper.Pi) *
                Matrix.CreateScale(OldModelScale * 0.67f, OldModelScale * 0.67f, OldModelScale * 0.67f);
        }
       

        private void LoadCityModel()
        {
            this.cityModelSend = new Dictionary<City, RigidModel>(BattleField.MaxCities);
            this.cityModelStopped = new Dictionary<City, RigidModel>(BattleField.MaxCities);
            this.cityModelReceive = new Dictionary<City, RigidModel>(BattleField.MaxCities);
            this.cityModelIdle = new Dictionary<City, RigidModel>(BattleField.MaxCities);

            foreach (City city in state.Field.Cities)
            {
                RigidModel mdl=new RigidModel(game, "testSend");
                city.HookAnimationEvent(mdl);
                this.cityModelSend.Add(city, mdl);

                mdl = new RigidModel(game, "testStopped");
                city.HookAnimationEvent(mdl);
                this.cityModelStopped.Add(city, new RigidModel(game, "testStopped"));

                mdl = new RigidModel(game, "testIdle");
                city.HookAnimationEvent(mdl);
                this.cityModelIdle.Add(city, new RigidModel(game, "testIdle"));

                mdl = new RigidModel(game, "testRecv");
                city.HookAnimationEvent(mdl);
                this.cityModelReceive.Add(city, new RigidModel(game, "testRecv"));
            }

        }

        #endregion

        protected override void UnloadContent()
        {
            isLoaded = false;
            for (int i = 0; i < terrainTiles.Length; i++)
            {
                terrainTiles[i].Dispose();
            }
            terrainEffect.Dispose();
        }


        private void DrawSkinnedModel(Matrix transform, SkinnedModel model)
        {

            Model m = model.Model;

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = camera.ProjectionMatrix;
                    effect.View = camera.ViewMatrix;
                    model.SetupEffect(effect, transform);

                    effect.SpecularColor = Vector3.Zero;
                }

                mesh.Draw();
            }
        }
        private void DrawRigidModel(Matrix transform, RigidModel model)
        {
            Model m = model.Model;
           
            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = camera.ProjectionMatrix;
                    effect.View = camera.ViewMatrix;
                    model.SetupEffect(effect, transform, mesh);

                    effect.SpecularColor = Vector3.Zero;
                }

                mesh.Draw();
            }
        }
        private void DrawSkinnedModelSMGen(Matrix transform, SkinnedModel model)
        {

            Model m = model.Model;
            transform = model.GetRootTransform() * transform;

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (EffectPass p in skinSMGen.CurrentTechnique.Passes)
                {
                    Matrix lightPrjTrans;
                    Matrix.Multiply(ref transform, ref EffectParameters.DepthViewProj, out lightPrjTrans);
                    skinSMGen.Parameters["mvp"].SetValue(lightPrjTrans);
                    skinSMGen.Parameters["Bones"].SetValue(model.GetBoneTransforms());

                    p.Apply();

                    mesh.Draw();
                }
            }
        }
        private void DrawRigidModelSMGen(Matrix transform, RigidModel model)
        {
            Model m = model.Model;

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (EffectPass p in stdSMGen.CurrentTechnique.Passes)
                {
                    Matrix world = model.GetWorldTransform(transform, mesh);

                    Matrix lightPrjTrans;
                    Matrix.Multiply(ref world, ref EffectParameters.DepthViewProj, out lightPrjTrans);
                    stdSMGen.Parameters["mvp"].SetValue(lightPrjTrans);

                    p.Apply();


                    mesh.Draw();
                }
            }
        }


        private void DrawCities(GameTime time, bool smgen)
        {
            Frustum frus = camera.Frustum;
            
            if (!isCityModelLoaded)
            {
                LoadCityModel();
                isCityModelLoaded = true;
            }

            foreach (City city in state.Field.Cities)
            {
                if (frus.IntersectsSphere(city.Position, city.BoundingSphere.Radius))
                {
                    switch (city.AnimationType)
                    {
                        case CityAnimationState.Stopped:
                            if (smgen)
                                DrawRigidModelSMGen(city.Transformation, cityModelStopped[city]);
                            else
                                DrawRigidModel(city.Transformation, cityModelStopped[city]);
                            break;

                        case CityAnimationState.SendBall:
                            if (smgen)
                                DrawRigidModelSMGen(city.Transformation, cityModelSend[city]);
                            else
                                DrawRigidModel(city.Transformation, cityModelSend[city]);
                            break;

                        case CityAnimationState.ReceiveBall:
                            if (smgen)
                                DrawRigidModelSMGen(city.Transformation, cityModelReceive[city]);
                            else
                                DrawRigidModel(city.Transformation, cityModelReceive[city]);
                            break; ;
                    }

                }

                if (city.Type == CityType.Green || city.Type == CityType.Oil)
                {
                    GatherCity gcity = city as GatherCity;

                    Harvester harv = gcity.Harvester;
                    if (frus.IntersectsSphere(harv.Position, harv.BoundingSphere.Radius))
                    {
                        int fid = harv.ModelIndex;
                        if (!smgen)
                        {
                            foreach (ModelMesh mesh in harvester[fid].Meshes)
                            {
                                foreach (BasicEffect effect in mesh.Effects)
                                {
                                    effect.EnableDefaultLighting();
                                    effect.Projection = camera.ProjectionMatrix;
                                    effect.View = camera.ViewMatrix;
                                    effect.World = harvesterPremult * harv.Transformation;

                                    effect.SpecularColor = Vector3.Zero;
                                }

                                mesh.Draw();
                            }
                        }
                        else 
                        {
                            foreach (ModelMesh mesh in harvester[fid].Meshes)
                            {
                                foreach (EffectPass p in stdSMGen.CurrentTechnique.Passes)
                                {
                                    Matrix world = harvesterPremult * harv.Transformation;
                                    Matrix lightPrjTrans;
                                    Matrix.Multiply(ref world, ref EffectParameters.DepthViewProj, out lightPrjTrans);
                                    stdSMGen.Parameters["mvp"].SetValue(lightPrjTrans);

                                    p.Apply();

                                    mesh.Draw();
                                }
                            }
                        }
                    }
                }

            }

        }
      

        private void DrawNaturalResource(GameTime time, bool smGen)
        { 
            Frustum frus = camera.Frustum;
           
            NatureResource[] resources = state.Field.NaturalResources;

            for (int i = 0; i < resources.Length; i++)
            {
                if (frus.IntersectsSphere(ref resources[i].BoundingSphere.Center,
                    resources[i].BoundingSphere.Radius))
                {
                    if (resources[i].Type == NaturalResourceType.Oil)
                    {
                        Model[] mdl = resources[i].IsInOcean ? oilDerrickSea : oilDerrick;

                        int fid = resources[i].FrameIndex;
                       
                        foreach (ModelMesh mesh in mdl[fid].Meshes)
                        {                            
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                                effect.EnableDefaultLighting();
                                effect.Projection = camera.ProjectionMatrix;
                                effect.View = camera.ViewMatrix;
                                effect.World = oilDerrickPremult * resources[i].Transformation;

                                effect.SpecularColor = Vector3.Zero;
                            }

                            mesh.Draw();
                        }

                    }
                }
            }
        }
        private void DrawNaturalResourceSMGen(GameTime time)
        {
            Frustum frus = camera.Frustum;
           
            NatureResource[] resources = state.Field.NaturalResources;

            for (int i = 0; i < resources.Length; i++)
            {
                //if (frus.IntersectsSphere(ref resources[i].BoundingSphere.Center,
                //      resources[i].BoundingSphere.Radius))
                {

                    if (resources[i].Type == NaturalResourceType.Oil)
                    {
                        Model[] mdl = resources[i].IsInOcean ? oilDerrickSea : oilDerrick;

                        int fid = resources[i].FrameIndex;
                        foreach (EffectPass p in stdSMGen.CurrentTechnique.Passes)
                        {
                            Matrix world = resources[i].Transformation;
                            Matrix lightPrjTrans;
                            Matrix.Multiply(ref world, ref EffectParameters.DepthViewProj, out lightPrjTrans);
                            stdSMGen.Parameters["mvp"].SetValue(lightPrjTrans);

                            p.Apply();

                            foreach (ModelMesh mesh in mdl[fid].Meshes)
                            {
                                mesh.Draw();
                            }
                        }
                    }
                }
            }
        }

        private void DrawTerrain(GameTime time)
        {
            Frustum frus = camera.Frustum;



            //渲染地形
            game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            foreach (EffectPass p in terrainEffect.CurrentTechnique.Passes)
            {
                for (int i = 0; i < terrainTiles.Length; i++)
                {
                    Vector3 center = terrainTiles[i].BoundingSphere.Center;
                    if (frus.IntersectsSphere(ref center, terrainTiles[i].BoundingSphere.Radius))
                    {
                        terrainEffect.Parameters["mvp"].SetValue(terrainTiles[i].Transformation * camera.ViewMatrix * camera.ProjectionMatrix);
                        terrainEffect.Parameters["terrSize"].SetValue(33.0f);
                        terrainEffect.Parameters["world"].SetValue(terrainTiles[i].Transformation);
                        terrainEffect.Parameters["i_a"].SetValue(EffectParameters.LightAmbient);
                        terrainEffect.Parameters["i_d"].SetValue(EffectParameters.LightDiffuse);
                        terrainEffect.Parameters["lightDir"].SetValue(EffectParameters.LightDir);

                        terrainEffect.Parameters["texDet1"].SetValue(terrainLibrary.GetTexture("Snow"));
                        terrainEffect.Parameters["texDet2"].SetValue(terrainLibrary.GetTexture("Grass"));
                        terrainEffect.Parameters["texDet3"].SetValue(terrainLibrary.GetTexture("Sand"));
                        terrainEffect.Parameters["texDet4"].SetValue(terrainLibrary.GetTexture("Rock"));
                        terrainEffect.Parameters["texShd"].SetValue(EffectParameters.ShadowMap);


                        terrainEffect.Parameters["texColor"].SetValue(terrainLibrary.GlobalColorTexture);
                        terrainEffect.Parameters["texIdx"].SetValue(terrainLibrary.GlobalIndexTexture);
                        terrainEffect.Parameters["texNorm"].SetValue(terrainLibrary.GlobalNormalTexture);
                        terrainEffect.Parameters["texCliff"].SetValue(terrainLibrary.CliffColor);

                        terrainEffect.Parameters["k_a"].SetValue(EffectParameters.TerrainAmbient);
                        terrainEffect.Parameters["k_d"].SetValue(EffectParameters.TerrainDiffuse);
                        Matrix lightPrjTrans;
                        Matrix.Multiply(ref terrainTiles[i].Transformation, ref EffectParameters.DepthViewProj, out lightPrjTrans);

                        terrainEffect.Parameters["smTrans"].SetValue(lightPrjTrans);

                        //float4 k_d;
                        //float4 k_a;

                        //float4 i_a;
                        //float4 i_d;
                        p.Apply();

                        terrainTiles[i].Render();
                    }

                }
            }
        }
        private void DrawTerrainSMGen(GameTime time)
        {
            Frustum frus = camera.Frustum;
           
            //渲染地形
            game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            foreach (EffectPass p in stdSMGen.CurrentTechnique.Passes)
            {
                for (int i = 0; i < terrainTiles.Length; i++)
                {
                    Vector3 center = terrainTiles[i].BoundingSphere.Center;
                    if (frus.IntersectsSphere(ref center, terrainTiles[i].BoundingSphere.Radius))
                    {
                        Matrix lightPrjTrans;
                        Matrix.Multiply(ref terrainTiles[i].Transformation, ref EffectParameters.DepthViewProj, out lightPrjTrans);
                        stdSMGen.Parameters["mvp"].SetValue(lightPrjTrans);

                        p.Apply();

                        terrainTiles[i].Render();
                    }


                }
            }
        }

        private void DrawBattleField(GameTime time)
        {
            DrawCities(time, false);
            DrawNaturalResource(time);
        }
        private void GenerateShadowMap(GameTime time)
        {
            vsm.Begin(EffectParameters.LightDir, EffectParameters.CurrentCamera);

            //DrawTerrainSMGen(time);
            DrawCities(time, true);
            DrawNaturalResourceSMGen(time);

            vsm.End(postSprite);

            EffectParameters.ShadowMap = vsm.ShadowColorMap;

        }

        public override void Draw(GameTime time)
        {
            EffectParameters.CurrentCamera = camera;
            if (!isLoaded)
                return;

            GenerateShadowMap(time);

            //game.GraphicsDevice.Clear(Color.White);

            //DrawTerrain(time);
            //DrawBattleField(time);


            //postSprite.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            //postSprite.Draw(vsm.ShadowColorMap, Vector2.Zero, Color.White);

            //postSprite.End();
        }

        public override void Update(GameTime time)
        {
            camera.Update(time);
            EffectParameters.InvView = Matrix.Invert(camera.ViewMatrix);

            Matrix view = Matrix.CreateRotationY(-MathHelper.Pi / 6) * EffectParameters.InvView;
            EffectParameters.LightDir = -view.Forward;

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
            {
                camera.MoveFront();
            }
            if (state.IsKeyDown(Keys.S))
            {
                camera.MoveBack();
            }
            if (state.IsKeyDown(Keys.A))
            {
                camera.MoveLeft();
            }
            if (state.IsKeyDown(Keys.D))
            {
                camera.MoveRight();
            }

            BattleField btfld = this.state.Field;
            btfld.Update(time);
            if (isLoaded)
            {
                if (isCityModelLoaded)
                {

                    foreach (City city in btfld.Cities)
                    {

                        if (!this.cityModelIdle[city].IsPlaying)
                        {
                            this.cityModelIdle[city].Play();
                        }
                        else
                        {
                            this.cityModelIdle[city].Update(time);
                        }

                        if (!this.cityModelStopped[city].IsPlaying)
                        {
                            this.cityModelStopped[city].Play();
                        }
                        else
                        {
                            this.cityModelStopped[city].Update(time);
                        }

                        if (!this.cityModelSend[city].IsPlaying)
                        {
                            this.cityModelSend[city].Play();
                        }
                        else
                        {
                            this.cityModelSend[city].Update(time);
                        }
                    }
                }

            }


        }


        /// <summary>
        /// 获取城市在屏幕坐标的位置区域
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        private Rectangle GetCityScreenArea(City city)
        {
            Vector3 tangX = PlanetEarth.GetTangentX(MathHelper.ToRadians(city.Longitude),
                MathHelper.ToRadians(city.Latitude));

            Vector3 pposLeft = game.GraphicsDevice.Viewport.Project(city.Position - tangX * (RulesTable.CityRadius + 5),
                camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);

            Vector3 pposRight = game.GraphicsDevice.Viewport.Project(city.Position - tangX * (RulesTable.CityRadius + 5),
               camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);


            Point screenPosLeft = new Point((int)pposLeft.X, (int)pposLeft.Y);
            int width = (int)(pposRight.X - pposRight.X);
            int height = (int)(pposRight.Y - pposLeft.Y);

            return new Rectangle(screenPosLeft.X, screenPosLeft.Y, width, height);
        }


       
    }
}
