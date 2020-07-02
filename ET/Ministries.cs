using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace ET
{
    public class Ministries
    {
        [Key]
        public int MinistryID { get; set; }

        [Required]
        [Display(Name ="Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Imagen de Fondo")]
        public HttpPostedFileBase UploadFile { get; set; }

        public string ImageExt { get; set; }

        [Display(Name = "Enlace de Página (si aplica)")]
        public string ActionLink { get; set; }

        [Display(Name ="Status")]
        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
