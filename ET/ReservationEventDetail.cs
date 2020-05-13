using System;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class ReservationEventDetail
    {
        public int EventID { get; set; }

        public DateTime ScheduledDate { get; set; }

        public int Capacity { get; set; }

        public int Available { get; set; }

        public int Unavailable { get; set; }

        public int Booked { get; set; }

        public string SeatsReserved { get; set; }

        [Required]
        [Range(1,10, ErrorMessage ="Tienes que elegir al menos uno y como máximo 10 espaciones simultaneamente. Si necesita más de 10 espaciones, por favor realice las reservaciones en tractos de 10 espacios a la vez.")]
        public int SeatsNbrReserved { get; set; }

        public int MaxToReserve { get; set; }

        public string ReservedFor { get; set; }

        public AuditoriumLayout Layout { get; set; }

        public ReservationEventDetail()
        {
            Layout = new AuditoriumLayout();
        }
    }    
}
