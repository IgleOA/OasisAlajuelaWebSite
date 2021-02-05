using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Reservations
    {
        public int ReservationID { get; set; }

        public string GUID { get; set; }
                
        public int EventID { get; set; }

        public string Title { get; set; }

        public DateTime ScheduledDate { get; set; }

        public int BookedBy { get; set; }

        public string BookedByName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IdentityID { get; set; }

        public DateTime ReservationDate { get; set; }

        public string ActionType { get; set; }

    }
    
    public class ReservationRequest
    {
        public string GUID { get; set; }

        public int EventID { get; set; }

        public int BookedBy { get; set; }

        public string JSONBookedFor { get; set; }
    }

    public class ReservationResult
    {

        public string GUID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IdentityID { get; set; }

        public string Status { get; set; }
    }

    public class ReservationListRequest
    {
        public string GUID { get; set; }

        public int? EventID { get; set; }
    }

    public class Reserver
    {
        public string IdentityID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
