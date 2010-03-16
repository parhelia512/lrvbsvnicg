﻿using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D;
using Apoc3D.Graphics;
using Apoc3D.Graphics.Animation;
using Apoc3D.MathLib;
using Apoc3D.Scene;
using Apoc3D.Vfs;
using Code2015.EngineEx;
using Code2015.Logic;

namespace Code2015.World
{
    enum UnitState
    {
        TargetAuto,
        HomeAuto
    }

    public class Harvester : DynamicObject
    {
        float longtitude;
        float latitude;

        PathFinder finder;
        UnitState state;

        float autoSLng;
        float autoSLat;
        float autoTLng;
        float autoTLat;

        int destX; 
        int destY;


        Quaternion targetOri;
        Quaternion srcOri;

        bool rotUpdated;

        float currentPrg;
        int currentNode;
        PathFinderResult cuurentPath;

        const float Speed = 1;

        public float Longtitude 
        {
            get { return longtitude; }
            set { longtitude = value; }
        }
        public float Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }


        public bool IsAuto
        {
            get;
            private set;
        }

        public Harvester(RenderSystem rs, Map map,  Model mdl)
        {
            finder = map.PathFinder.CreatePathFinder();

            ModelL0 = mdl;
            BoundingSphere.Radius = 50;
            
        }

        public void SetAuto(float tlng, float tlat, float slng, float slat)
        {
            IsAuto = true;
           
            autoSLat = slat;
            autoSLng = slng;
            autoTLat = tlat;
            autoTLng = tlng;

            move(autoTLng, autoTLat);
            state = UnitState.TargetAuto;
        }

        void move(float lng, float lat) 
        {
            int sx, sy;
            Map.GetMapCoord(longtitude, latitude, out sx, out sy);

            int tx, ty;
            Map.GetMapCoord(lng, lat, out tx, out ty);

            destX = tx;
            destY = ty;

            finder.Reset();
            cuurentPath = finder.FindPath(sx, sy, tx, ty);
            currentNode = 0;
            currentPrg = 0;
        }
        public void Move(float lng, float lat)
        {
            IsAuto = false;
            move(lng, lat);
        }
        void Move(int x, int y)
        {

            int sx, sy;
            Map.GetMapCoord(longtitude, latitude, out sx, out sy);

            destX = x;
            destY = y;

            //finder.Continue();
            //cuurentPath = finder.FindPath(sx, sy, x, y);
            finder.Reset();
            cuurentPath = finder.FindPath(sx, sy, x, y);
            currentNode = 0;
            currentPrg = 0;
        }

        Quaternion GetOrientation(Point pa, Point pb)
        {
            float alng;
            float alat;
            float blng;
            float blat;

            Map.GetCoord(pa.X, pa.Y, out alng, out alat);
            Map.GetCoord(pb.X, pb.Y, out blng, out blat);

            Vector3 n = PlanetEarth.GetNormal(alng, alat);
            Vector3 posA = PlanetEarth.GetPosition(alng, alat);
            Vector3 posB = PlanetEarth.GetPosition(blng, blat);

            Vector3 dir = posB - posA;
            dir.Normalize();
            Vector3 bi = Vector3.Cross(n, dir);
            bi.Normalize();

            Matrix result = Matrix.Identity;
            result.Right = bi;
            result.Up = n;
            result.Forward = -dir;
            return Quaternion.RotationMatrix(result);
        }

        public override void Update(GameTime dt)
        {
            Orientation = Matrix.Identity;

            if (cuurentPath != null)
            {
                int nextNode = currentNode + 1;

                if (nextNode >= cuurentPath.NodeCount)
                {
                    nextNode = 0;
                    currentPrg = 0;

                    if (cuurentPath.RequiresPathFinding)
                    {
                        Move(destX, destY);
                    }
                    else
                    {
                        cuurentPath = null;
                        if (IsAuto)
                        {
                            if (state == UnitState.HomeAuto)
                            {
                                move(autoTLng, autoTLat);
                                state = UnitState.TargetAuto;
                            }
                            else if (state == UnitState.TargetAuto)
                            {
                                move(autoSLng, autoSLat);
                                state = UnitState.HomeAuto;
                            }

                        }
                    }
                }
                else
                {
                    Point np = cuurentPath[nextNode];
                    Point cp = cuurentPath[currentNode];

                    if (currentPrg > 0.5f && !rotUpdated)
                    {
                        if (nextNode < cuurentPath.NodeCount - 1)
                        {
                            srcOri = GetOrientation(cp, np);
                            targetOri = GetOrientation(np, cuurentPath[nextNode + 1]);
                        }
                        else
                        {
                            targetOri = GetOrientation(cp, np);
                            srcOri = targetOri;
                        }
                        rotUpdated = true;
                    }


                    float x = MathEx.LinearInterpose(cp.X, np.X, currentPrg);
                    float y = MathEx.LinearInterpose(cp.Y, np.Y, currentPrg);

                    Map.GetCoord(x, y, out longtitude, out latitude);

                    Orientation = Matrix.RotationQuaternion(
                        Quaternion.Slerp(srcOri, targetOri, currentPrg > 0.5f ? currentPrg - 0.5f : currentPrg + 0.5f));

                    currentPrg += 0.05f;

                    if (currentPrg > 1)
                    {
                        currentPrg = 0;
                        currentNode++;
                        rotUpdated = false;
                    }
                }
            }



            //Orientation *= PlanetEarth.GetOrientation(longtitude, latitude);
            Position = PlanetEarth.GetPosition(longtitude, latitude, PlanetEarth.PlanetRadius + 50);

            base.Update(dt);
        }


        public override bool IsSerializable
        {
            get { return false; ; }
        }
    }
}
