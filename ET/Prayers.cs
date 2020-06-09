using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Prayers
    {
        [Key]
        public int PrayerID { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nombre")]
        public string Requester { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Por favor ingrese una email válido.")]
        public string Email { get; set; }

        [Display(Name = "Celular")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Motivo de Oración")]
        public string Reason { get; set; }
        
        [Display(Name = "Fecha de Solicitud")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime InsertDate { get; set; }

        public string IP { get; set; }

        [Display(Name = "Pais")]
        public string Country { get; set; }

        [Display(Name = "Región")]
        public string Region { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        public string ActionType { get; set; }

    }
}
