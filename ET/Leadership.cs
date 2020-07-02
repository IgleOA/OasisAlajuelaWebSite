using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace ET
{
    public class Leadership
    {
        [Key]
        public int LeaderID { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Foto")]
        [Required]
        public HttpPostedFileBase UploadFile { get; set; }

        public string ImageExt { get; set; }

        [Display(Name = "Enlace de Página")]
        public string ActionLink { get; set; }

        [Display(Name = "Orden")]
        [Required]
        public int Order { get; set; }

        public string ActionType { get; set; }
    }
}
