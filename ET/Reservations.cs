using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Reservations
    {
        public int ReservationID { get; set; }

        public string GUID { get; set; }
                
        public int WorshipID { get; set; }

        public int BookedBy { get; set; }

        public string BookedByName { get; set; }

        public string BookedFor { get; set; }

        public string SeatID { get; set; }

        public string SeatsReserved { get; set; }

        public List<ReserveDetail> Details { get; set; }
    }

    public class ReserveDetail
    {
        public string SeatID { get; set; }

        public bool IsValid { get; set; }
    }
        
}
