using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Services
    {
        [Key]
        public int ServiceID { get; set; }

        [Required]
        [Display(Name = "Nombre del Servicio")]
        public string ServiceName { get; set; }

        [Required]
        [Display(Name = "Detalle del Servicio")]
        public string ServiceDescription { get; set; }

        [Required]
        [Display(Name = "Icono del Servicio")]
        public string ServiceIcon { get; set; }

        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }

        [Required]
        [Display(Name = "Orden")]
        public int Order { get; set; }

        public string ActionType { get; set; }
    }
}
