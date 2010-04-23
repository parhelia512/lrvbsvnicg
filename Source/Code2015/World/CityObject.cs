﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Apoc3D;
using Apoc3D.Collections;
using Apoc3D.Core;
using Apoc3D.Graphics;
using Apoc3D.Graphics.Animation;
using Apoc3D.Graphics.Effects;
using Apoc3D.MathLib;
using Apoc3D.Scene;
using Apoc3D.Vfs;
using Code2015.BalanceSystem;
using Code2015.EngineEx;
using Code2015.Logic;
using Code2015.World.Screen;
using Code2015.ParticleSystem;

namespace Code2015.World
{
    public enum PluginPositionFlag
    {
        None = 0,
        P1 = 1,
        P2 = 1 << 1,
        P3 = 1 << 2,
        P4 = 1 << 3
    }

    public delegate void CityVisibleHander(CityObject obj);    

    public class CityObject : SceneObject, ISelectableObject
    {
        struct PluginEntry
        {
            public CityPlugin plugin;
            public PluginPositionFlag position;

            /// <summary>
            ///  附加物的变换矩阵
            /// </summary>
            public Matrix transform;
            //public MdgResource CurrentPiece;
        }

        class SmokeEffectBuffer
        {
            SmokeEffect[] smokes = new SmokeEffect[CityGrade.LargePluginCount];
            bool[] activeState = new bool[CityGrade.LargePluginCount];
            RenderSystem renderSys;
            CityObject city;

            public SmokeEffectBuffer(RenderSystem rs, CityObject city)
            {
                this.city = city;
                this.renderSys = rs;
                for (int i = 0; i < smokes.Length; i++)
                {
                    smokes[i] = new SmokeEffect(rs);
                    smokes[i].Modifier = new SmokeModifier();

                    SmokeEmitter se = new SmokeEmitter();
                    se.Up = city.Transformation.Up;
                    se.Right = city.Transformation.Right;
                    se.Front = city.Transformation.Forward;

                    smokes[i].Emitter = se;
                }

            }

            public void RenderNotify()
            {
                for (int i = 0; i < activeState.Length; i++)
                    activeState[i] = false;
            }

            public RenderOperation[] GetRenderOperation(int idx)
            {
                ((SmokeEmitter)smokes[idx].Emitter).Position = Vector3.TransformSimple(city.GetPluginPosition(idx) + Vector3.UnitY * 100, city.Transformation);

                activeState[idx] = true;
                return smokes[idx].GetRenderOperation();
            }

            public void Update(GameTime time) 
            {
                for (int i = 0; i < activeState.Length; i++)
                {
                    if (activeState[i])
                    {
                        smokes[i].Update(time);
                    }
                }
            }
        }


        SoundObject sound;

        City city;
        CityStyle style;
        CityOwnerRing sideRing;


        CityGoalSite goalSite;

        RenderSystem renderSys;
        Map map;
        SmokeEffectBuffer smoke;

        FastList<Harvester> harvesters = new FastList<Harvester>();
        FastList<PluginEntry> plugins;

        
        PluginPositionFlag pluginFlags;

        FastList<RenderOperation> opBuffer = new FastList<RenderOperation>();

        Vector3 position;

        bool isSelected;
        //bool isVisible;
        #region 属性

        public MdgType MajorProblem
        {
            get { return city.MajorProblem; }
        }

        public CityGoalSite GoalSite
        {
            get { return goalSite; }
        }
        public bool IsLinked
        {
            get;
            set;
        }
        public bool IsUnlinked
        {
            get;
            set;
        }

        public City City
        {
            get { return city; }
        }
        public string Name
        {
            get { return city.Name; }
        }
        public Vector3 Position
        {
            get { return position; }
        }

        public float Satisfaction
        {
            get { return city.Satisfaction; }
        }
        public float Longitude
        {
            get { return city.Longitude; }
        }
        public float Latitude
        {
            get { return city.Latitude; }
        }
        public UrbanSize Size
        {
            get { return city.Size; }
        }
        public void Flash(int duration)
        {
            sideRing.Flash(duration);
        }
        //public bool IsPlayerCapturing(Player pl)
        //{
        //    return city.Capture.IsPlayerCapturing(pl);
        //}
        public bool CanCapture(Player pl)
        {
            return city.CanCapture(pl);
        }
        public bool IsCapturing
        {
            get { return city.Capture.IsCapturing; }
        }
        public bool IsCaptured
        {
            get { return city.IsCaptured; }
        }
        public CaptureState Capture
        {
            get { return city.Capture; }
        }
        public Player Owner
        {
            get { return city.Owner; }
        }

        public int HarvesterCount
        {
            get { return harvesters.Count; }
        }
        public Harvester GetHarvester(int idx)
        {
            return harvesters[idx];
        }

        public int PluginCount
        {
            get { return plugins.Count; }
        }
        #endregion

        public Matrix GetPluginTransform(int i)
        {
            return plugins[i].transform;
        }
        public Vector3 GetPluginPosition(int i)
        {

            return style.GetPluginTranslation(plugins[i].position);
        }
        public CityPlugin GetPlugin(int i)
        {
            return plugins[i].plugin;
        }

        

        bool ISelectableObject.IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
            }
        }

        public event CityVisibleHander CityVisible;

        public CityObject(RenderSystem rs, Map map, SceneManagerBase sceMgr, City city, CityStyleTable styleSet)
            : base(false)
        {
            this.city = city;
            this.city.Parent = this;
            base.SceneManager = sceMgr;
            this.map = map;

            this.plugins = new FastList<PluginEntry>();
            this.style = styleSet.CreateStyle(city.Culture);
            this.renderSys = rs;

            city.PluginAdded += City_PluginAdded;
            city.PluginRemoved += City_PluginRemoved;
            city.NearbyCityAdded += City_Linked;
            city.NearbyCityRemoved += City_UnLinked;

            city.CityOwnerChanged += City_OwnerChanged;

            float radLong = MathEx.Degree2Radian(city.Longitude);
            float radLat = MathEx.Degree2Radian(city.Latitude);

            float altitude = TerrainData.Instance.QueryHeight(radLong, radLat);
            Vector3 pos = PlanetEarth.GetPosition(radLong, radLat, PlanetEarth.PlanetRadius + TerrainMeshManager.PostHeightScale * altitude + 5);

            Transformation = PlanetEarth.GetOrientation(radLong, radLat);

            Transformation.TranslationValue = pos;
            BoundingSphere.Radius = CityStyleTable.CityRadius;
            BoundingSphere.Center = pos;
            position = pos;

            if (city.Owner != null)
                City_OwnerChanged(city.Owner);

            sideRing = new CityOwnerRing(this, style);
            goalSite = new CityGoalSite(rs, this, style);

            smoke = new SmokeEffectBuffer(rs, this);
            sound = SoundManager.Instance.MakeSoundObjcet("city", null, CityStyleTable.CityRadius * 2);
            sound.Position = pos;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                city.PluginAdded -= City_PluginAdded;
                city.PluginRemoved -= City_PluginRemoved;
            }
        }

        public void MakeLinkWith(CityObject city) 
        {

            CityLinkObject link = new CityLinkObject(renderSys, this, city);
            SceneManager.AddObjectToScene(link);

        }

        void City_UnLinked(City a, City b)
        {
            if (b != null)
            {
                IsUnlinked = true;
            }

        }
        void City_Linked(City a, City b)
        {
            if (b != null)
            {
                IsLinked = true;

                goalSite.ClearCapturePiece(b);
            }

        }
        void City_PluginAdded(City city, CityPlugin plugin)
        {
            PluginEntry ent = new PluginEntry();

            if ((pluginFlags & PluginPositionFlag.P1) == 0)
            {
                pluginFlags |= PluginPositionFlag.P1;
                ent.position = PluginPositionFlag.P1;
            }
            else if ((pluginFlags & PluginPositionFlag.P2) == 0)
            {
                pluginFlags |= PluginPositionFlag.P2;
                ent.position = PluginPositionFlag.P2;
            }
            else if ((pluginFlags & PluginPositionFlag.P3) == 0)
            {
                pluginFlags |= PluginPositionFlag.P3;
                ent.position = PluginPositionFlag.P3;
            }
            else if ((pluginFlags & PluginPositionFlag.P4) == 0)
            {
                pluginFlags |= PluginPositionFlag.P4;
                ent.position = PluginPositionFlag.P4;
            }
            ent.plugin = plugin;

            ent.transform = Matrix.Translation(style.GetPluginTranslation(ent.position));
            plugins.Add(ent);

            goalSite.SetDesired(plugins.Count - 1, CityGoalSite.GetDesired(plugin.TypeId));


            if (IsCaptured && (plugin.TypeId == CityPluginTypeId.OilRefinary ||
                plugin.TypeId == CityPluginTypeId.WoodFactory))
            {
                Harvester harv = new Harvester(renderSys, map, style.Cow);
                harv.Latitude = MathEx.Degree2Radian(Latitude - 2);
                harv.Longtitude = MathEx.Degree2Radian(Longitude);
                harvesters.Add(harv);
                base.SceneManager.AddObjectToScene(harv);

                NaturalResource res = plugin.CurrentResource;
                if (res != null)
                {
                    harv.SetAuto(
                        MathEx.Degree2Radian(res.Longitude), MathEx.Degree2Radian(res.Latitude),
                        MathEx.Degree2Radian(Longitude), MathEx.Degree2Radian(Latitude));
                }
            }
        }
        void City_PluginRemoved(City city, CityPlugin plugin)
        {
            for (int i = 0; i < plugins.Count; i++)
            {
                if (plugin == plugins[i].plugin)
                {
                    pluginFlags ^= plugins[i].position;
                    plugins.RemoveAt(i);
                    break;
                }
            }
        }
        void City_OwnerChanged(Player owner)
        {
            if (IsCaptured)
            {
                //Color4F color = new Color4F(owner.SideColor);
                //ringMaterial.Ambient *= color;
                //ringMaterial.Diffuse *= color;

                int hid = 0;
                for (int i = 0; i < plugins.Count; i++)
                {
                    CityPlugin plugin = plugins[i].plugin;

                    if (plugin.TypeId == CityPluginTypeId.OilRefinary ||
                        plugin.TypeId == CityPluginTypeId.WoodFactory)
                    {
                        NaturalResource res = plugin.CurrentResource;
                        if (res != null)
                        {
                            harvesters[hid++].SetAuto(
                                MathEx.Degree2Radian(res.Longitude), MathEx.Degree2Radian(res.Latitude),
                                MathEx.Degree2Radian(Longitude), MathEx.Degree2Radian(Latitude));
                        }
                    }
                }
            }
            else
            {
                int hid = 0;
                for (int i = 0; i < plugins.Count; i++)
                {
                    CityPlugin plugin = plugins[i].plugin;

                    if ((plugin.TypeId == CityPluginTypeId.OilRefinary ||
                        plugin.TypeId == CityPluginTypeId.WoodFactory))
                    {
                        harvesters[hid++].Move(MathEx.Degree2Radian(Longitude), MathEx.Degree2Radian(Latitude));
                    }
                }
            }
        }

        public override RenderOperation[] GetRenderOperation()
        {
            //isVisible = true;
            opBuffer.FastClear();
            if (CityVisible != null)
            {
                CityVisible(this);
            }

            RenderOperation[] ops = style.Base[(int)city.Size].GetRenderOperation();
            if (ops != null)
                opBuffer.Add(ops);

            ops = style.Urban[(int)city.Size].GetRenderOperation();
            if (ops != null)
                opBuffer.Add(ops);

            smoke.RenderNotify();
            for (int i = 0; i < plugins.Count; i++)
            {
                ops = null;
                switch (plugins[i].plugin.TypeId)
                {
                    case CityPluginTypeId.BiofuelFactory:
                        ops = style.BiofuelFactory.GetRenderOperation();
                        break;
                    case CityPluginTypeId.EducationOrg:
                        ops = style.EducationOrgan.GetRenderOperation();
                        break;
                    case CityPluginTypeId.Hospital:
                        ops = style.Hospital.GetRenderOperation();
                        break;
                    case CityPluginTypeId.OilRefinary:
                        ops = style.OilRefinary.GetRenderOperation();

                        RenderOperation[] ops2 = smoke.GetRenderOperation(i);
                        if (ops2 != null)
                        {
                            opBuffer.Add(ops2);
                        }
                        break;
                    case CityPluginTypeId.WoodFactory:
                        ops = style.BiofuelFactory.GetRenderOperation();
                        break;
                }
                if (ops != null)
                {
                    for (int j = 0; j < ops.Length; j++)
                    {
                        ops[j].Transformation *= plugins[i].transform;
                    }
                    opBuffer.Add(ops);
                }
            }

            for (int i = 0; i < city.FarmLandCount; i++)
            {
                ops = style.FarmLand.GetRenderOperation();
                if (ops != null)
                {
                    for (int j = 0; j < ops.Length; j++)
                    {
                        ops[j].Transformation *= CityStyleTable.FarmTransform[i];
                    }

                    opBuffer.Add(ops);
                }
            }


            if (isSelected)
            {
                ops = style.SelRing.GetRenderOperation();
                if (ops != null)
                {
                    opBuffer.Add(ops);
                }
            }

            ops = sideRing.GetRenderOperation();
            if (ops != null)
            {
                opBuffer.Add(ops);
            }
            ops = goalSite.GetRenderOperation();
            if (ops != null)
            {
                opBuffer.Add(ops);
            }

            opBuffer.Trim();
            return opBuffer.Elements;
        }

        public void UpgradeAI()
        {
            for (int i = 0; i < plugins.Count; i++)
            {
                plugins[i].plugin.Upgrade(CityPlugin.UpgradeAmount);
            }
        }
        void Upgrade()
        {
            // 升级
            for (int i = 0; i < plugins.Count; i++)
            {
                if (goalSite.Match(i, plugins[i].plugin.TypeId))
                {
                    plugins[i].plugin.Upgrade(CityPlugin.UpgradeAmount);
                    goalSite.ClearAt(i);
                }
            }
        }
        public bool TryLink(int goalIdx, MdgType type, out City target)
        {
            Vector3 t = CityStyleTable.SiteTransform[goalIdx].TranslationValue;
            t = Vector3.TransformNormal(t, Transformation);
            t.Normalize();

            for (int i = 0; i < city.LinkableCityCount; i++)
            {
                Vector3 dir = city.GetLinkableCity(i).Parent.position - position;
                dir.Normalize();

                float dot = Vector3.Dot(ref t, ref dir);
                if (dot > 0.5f)
                {
                    target = city.GetLinkableCity(i);
                    if (CityGoalSite.CompareCategory(target.MajorProblem, type))
                    {
                        return true;
                    }
                }
            }
            target = null;
            return false;
        }

        void TryFarm()
        {
            for (int i = 0; i < CityGoalSite.SiteCount; i++)
            {
                if (goalSite.GetPieceType(i) != MdgType.Hunger || !goalSite.HasPiece(i))
                {
                    return;
                }
            }

            goalSite.Clear();
            city.AddFarm();
        }

        public bool TryUpgrade()
        {
            TryFarm();
            if (MatchSite())
            {
                Upgrade();
                return true;
            }
            return false;
        }

        public bool MatchSite()
        {
            if (plugins.Count == 0)
                return false;

            bool result = true;
            for (int i = 0; i < plugins.Count; i++)
            {
                result &= goalSite.Match(i, plugins[i].plugin.TypeId);
                if (!result)
                    break;
            }
            return result;
        }

        public override void Update(GameTime dt)
        {
            BoundingSphere.Radius = CityStyleTable.CityRadius;

            sideRing.Update(dt);
            //smoke.RenderNotify();
            smoke.Update(dt);
            sound.Update(dt);
        }


        public override bool IsSerializable
        {
            get { return false; }
        }
    }
}
