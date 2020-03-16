using System.Collections.Generic;
using ET;
using DAL;
using System;

namespace BL
{
    public class UpcommingEventsBL
    {
        private UpcommingEventsDAL UDAL = new UpcommingEventsDAL();

        public List<UpcommingEvents> List(DateTime startdate)
        {
            return UDAL.List(startdate);
        }
    }
}
