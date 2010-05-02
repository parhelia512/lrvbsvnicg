﻿using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D;
using Apoc3D.Collections;
using Apoc3D.Graphics;
using Apoc3D.GUI.Controls;
using Apoc3D.MathLib;
using Apoc3D.Vfs;
using Code2015.EngineEx;
using Code2015.World;

namespace Code2015.GUI
{
    class ScoreScreen2 : UIComponent
    {
        Code2015 parent;
        RenderSystem renderSys;
        Menu menu;
        Texture cursor;
        Point mousePosition;
        NormalSoundObject mouseHover;
        NormalSoundObject mouseDown;

        FastList<ScoreEntry> scores = new FastList<ScoreEntry>();

        Texture background;

        RoundButton countinue;
        GameFont font;


        int index;
        Texture[] plates;

        public void Add(ScoreEntry entry)
        {
            scores.Add(entry);

            string color;

            if (entry.Player.SideColor == ColorValue.Red)
            {
                color = "r";
            }
            else if (entry.Player.SideColor == ColorValue.Green)
            {
                color = "g";
            }
            else if (entry.Player.SideColor == ColorValue.Yellow)
            {
                color = "y";
            }
            else
            {
                color = "b";
            }
            FileLocation fl = FileSystem.Instance.Locate("ss_" + (index + 1).ToString() + color + ".tex", GameFileLocs.GUI);
            plates[index++] = UITextureManager.Instance.CreateInstance(fl);
        }
        public void Clear()
        {
            scores.Clear();
        }

        public ScoreScreen2(Code2015 parent, Menu menu)
        {
            this.menu = menu;
            this.parent = parent;
            this.renderSys = parent.RenderSystem;

            FileLocation fl = FileSystem.Instance.Locate("ss_bg.tex", GameFileLocs.GUI);
            background = UITextureManager.Instance.CreateInstance(fl);



            font = GameFontManager.Instance.F18;

            countinue = new RoundButton();
            countinue.Enabled = true;
            countinue.IsValid = true;
            //fl = FileSystem.Instance.Locate("ss_continue.tex", GameFileLocs.GUI);
            //countinue.Image = UITextureManager.Instance.CreateInstance(fl);

            countinue.X = 357;
            countinue.Y = 493;
            //countinue.Width = countinue.Image.Width;
            //countinue.Height = countinue.Image.Height;
            //fl = FileSystem.Instance.Locate("ss_continuehouver.tex", GameFileLocs.GUI);
            //countinue.ImageMouseOver = UITextureManager.Instance.CreateInstance(fl);


            fl = FileSystem.Instance.Locate("cursor.tex", GameFileLocs.GUI);
            cursor = UITextureManager.Instance.CreateInstance(fl);
            mouseHover = (NormalSoundObject)SoundManager.Instance.MakeSoundObjcet("buttonHover", null, 0);
            mouseDown = (NormalSoundObject)SoundManager.Instance.MakeSoundObjcet("buttonDown", null, 0);

            plates = new Texture[4];
        }

        public override void Render(Sprite sprite)
        {

            sprite.Draw(background, 0, 0, ColorValue.White);


            ColorValue ftc = scores.Elements[0].Player.SideColor;
            
            if (plates[0] != null) 
            {
                sprite.Draw(plates[0], 0, 0, ColorValue.White);
            }

            string msg = ((int)Math.Round(scores.Elements[0].Development)).ToString("G");
            font.DrawString(sprite, msg, 412, 298, ColorValue.White);

            msg = ((int)Math.Round(scores.Elements[0].CO2)).ToString("G");
            font.DrawString(sprite, msg, 657, 298, ColorValue.White);

            msg = ((int)Math.Round(scores.Elements[0].Total)).ToString("G");
            font.DrawString(sprite, msg, 892, 299, ColorValue.White);



            ftc = scores.Elements[1].Player.SideColor;

            if (plates[1] != null)
            {
                sprite.Draw(plates[1], 0, 0, ColorValue.White);
            }

            msg = ((int)Math.Round(scores.Elements[1].Development)).ToString("G");
            font.DrawString(sprite, msg, 412, 360, ColorValue.White);

            msg = ((int)Math.Round(scores.Elements[1].CO2)).ToString("G");
            font.DrawString(sprite, msg, 657, 360, ColorValue.White);

            msg = ((int)Math.Round(scores.Elements[1].Total)).ToString("G");
            font.DrawString(sprite, msg, 892, 360, ColorValue.White);



            ftc = scores.Elements[2].Player.SideColor;

            if (plates[2] != null)
            {
                sprite.Draw(plates[2], 0, 0, ColorValue.White);
            }


            msg = ((int)Math.Round(scores.Elements[1].Development)).ToString("G");
            font.DrawString(sprite, msg, 412, 423, ColorValue.White);

            msg = ((int)Math.Round(scores.Elements[1].CO2)).ToString("G");
            font.DrawString(sprite, msg, 657, 423, ColorValue.White);

            msg = ((int)Math.Round(scores.Elements[1].Total)).ToString("G");
            font.DrawString(sprite, msg, 892, 423, ColorValue.White);


            ftc = scores.Elements[3].Player.SideColor;

            if (plates[3] != null)
            {
                sprite.Draw(plates[3], 0, 0, ColorValue.White);
            }


            msg = ((int)Math.Round(scores.Elements[2].Development)).ToString("G");
            font.DrawString(sprite, msg, 412, 489, ColorValue.White);

            msg = ((int)Math.Round(scores.Elements[2].CO2)).ToString("G");
            font.DrawString(sprite, msg, 657, 489, ColorValue.White);

            msg = ((int)Math.Round(scores.Elements[2].Total)).ToString("G");
            font.DrawString(sprite, msg, 892, 489, ColorValue.White);


            countinue.Render(sprite);

            sprite.Draw(cursor, mousePosition.X, mousePosition.Y, ColorValue.White);
        }

        public override void Update(GameTime time)
        {
            mousePosition.X = MouseInput.X;
            mousePosition.Y = MouseInput.Y;
            countinue.Update(time);
        }
    }
}
