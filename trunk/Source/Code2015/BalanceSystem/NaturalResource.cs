﻿using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D;
using Apoc3D.Collections;
using Apoc3D.Config;

namespace Code2015.BalanceSystem
{
    /// <summary>
    ///  表示自然资源的类型
    /// </summary>
    public enum NaturalResourceType
    {
        None,
        Wood,
        Food,
        Petro
    }


    public abstract class NaturalResource : SimulationObject
    {
       
        protected NaturalResource(SimulationWorld region, NaturalResourceType type)
            : base(region)
        {
            Type = type;
        }
        
        /// <summary>
        ///  获取自然资源的类型
        /// </summary>
        public NaturalResourceType Type
        {
            get;
            private set;
        }

        public bool IsLow
        {
            get { return CurrentAmount < 1000; }
        }


        /// <summary>
        ///  获取当前自然资源
        /// </summary>
        public float CurrentAmount
        {
            get;
            protected set;
        }

        public object OutputTarget
        {
            get;
            set;
        }

        /// <summary>
        ///  开采一定数量的自然资源
        /// </summary>
        /// <param name="amount">申请值</param>
        /// <returns>实际得到的</returns>
        public float Exploit(float amount)
        {
            if (amount < CurrentAmount)
            {
                CurrentAmount -= amount;
                return amount;
            }


            float r = CurrentAmount;
            CurrentAmount = 0;
            return r;

        }


       

        public override void Update(GameTime time)
        {
            
        }

        public override void Parse(ConfigurationSection sect)
        {
            base.Parse(sect);

            CurrentAmount = sect.GetSingle("Amount");
        }

    }
}
