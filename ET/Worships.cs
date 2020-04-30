using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Worships
    {
        public int WorshipID { get; set; }

        public DateTime ScheduledDate { get; set; }

        public int Capacity { get; set; }

        public int Available { get; set; }

        public int Unavailable { get; set; }

        public int Booked { get; set; }

        public string SeatsReserved { get; set; }

        public string ReservedFor { get; set; }

        public AuditoriumLayout Layout { get; set; }

        public Worships()
        {
            Layout = new AuditoriumLayout();
        }
    }    
}
