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

        [Display(Name = "Reservado Por")]
        public string BookedByName { get; set; }

        [Display(Name = "Reservado Para")]
        public string BookedFor { get; set; }

        [Display(Name = "ID Asiento")]
        public string SeatID { get; set; }

        public string SeatsReserved { get; set; }

        public bool ActiveFlag { get; set; }

        public DateTime ReservationDate { get; set; }

        public List<ReserveDetail> Details { get; set; }
    }

    public class ReserveDetail
    {
        public string SeatID { get; set; }

        public bool IsValid { get; set; }
    }

    public class ReservationLevel1
    {
        [Display(Name = "Código de Reservación")]
        public string GUID { get; set; }

        public int EventID { get; set; }

        [Display(Name = "Evento")]
        public string Title { get; set; }

        [Display(Name = "Fecha de Evento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ScheduledDate { get; set; }

        [Display(Name = "Condición")]
        public bool ActiveFlag { get; set; }

        [Display(Name = "Fecha de Reservación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReservationDate { get; set; }

        [Display(Name = "Reservado Por")]
        public string BookedByName { get; set; }

        [Display(Name = "Reservado Para")]
        public string BookedFor { get; set; }

        public List<Reservations> Details { get; set; }

    }
        
}
