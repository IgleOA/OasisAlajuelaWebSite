using System.Collections.Generic;
using ET;
using DAL;
using System;

namespace BL
{
    public class UpcommingEventsBL
    {
        private UpcommingEventsDAL UDAL = new UpcommingEventsDAL();

        public List<UpcommingEvents> List(DateTime startdate, bool upcommingflag, bool? activeflag)
        {
            return UDAL.List(startdate, upcommingflag, activeflag);
        }

        public bool AddNew(UpcommingEvents Event, string insertuser)
        {
            return UDAL.AddNew(Event, insertuser);
        }

        public UpcommingEvents Details(int eventid)
        {
            return UDAL.Details(eventid);
        }

        public bool Update(UpcommingEvents Event, string insertuser)
        {
            return UDAL.Update(Event, insertuser);
        }
    }
}
