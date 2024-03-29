﻿/*
-----------------------------------------------------------------------------
This source file is part of Zonelink

Copyright (c) 2009+ Tao Games

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  if not, write to the Free Software Foundation, 
Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA, or go to
http://www.gnu.org/copyleft/gpl.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Apoc3D;
using Apoc3D.Collections;
using Apoc3D.Config;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using Apoc3D.Vfs;
using Code2015.EngineEx;
using Code2015.GUI;
using Code2015.Logic;
using Code2015.World;
using XI = Microsoft.Xna.Framework.Input;

namespace Code2015
{
    class GameCreationParameters
    {
        public Player Player1
        {
            get;
            set;
        }

        public Player Player2
        {
            get;
            set;
        }

        public Player Player3
        {
            get;
            set;
        }

        public Player Player4
        {
            get;
            set;
        }
        public Player Player5
        {
            get;
            set;
        }
        public Player Player6
        {
            get;
            set;
        }
        public Player Player7
        {
            get;
            set;
        }
        public Player Player8
        {
            get;
            set;
        }
    }

    /// <summary>
    ///  表示一场游戏
    /// </summary>
    class Game : IGameComponent, IDisposable
    {
        public const float ObjectScale = 3;
        public const float TreeScale = ObjectScale * 4.0f;


        object syncHelper = new object();

        GameCreationParameters parameters;

        Code2015 game;

        SoundObjectWorld soundWorld;

        #region 界面/呈现
        InGameUI ingameUI;

        RenderSystem renderSys;
        GameScene scene;

        #endregion


        #region 游戏状态
        GameState gameState;
        #endregion

        //CityLinkManager linkMgr;

        bool isLoaded;
        int loadingCountDown = 100;

        int maxLoadingOp;

        public GameScene Scene
        {
            get { return scene; }
        }
        public bool IsPaused
        {
            get;
            set;
        }
        public bool IsLoaded
        {
            get
            {
                lock (syncHelper)
                {
                    return isLoaded;
                }
            }
            private set
            {
                lock (syncHelper)
                {
                    isLoaded = value;
                }
            }
        }
        public float LoadingProgress
        {
            get;
            private set;
        }

        public Player HumanPlayer
        {
            get;
            private set;
        }


        public void Over()
        {
            //ResultScore = gameState.GetScores();
            IsOver = true;
        }
        public bool IsOver
        {
            get;
            private set;
        }

        Player[] GetLocalPlayers(GameCreationParameters gcp)
        {
            List<Player> list = new List<Player>();
            if (gcp.Player1 != null && gcp.Player1.Type != PlayerType.Remote)
            {
                list.Add(gcp.Player1);
            }
            if (gcp.Player2 != null && gcp.Player2.Type != PlayerType.Remote)
            {
                list.Add(gcp.Player2);
            }
            if (gcp.Player3 != null && gcp.Player3.Type != PlayerType.Remote)
            {
                list.Add(gcp.Player3);
            }
            if (gcp.Player4 != null && gcp.Player4.Type != PlayerType.Remote)
            {
                list.Add(gcp.Player4);
            }
            if (gcp.Player5 != null && gcp.Player5.Type != PlayerType.Remote)
            {
                list.Add(gcp.Player5);
            }
            if (gcp.Player6 != null && gcp.Player6.Type != PlayerType.Remote)
            {
                list.Add(gcp.Player6);
            }
            if (gcp.Player7 != null && gcp.Player7.Type != PlayerType.Remote)
            {
                list.Add(gcp.Player7);
            }
            if (gcp.Player8 != null && gcp.Player8.Type != PlayerType.Remote)
            {
                list.Add(gcp.Player8);
            }
            return list.ToArray();
        }


        public Game(Code2015 game, GameCreationParameters gcp)
        {
            this.game = game;
            this.renderSys = game.RenderSystem;
            this.parameters = gcp;
            this.soundWorld = new SoundObjectWorld();
            
            gameState = new GameState(GetLocalPlayers(gcp));

            BattleField field = gameState.Field;
            // 初始化场景
            this.scene = new GameScene(renderSys);
           

            
            field.ResourceBallChanged += Field_ResourceBallChanged;

            for (int i = 0; i < field.CityCount; i++)
            {
                City city = field.Cities[i];

                city.InitalizeGraphics(renderSys);
                city.CityVisible += scene.City_Visible;

                GatherCity gaCity = city as GatherCity;
                if (gaCity != null)
                {
                    gaCity.Harvester.InitializeGraphics(renderSys);
                    scene.Scene.AddObjectToScene(gaCity.Harvester);
                }
                scene.Scene.AddObjectToScene(city);
            }

            field.Fog.InitailizeGraphics(renderSys);

            AddResources(field);
            AddScenery(field);
            
            gameState.InitialStandards();
            {
                HumanPlayer = gameState.LocalHumanPlayer;
                City hstart = HumanPlayer.Area.RootCity;
                this.scene.Camera.Longitude = MathEx.Degree2Radian(hstart.Longitude);
                this.scene.Camera.Latitude = MathEx.Degree2Radian(hstart.Latitude);
            }
            this.ingameUI = new InGameUI(game, this, scene, gameState);
        }

        void AddResources(BattleField field)
        {
            NaturalResource[] res = field.NaturalResources;

            for (int i = 0; i < res.Length; i++)
            {
                NaturalResource obj = res[i];
                switch (obj.Type)
                {
                    case NaturalResourceType.Oil:
                        OilFieldObject oilfld = obj as OilFieldObject;
                        if (oilfld != null)
                        {
                            oilfld.ResVisible += scene.Resource_Visible;
                            oilfld.InitalizeGraphics(renderSys);
                            scene.Scene.AddObjectToScene(oilfld);
                        }
                        break;
                    case NaturalResourceType.Wood:
                        ForestObject forest = obj as ForestObject;
                        if (forest != null)
                        {
                            forest.ResVisible += scene.Resource_Visible;
                            forest.InitalizeGraphics(renderSys);
                            scene.Scene.AddObjectToScene(forest);
                        }
                        break;
                }
            }
        }
        void AddScenery(BattleField field)
        {
            FileLocation fl = FileSystem.Instance.Locate("sceneObjects.xml", GameFileLocs.Config);
            Configuration config = ConfigurationManager.Instance.CreateInstance(fl);

            foreach (KeyValuePair<string, ConfigurationSection> e in config)
            {
                ConfigurationSection sect = e.Value;

                SceneryObject obj = new SceneryObject(field);
                obj.Parse((GameConfigurationSection)sect);
                obj.InitalizeGraphics(renderSys);
                scene.Scene.AddObjectToScene(obj);
            }
        }


        void Field_ResourceBallChanged(object sender, ResourceBallEventArg e)
        {
            if (e.BornOrDie)
            {
                e.ResourceBall.InitializeGraphics(renderSys);
                scene.Scene.AddObjectToScene(e.ResourceBall);
            }
            else
            {
                scene.Scene.RemoveObjectFromScene(e.ResourceBall);
            }
        }

        public void Render(Sprite sprite)
        {
            ingameUI.Render(sprite);
        }
        public void Render()
        {
            if (!isLoaded)
            {
                scene.ActivateVisibles();
            }
            else
            {
                scene.RenderScene();
            }
        }

        //public ScoreEntry[] ResultScore
        //{
        //    get;
        //    private set;
        //} 

        public void Update(GameTime time)
        {
            if (!IsPaused)
            {
                scene.Update(time);
            }
            soundWorld.Update(time);

            RtsCamera camera = scene.Camera;

            SoundManager.Instance.ListenerPosition = camera.Position;// PlanetEarth.GetPosition(camera.Longitude, camera.Latitude);
            // scene.Camera.ViewMatrix.TranslationValue;

            if (!IsLoaded)
            {
                bool newVal = TerrainMeshManager.Instance.IsIdle &
                    ModelManager.Instance.IsIdle &
                    TextureManager.Instance.IsIdle &
                    TreeBatchModelManager.Instance.IsIdle;

                if (newVal)
                {
                    if (--loadingCountDown < 0)
                    {
                        IsLoaded = true;
                    }
                }
                else
                {
                    if (++loadingCountDown > 100)
                        loadingCountDown = 100;
                }

                int ldop = TerrainMeshManager.Instance.GetCurrentOperationCount();
                ldop += ModelManager.Instance.GetCurrentOperationCount();
                ldop += TextureManager.Instance.GetCurrentOperationCount();
                ldop += TreeBatchModelManager.Instance.GetCurrentOperationCount();

                if (ldop > maxLoadingOp)
                    maxLoadingOp = ldop;

                if (maxLoadingOp > 0)
                {
                    float newPrg = 1 - ldop / (float)maxLoadingOp;
                    if (newPrg > LoadingProgress)
                        LoadingProgress = newPrg;
                }

                ingameUI.Update(time);
            }
            else
            {

                //if (gameState.CheckGameOver())
                //{
                    //if (!IsOver)
                    //{
                        //ResultScore = gameState.GetScores();
                        //ScoreEntry[] entries = gameState.GetScores();
                        //ingameUI.ShowScore(entries);
                        //IsOver = true;
                    //}
                //}
                //else
                {
                    if (!IsPaused)
                    {
                        gameState.Update(time);
                        if (gameState.CheckGameOver())
                        {
                            ingameUI.ShowFinished();
                        }
                    }
                }

                ingameUI.Update(time);

            }
        }

        #region IDisposable 成员
        public bool Disposed
        {
            get;
            private set;
        }
        public void Dispose()
        {
            if (!Disposed)
            {
                ingameUI.Dispose();
                ingameUI = null;


                scene = null;
                soundWorld = null;
                gameState = null;

                Disposed = true;
            }
            else 
            {
                throw new ObjectDisposedException(ToString());
            }
   
        }

        #endregion
    }

}
