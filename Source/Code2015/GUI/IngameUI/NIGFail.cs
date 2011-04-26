﻿using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.GUI.Controls;
using Apoc3D.MathLib;
using Apoc3D.Vfs;
using Code2015.EngineEx;
using Code2015.Logic;
using Code2015.World;

namespace Code2015.GUI.IngameUI
{
    class NIGFail : UIComponent
    {
        GameScene scene;
        GameState gameLogic;
        RenderSystem renderSys;
        Code2015 game;
        Game parent;

        Texture overlay;
        Texture background;
        Texture failTex;


        Button backButton;
        Button replayButton;

        Texture rankBackground;
        Texture homeBackground;
        Texture rankColor;

        GameFontRuan f8;
        GameFontRuan f6;


        float showPrg;
        NIGDialogState state;


        public NIGFail(Code2015 game, Game parent, GameScene scene, GameState gamelogic)
        {
            this.parent = parent;
            this.game = game;
            this.renderSys = game.RenderSystem;
            this.scene = scene;
            this.gameLogic = gamelogic;
         
            FileLocation fl = FileSystem.Instance.Locate("nig_fail_bg.tex", GameFileLocs.GUI);
            background = UITextureManager.Instance.CreateInstance(fl);

            fl = FileSystem.Instance.Locate("nig_fail_fail.tex", GameFileLocs.GUI);
            failTex = UITextureManager.Instance.CreateInstance(fl);

            fl = FileSystem.Instance.Locate("nig_result_back.tex", GameFileLocs.GUI);
            backButton = new Button();
            backButton.Image = UITextureManager.Instance.CreateInstance(fl);
            backButton.X = 450;
            backButton.Y = 530;
            backButton.Width = backButton.Image.Width;
            backButton.Height = backButton.Image.Height;
            backButton.Enabled = true;
            backButton.IsValid = true;
            backButton.MouseClick += BackButton_Click;

            fl = FileSystem.Instance.Locate("nig_result_replay.tex", GameFileLocs.GUI);
            replayButton = new Button();
            replayButton.Image = UITextureManager.Instance.CreateInstance(fl);
            replayButton.X = 738;
            replayButton.Y = 512;
            replayButton.Width = replayButton.Image.Width;
            replayButton.Height = replayButton.Image.Height;
            replayButton.Enabled = true;
            replayButton.IsValid = true;
            replayButton.MouseClick += ReplayButton_Click;

            fl = FileSystem.Instance.Locate("nig_rank_bk.tex", GameFileLocs.GUI);
            rankBackground = UITextureManager.Instance.CreateInstance(fl);

            fl = FileSystem.Instance.Locate("nig_home.tex", GameFileLocs.GUI);
            homeBackground = UITextureManager.Instance.CreateInstance(fl);

            fl = FileSystem.Instance.Locate("nig_rank_color.tex", GameFileLocs.GUI);
            rankColor = UITextureManager.Instance.CreateInstance(fl);

            f6 = GameFontManager.Instance.FRuanEdged4;
            f8 = GameFontManager.Instance.FRuanEdged6;


            
            fl = FileSystem.Instance.Locate("bg_black.tex", GameFileLocs.GUI);
            overlay = UITextureManager.Instance.CreateInstance(fl);
            state = NIGDialogState.Hiding;

        }

        public bool IsHidden
        {
            get { return state == NIGDialogState.Hiding; }
        }
        public void Show()
        {
            state = NIGDialogState.MovingIn;
        }
        public void Hide()
        {
            state = NIGDialogState.MovingOut;
        }

        public override int Order
        {
            get
            {
                return 96;
            }
        }

        public override bool HitTest(int x, int y)
        {
            if (state != NIGDialogState.Hiding)
            {
                return true;
            }
            return false;
        }

        void ReplayButton_Click(object sender, MouseButtonFlags btn)
        {
            if (btn == MouseButtonFlags.Left)
            {
                Hide();
            }
        }


        void BackButton_Click(object sender, MouseButtonFlags btn)
        {
            if (btn == MouseButtonFlags.Left)
            {
                parent.Over();
                Hide();
            }
        }

        float clearProgress = 1.3f;
        public override void Update(Apoc3D.GameTime time)
        {
            if (state == NIGDialogState.MovingOut)
            {
                showPrg -= time.ElapsedGameTimeSeconds * 8;
                if (showPrg < 0)
                {
                    showPrg = 0;
                    state = NIGDialogState.Hiding;
                }
            }
            else if (state == NIGDialogState.MovingIn)
            {
                showPrg += time.ElapsedGameTimeSeconds * 8;
                if (showPrg > 1)
                {
                    showPrg = 1;
                    state = NIGDialogState.Showing;
                }
            }
            if (state == NIGDialogState.Showing)
            {
                replayButton.Update(time);
                backButton.Update(time);

                float dt = (float)time.ElapsedGameTime.TotalSeconds;
                if (clearProgress > 0)
                {
                    clearProgress -= dt;
                }
                else
                {
                    clearProgress = 0;
                }
            }
            if (state == NIGDialogState.Hiding)
            {
                clearProgress = 1.8f;
            }
        }

        public override void UpdateInteract(Apoc3D.GameTime time)
        {
            if (state == NIGDialogState.Showing)
            {
                replayButton.Update(time);
                backButton.Update(time);
            }
        }

        public override void Render(Sprite sprite)
        {
            if (state != NIGDialogState.Hiding)
            {
                ColorValue overlayColor = ColorValue.Transparent;
                overlayColor.A = (byte)(165 * showPrg);

                sprite.Draw(overlay, 0, 0, overlayColor);


                int x = 373;
                int y = 54;


                Matrix trans = Matrix.Translation(-background.Width / 2, -background.Height / 2, 0) *
                    Matrix.Scaling(showPrg * 0.8f + 0.2f, 1, 1) *
                    Matrix.Translation(x + background.Width / 2, y + background.Height / 2, 0);

                sprite.SetTransform(trans);
                sprite.Draw(background, 0, 0, ColorValue.White);
                sprite.SetTransform(Matrix.Identity);

            }
            if (state == NIGDialogState.Showing)
            {
                //sprite.Draw(failTex, 661, 54, ColorValue.White);

                if (clearProgress > 1)
                {
                    float scale = -MathEx.Sqr(MathEx.Saturate(1 * 6) * 1.5f - 1) + 1;
                    scale = 0.5f + 1.5f * scale;

                    Matrix trans = Matrix.Scaling(scale, scale, 1) *
                        Matrix.Translation(661 + failTex.Width / 2, 54 + failTex.Height / 2, 0);

                    sprite.SetTransform(trans);
                    sprite.Draw(failTex, -failTex.Width / 2, -failTex.Height / 2, ColorValue.White);
                    sprite.SetTransform(Matrix.Identity);
                }

                if (clearProgress <= 1)
                {
                    float scale = -MathEx.Sqr(MathEx.Saturate(clearProgress * 6) * 1.5f - 1) + 1;
                    scale = 0.5f + 1.5f * scale;

                    Matrix trans = Matrix.Scaling(scale, scale, 1) *
                        Matrix.Translation(661 + failTex.Width / 2, 54 + failTex.Height / 2, 0);

                    sprite.SetTransform(trans);
                    sprite.Draw(failTex, -failTex.Width / 2, -failTex.Height / 2, ColorValue.White);
                    sprite.SetTransform(Matrix.Identity);
               
                }

                sprite.Draw(backButton.Image, backButton.X, backButton.Y, ColorValue.White);
                if (backButton.IsMouseOver)
                {
                    sprite.Draw(backButton.Image, backButton.X, backButton.Y - 4, ColorValue.White);
                }

                sprite.Draw(replayButton.Image, replayButton.X, replayButton.Y, ColorValue.White);
                if (replayButton.IsMouseOver)
                {
                    sprite.Draw(replayButton.Image, replayButton.X, replayButton.Y - 4, ColorValue.White);
                }

                RenderRank(sprite);


            }
        }

        public void RenderRank(Sprite sprite)
        {
            List<Player> players = new List<Player>();
            for (int i = 0; i < gameLogic.LocalPlayerCount; i++)
                players.Add(gameLogic.GetLocalPlayer(i));

            RankInfo.StatisticRank(players);

            int startX = 543;
            int startY = 160;

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Type != PlayerType.LocalHuman)
                {
                    string s = (i + 1).ToString();
                    sprite.Draw(rankBackground, startX, startY, ColorValue.White);
                    f8.DrawString(sprite, s, startX, startY - 20, ColorValue.White);


                    Matrix trans = Matrix.Scaling(0.33f, 0.33f, 1) * Matrix.Translation(new Vector3(startX + 19, startY + 14, 0));
                    sprite.SetTransform(trans);

                    f8.DrawString(sprite, "TH", 0, 0, ColorValue.White);
                    sprite.SetTransform(Matrix.Identity);

                    sprite.Draw(rankColor, startX + 35, startY + 9, players[i].SideColor);
                    f8.DrawString(sprite, players[i].Area.CityCount.ToString(), startX + 80, startY - 20, ColorValue.White);
                    startY += 37;
                }
                else
                {
                    string s = (i + 1).ToString();
                    sprite.Draw(homeBackground, startX, startY - 4, players[i].SideColor);
                    f8.DrawString(sprite, s, startX + 2, startY, ColorValue.White);

                    Matrix trans = Matrix.Scaling(0.35f, 0.35f, 1) * Matrix.Translation(new Vector3(startX + 22, startY + 35, 0));
                    sprite.SetTransform(trans);
                    f8.DrawString(sprite, "TH", 0, 0, ColorValue.White);
                    sprite.SetTransform(Matrix.Identity);

                    f6.DrawString(sprite, players[i].Area.CityCount.ToString(), startX + 73, startY - 45, ColorValue.White);
                    startY += 70;
                }
            }
        }
    }
}
