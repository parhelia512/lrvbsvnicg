﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D;
using Apoc3D.Collections;

namespace Code2015.BalanceSystem
{
    public class Simulateobject:IUpdatable
    {     
        /// <summary>
        /// CO2的改变量
        /// </summary>
        public float CarbonWeight=0.0f;
        public float CarbonSpeed
        {
            get;
            set;
        }

        NaturalResource naturalresource;
        City city; 
        public Simulateobject()
        {
            //naturalresource= new NaturalResource();
            //city=new City();
        }
        public virtual float GetCarbonWeight()
        {
            return this.CarbonWeight;
        }

        public virtual void Update(GameTime time)
        {
            naturalresource.Update(time);
            city.Update(time);
            this.CarbonWeight += (naturalresource.CarbonWeight+city.CarbonWeight);
        }
    }
}
