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
using Apoc3D;
using Apoc3D.Collections;
using Apoc3D.Graphics;
using Apoc3D.Graphics.Animation;
using Apoc3D.MathLib;
using Apoc3D.Scene;
using Apoc3D.Vfs;
using Code2015.EngineEx;
using Code2015.Logic;

namespace Code2015.World
{
    class CitySelectionMarker : SceneObject
    {
        public const float LinkWidthScale = 0.006f;
        public const float LinkHeightScale = 4 * 1f / LinkBaseLength;
        public const float LinkBaseLength = 100;

        const float RingRadius = 100;

        Player player;
        City mouseHoverCity;
        City start;
        City[] targets;

        Model inner_marker;
        Model outter_marker;

        Model[] linkArrow;

        FastList<RenderOperation> opBuffer = new FastList<RenderOperation>();

        public City MouseHoverCity
        {
            get { return mouseHoverCity; }
            set
            {
                mouseHoverCity = value;
                if (mouseHoverCity != null)
                {
                    float s = City.CitySelRingScale * (City.CityOutterRadius) / RingRadius;
                    Matrix scale = Matrix.Scaling(s, 1, s);

                    outter_marker.CurrentAnimation.Clear();
                    outter_marker.CurrentAnimation.Add(new NoAnimaionPlayer(scale * mouseHoverCity.Transformation));
                }
            }
        }

        public void SetCity(City start, City[] targets)
        {
            this.start = start;
            this.targets = targets;

            if (targets != null)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    City end = targets[i];

                    Vector3 dir = end.Position - start.Position;

                    dir.Normalize();

                    Vector3 pa = start.Position + dir * (City.CityRadius + 130);
                    Vector3 pb = end.Position - dir * (City.CityRadius + 130);


                    float dist = Vector3.Distance(pa, pb);

                    float longitude = MathEx.Degree2Radian(MathEx.LinearInterpose(start.Longitude, end.Longitude, 0.3f));
                    float latitude = MathEx.Degree2Radian(MathEx.LinearInterpose(start.Latitude, end.Latitude, 0.3f));

                    float scale = dist;
                    Matrix ori = Matrix.Identity;
                    ori.Right = Vector3.Normalize(pa - pb);
                    ori.Up = Vector3.Normalize(pa);
                    ori.Forward = Vector3.Normalize(Vector3.Cross(ori.Up, ori.Right));
                    ori.TranslationValue = pa + ori.Up * 75;

                    linkArrow[i].CurrentAnimation.Clear();
                    linkArrow[i].CurrentAnimation.Add(new NoAnimaionPlayer(
                        Matrix.Scaling(dist / LinkBaseLength, 1 + LinkHeightScale, 1 + LinkWidthScale * dist) *
                        ori));

                }
            }

            if (start != null)
            {
                float s = City.CitySelRingScale * (City.CityOutterRadius) / RingRadius;
                Matrix scale = Matrix.Scaling(s, 1, s);

                inner_marker.CurrentAnimation.Clear();
                inner_marker.CurrentAnimation.Add(new NoAnimaionPlayer(scale * start.Transformation));
            }
        }

        public CitySelectionMarker(RenderSystem rs, Player player)
            : base(false)
        {
            this.player = player;
            this.linkArrow = new Model[4];

            ColorValue color = player.SideColor;
            string fileName;
            if (color == ColorValue.Red)
            {
                fileName = "link_e_red.mesh";
            }
            else if (color == ColorValue.Green)
            {
                fileName = "link_e_green.mesh";
            }
            else if (color == ColorValue.Blue)
            {
                fileName = "link_e_blue.mesh";
            }
            else
            {
                fileName = "link_e_yellow.mesh";
            }

            FileLocation fl = FileSystem.Instance.Locate(fileName, GameFileLocs.Model);
            for (int i = 0; i < linkArrow.Length; i++)
            {
                linkArrow[i] = new Model(ModelManager.Instance.CreateInstance(rs, fl));
            }
            fl = FileSystem.Instance.Locate("citysel_inner.mesh", GameFileLocs.Model);
            inner_marker = new Model(ModelManager.Instance.CreateInstance(rs, fl));

            fl = FileSystem.Instance.Locate("citysel_outter.mesh", GameFileLocs.Model);
            outter_marker = new Model(ModelManager.Instance.CreateInstance(rs, fl));

            BoundingSphere.Radius = float.MaxValue;
            Transformation = Matrix.Identity;
        }

        #region IRenderable 成员

        public override RenderOperation[] GetRenderOperation()
        {
            opBuffer.FastClear();

            if (targets != null)
            {
                if (start.IsCaptured && start.Owner == player)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (!targets[i].IsCaptured)
                        {
                            RenderOperation[] ops = linkArrow[i].GetRenderOperation();
                            if (ops != null)
                            {
                                opBuffer.Add(ops);
                            }
                        }
                    }
                }
            }
            if (start != null)
            {
                RenderOperation[] ops = inner_marker.GetRenderOperation();
                if (ops != null)
                {
                    opBuffer.Add(ops);
                }

            }
            if (MouseHoverCity != null)
            {
                RenderOperation[] ops = outter_marker.GetRenderOperation();
                if (ops != null)
                {
                    opBuffer.Add(ops);
                }
            } 
            opBuffer.Trim();
            return opBuffer.Elements;
        }

        public override RenderOperation[] GetRenderOperation(int level)
        {
            return GetRenderOperation();
        }

        #endregion

        public override void Update(GameTime time)
        {

        }

        public override bool IsSerializable
        {
            get { return false; }
        }
    }
}