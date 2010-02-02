﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D;
using Apoc3D.Config;
using Apoc3D.Graphics;
using Apoc3D.Graphics.Effects;
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Vfs;
using Code2015.Effects;
using Code2015.EngineEx;
using Code2015.GUI;
using Code2015.World;
using X = Microsoft.Xna.Framework;
using XGS = Microsoft.Xna.Framework.GamerServices;
using XN = Microsoft.Xna.Framework.Net;

namespace Code2015
{
    interface IGameComponent
    {
        void Render();
        void Render(Sprite sprite);
        void Update(GameTime time);
    }

    /// <summary>
    ///  表示游戏。处理、分配各种操作例如渲染，逻辑等等。
    /// </summary>
    class Code2015 : IRenderWindowHandler
    {
        RenderSystem renderSys;

        List<IGameComponent> gameComps;

        Sprite sprite;

        public Code2015(RenderSystem rs)
        {
            this.renderSys = rs;
            this.gameComps = new List<IGameComponent>();
        }

        #region IRenderWindowHandler 成员

        /// <summary>
        ///  处理游戏初始化操作，在游戏初始加载之前
        /// </summary>
        public void Initialize()
        {
            ConfigurationManager.Initialize();
            ConfigurationManager.Instance.Register(new IniConfigurationFormat());

            EffectManager.Initialize(renderSys);
            EffectManager.Instance.RegisterModelEffectType(TerrainEffect513Factory.Name, new TerrainEffect513Factory(renderSys));
            EffectManager.Instance.RegisterModelEffectType(TerrainEffect129Factory.Name, new TerrainEffect129Factory(renderSys));
            EffectManager.Instance.RegisterModelEffectType(TerrainEffect33Factory.Name, new TerrainEffect33Factory(renderSys));
            EffectManager.Instance.RegisterModelEffectType(WaterEffectFactory.Name, new WaterEffectFactory(renderSys));

            TextureManager.Instance.Factory = renderSys.ObjectFactory;
            TerrainMaterialLibrary.Initialize(renderSys);


            EffectManager.Instance.LoadEffects();
            FileLocation fl = FileSystem.Instance.Locate("terrainMaterial.ini", GameFileLocs.Config);
            TerrainMaterialLibrary.Instance.LoadTextureSet(fl);
            sprite = renderSys.ObjectFactory.CreateSprite();

        }

        public void Finalize()
        {
            sprite.Dispose();
        }

        #region unused
        /// <summary>
        ///  处理游戏初始加载资源工作
        /// </summary>
        public void Load()
        {
            // streaming 结构，不在此加载资源
        }

        /// <summary>
        ///  处理游戏关闭时的释放资源工作
        /// </summary>
        public void Unload()
        {

        }
        #endregion

        /// <summary>
        ///  进行游戏逻辑帧中的处理
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            for (int i = 0; i < gameComps.Count; i++) 
            {
                gameComps[i].Update(time);
            }
        }

        /// <summary>
        ///  进行游戏图像渲染帧中的渲染操作
        /// </summary>
        public void Draw()
        {
            for (int i = 0; i < gameComps.Count; i++)
            {
                gameComps[i].Render();
            }

            sprite.Begin();
            for (int i = 0; i < gameComps.Count; i++)
            {
                gameComps[i].Render(sprite);
            }
            sprite.End();
        }

        #endregion
    }
}
