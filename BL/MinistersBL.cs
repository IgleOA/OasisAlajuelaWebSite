﻿using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class MinistersBL
    {
        private MinistersDAL MBL = new MinistersDAL();

        public List<Ministers> List(bool activeflag)
        {
            return MBL.List(activeflag);
        }

        public bool AddNew(Ministers Detail, string InsertUser)
        {
            return MBL.AddNew(Detail, InsertUser);
        }

        public Ministers Details(int MinisterID)
        {
            return MBL.Details(MinisterID);
        }

        public bool Update(Ministers Detail, string InsertUser)
        {
            return MBL.Update(Detail, InsertUser);
        }
    }
}
