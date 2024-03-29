﻿using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D;
using Apoc3D.MathLib;
using Code2015.EngineEx;
using Code2015.Logic;
using Code2015.World;
using Apoc3D.Collections;

namespace Code2015.World
{
    /// <summary>
    ///  Represents cities with a harvester exploiting resources to produce resource balls
    /// </summary>
    class GatherCity : City
    {

        Harvester harvester;
        //NaturalResource exRes;

        int resourceIndex = 0;
        float resourceBuffer;

        /// <summary>
        ///  资源搜索范围
        ///  The distance that the city searches for resource to exploit
        /// </summary>
        float gatherDistance;

        bool isGatherOnHold;

        private FastList<NaturalResource> nearResource = new FastList<NaturalResource>();

        public NaturalResource ExResource
        {
            get
            {
                if (nearResource.Count == 0)
                    return null;
                return nearResource[resourceIndex];
            }
        }

        public Harvester Harvester { get { return harvester; } }
        public GatherCity(BattleField btfld, Player owner, CityType type)
            : base(btfld, owner, type)
        {
            harvester = new Harvester(this, btfld.Map);
            harvester.GotHome += Harv_Home;
            harvester.GotThere += Harv_Dest;

            Harvester.Props hprop = getHarvProps();
            harvester.SetProps(hprop);

            
            if (this.Type == CityType.Oil)
            {
                gatherDistance = RulesTable.OilGatherDistance;
            }
            else if (this.Type == CityType.Green)
            {
                gatherDistance = RulesTable.GreenGatherDistance;
            }
           
        }

        public bool IsResourceInRange(NaturalResource res)
        {
            return nearResource.Contains(res);
        }
        public float GetNearResourceCount()
        {
            NaturalResource[] resList = battleField.NaturalResources;
            float result = 0;
            for (int i = 0; i < resList.Length; i++)
            {
                if (resList[i].Type == NaturalResourceType.Wood)
                {
                    ForestObject forest = (ForestObject)resList[i];
                    float d = Vector3.Distance(forest.ForestCenter, this.Position);
                    if (d < gatherDistance)
                    {
                        result++;
                    }
                }
                else if (resList[i].Type == NaturalResourceType.Oil)
                {
                    float d = Vector3.Distance(resList[i].Position, this.Position);
                    if (d < gatherDistance)
                    {
                        result++;
                    }
                }

            }
            return result;
        }

        //public void SetTargetExResource(NaturalResource res)
        //{
        //    exRes = res;
        //}

        public override float GetProductionProgress()
        {
            float result;
            if (Type == CityType.Oil)
            {
                result = resourceBuffer / RulesTable.OilBallCost;
            }
            else
            {
                result = resourceBuffer / RulesTable.GreenBallCost;
            }
            return MathEx.Saturate(result);
        }

        public Harvester.Props getHarvProps()
        {
            Harvester.Props props = new Harvester.Props();
            props.HP = RulesTable.GreenHarvHP;
            props.Speed = RulesTable.GreenHarvSpeed;
            props.Storage = RulesTable.GreenHarvStorage;
            return props;                        
        }
        
        protected override void ChangeType()
        {
            base.ChangeType();
            FindResources(battleField.NaturalResources);
        }

        public void Gather(NaturalResource res)
        {
            if (!IsCaptured)
                return;

            int nid = nearResource.IndexOf(res);
            if (nid == -1)
                return;

            resourceIndex = nid;

            NaturalResource exRes = ExResource;
            if (exRes != null)
            {
                harvester.Move(MathEx.Degree2Radian(exRes.Longitude), MathEx.Degree2Radian(exRes.Latitude));
                harvester.SetMovePurpose(MovePurpose.Gather);
            }

            harvester.ExRes = exRes;

        }

        void Harv_Dest(object sender, EventArgs e)
        {

            harvester.Move(MathEx.Degree2Radian(Longitude), MathEx.Degree2Radian(Latitude));
            harvester.SetMovePurpose(MovePurpose.Home);

            // 自动返回
            //harvBackWait = getHarvWaitTime();
        }
        void Harv_Home(object sender, EventArgs e)
        {
            if (!isGatherOnHold)
            {
                NaturalResource exRes = ExResource;
                if (IsCaptured && exRes != null)
                {
                    harvester.Move(MathEx.Degree2Radian(exRes.Longitude), MathEx.Degree2Radian(exRes.Latitude));
                    harvester.SetMovePurpose(MovePurpose.Gather);
                }
                if (!harvester.IsFullLoaded && sender == harvester)
                {
                    FindNewNaturalResource();
                }

                harvester.ExRes = exRes;
            }

            //harvSendWait = getHarvWaitTime();
        }

        public override void ChangeOwner(Player player)
        {
            base.ChangeOwner(player);

            Harv_Home(null, EventArgs.Empty);
        }

        //public override void UpdateResource(GameTime gameTime)
        //{
        //    //开采资源
        //    //if (this.nearResource.Count > 0)
        //    //{
        //    //    float take = nearResource[resourceIndex].Exploit(10);
        //    //    if (take < 10)
        //    //    {
        //    //        FindNewNaturalResource();
        //    //    }
        //    //    resourceBuffer += take;
        //    //}

        //    harvester.Update(gameTime);

        //}

        //public override bool CanProduceRBall()
        //{
        //    return this.resourceBuffer > RulesTable.RBallProduceBall;
        //}

        ////产生小球
        //public override void ProduceBall()
        //{
        //    this.battleField.CreateResourceBall(this);
        //    resourceBuffer -= RulesTable.RBallProduceBall;
        //}


        //周围资源资源
        /// <summary>
        ///  Finds the nearby resources
        /// </summary>
        /// <param name="resList"></param>
        public void FindResources(NaturalResource[] resList)
        {
            nearResource.Clear();
            for (int i = 0; i < resList.Length; i++)
            {

                if (Type == CityType.Green)
                {
                    if (resList[i].Type == NaturalResourceType.Wood)
                    {
                        ForestObject forest = (ForestObject)resList[i];
                        float d = Vector3.Distance(forest.ForestCenter, this.Position);
                        if (d < gatherDistance)
                        {
                            nearResource.Add(resList[i]);
                        }
                    }
                }
                else if (Type == CityType.Oil)
                {
                    if (resList[i].Type == NaturalResourceType.Oil)
                    {
                        float d = Vector3.Distance(resList[i].Position, this.Position);
                        if (d < gatherDistance)
                        {
                            nearResource.Add(resList[i]);
                        }
                    }
                }
            }
            if (Type == CityType.Green)
            {
                nearResource.Sort(CamparisionForest);
            }
            else
            {
                nearResource.Sort(Camparision);
            }
        }
        public void FindResources(List<NaturalResource> resList)
        {
            nearResource.Clear();
            for (int i = 0; i < resList.Count; i++)
            {

                if (Type == CityType.Green)
                {
                    if (resList[i].Type == NaturalResourceType.Wood)
                    {
                        ForestObject forest = (ForestObject)resList[i];
                        float d = Vector3.Distance(forest.ForestCenter, this.Position);
                        if (d < gatherDistance)
                        {
                            nearResource.Add(resList[i]);
                        }
                    }
                }
                else if (Type == CityType.Oil)
                {
                    if (resList[i].Type == NaturalResourceType.Oil)
                    {
                        float d = Vector3.Distance(resList[i].Position, this.Position);
                        if (d < gatherDistance)
                        {
                            nearResource.Add(resList[i]);
                        }
                    }
                }
            }
            if (Type == CityType.Green)
            {
                nearResource.Sort(CamparisionForest);
            }
            else
            {
                nearResource.Sort(Camparision);
            }
        }

        private int CamparisionForest(NaturalResource a, NaturalResource b)
        {
            float da = Vector3.DistanceSquared(((ForestObject)a).ForestCenter, this.Position);
            float db = Vector3.DistanceSquared(((ForestObject)b).ForestCenter, this.Position);
            return da.CompareTo(db);
        }
        private int Camparision(NaturalResource a, NaturalResource b)
        {
            float da = Vector3.DistanceSquared(a.Position, this.Position);
            float db = Vector3.DistanceSquared(b.Position, this.Position);
            return da.CompareTo(db);
        }


        
        /// <summary>
        ///  从附近的资源中寻找一个数量足够的新资源
        ///  Choose a new resource site to exploit with sufficient amount of resources
        /// </summary>
        private void FindNewNaturalResource()
        {
            for (int i = 0; i < nearResource.Count; i++)
            {
                if (nearResource[i].CurrentAmount > nearResource[i].MaxAmount * 0.1f)
                    resourceIndex = i;
            }
        }

        public void NotifyGotResource(float change)
        {
            //if (Owner != null && Owner.Type == PlayerType.LocalAI)
                //change *= 5;
            this.resourceBuffer += change;
        }

        public override void ProduceBall()
        {
            base.ProduceBall();

            if (Type == CityType.Oil)
            {
                battleField.CreateResourceBall(Owner, this, RBallType.Oil);
            }
            else if (Type == CityType.Green)
            {
                battleField.CreateResourceBall(Owner, this, RBallType.Green);
            }
        }
        public override bool IsFull()
        {
            return NearbyOwnedBallCount >= RulesTable.CityBallLimit;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float required = Type == CityType.Green ? RulesTable.OilBallCost : RulesTable.GreenBallCost;

            if (nearbyBallList.Count >= RulesTable.CityBallLimit && !isGatherOnHold)
            {
                isGatherOnHold = true;
            }

            if (resourceBuffer > required && !isGatherOnHold)
            {
                ProduceBall();
                resourceBuffer -= required;
            }
            harvester.Update(gameTime);

            if (isGatherOnHold) 
            {
                if (nearbyBallList.Count < RulesTable.CityBallLimit)
                {
                    isGatherOnHold = false;
                    Gather(ExResource);
                }
            }
        }

        public override void Parse(GameConfigurationSection sect)
        {
            base.Parse(sect);

            harvester.SetPosition(MathEx.Degree2Radian(Longitude), MathEx.Degree2Radian(Latitude));
        }
    }

    /// <summary>
    ///  Represnts cities generating resource balls overtime
    /// </summary>
    class ProductionCity : City
    {
        /// <summary>
        ///  产生资源球所需时间
        ///  The total amount of time to generate a RBall
        /// </summary>
        float generateRBallTime;
        /// <summary>
        ///  The remaining amount of time to generate a RBall
        /// </summary>
        float generateRBallCD;

        static float ResetGenerateRBallCD(CityType ctype)
        {
            switch (ctype)
            {
                case CityType.Health:
                    return RulesTable.HealthBallGenInterval;
                case CityType.Volience:
                    return RulesTable.VolienceBallGenInterval;
                case CityType.Disease:
                    return RulesTable.DiseaseBallGenInterval;
                case CityType.Education:
                    return RulesTable.EducationBallGenInterval;
            }
            throw new InvalidOperationException();
        }

        public ProductionCity(BattleField btfld, Player owner, CityType type)
            : base(btfld, owner, type)
        {
            generateRBallTime = ResetGenerateRBallCD(Type);
            generateRBallCD = generateRBallTime;
        }
        
        public override float GetProductionProgress()
        {
            return 1 - generateRBallCD / generateRBallTime;
        }

        public override void Parse(GameConfigurationSection sect)
        {
            base.Parse(sect);
        }



        public override bool IsFull()
        {
            return NearbyOwnedBallCount >= RulesTable.CityBallLimit;            
        }
        public override void Update(GameTime dt)
        {
            base.Update(dt);


            float ddt = (float)dt.ElapsedGameTimeSeconds;
            if (Owner != null)
            {
                generateRBallCD -= ddt;

                // generate when ready
                if (generateRBallCD < 0 && !IsFull())
                {
                    generateRBallCD = generateRBallTime;
                    ProduceBall();
                }
            }
        }

        public override void ProduceBall()
        {
            base.ProduceBall();

            switch (Type)
            {
                case CityType.Health:
                    battleField.CreateResourceBall(Owner, this, RBallType.Health);
                    break;
                case CityType.Disease:
                    battleField.CreateResourceBall(Owner, this, RBallType.Disease);
                    break;
                case CityType.Volience:
                    battleField.CreateResourceBall(Owner, this, RBallType.Volience);
                    break;
                case CityType.Education:
                    battleField.CreateResourceBall(Owner, this, RBallType.Education);
                    break;

            }
        }

    }
}
