using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ET
{
    public class Sermons
    {
        [Key]
        public int SermonID { get; set; }

        [Required]
        [Display(Name = "Titulo")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Ministro")]
        public int MinisterID { get; set; }

        [Display(Name = "Ministro")]
        public string MinisterName { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Descripción")]
        public string Tags { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime SermonDate { get; set; }

        [Required]
        [Display(Name = "Link")]
        [DataType(DataType.Url)]
        public string SermonURL { get; set; }

        [Display(Name = "Imagen de Fondo")]
        [Required]
        public byte[] BannerData { get; set; }

        [Required]
        public string BannerExt { get; set; }

        [Required(ErrorMessage = "Por favor seleccione un archivo")]
        [DataType(DataType.Upload)]
        [AllowExtensions(Extensions = "png,jpg,jpeg,gif", ErrorMessage = "Por favor seleccione solo archivos soportados .png, .jpg, .jpeg, .gif")]
        [Display(Name = "Banner")]
        public HttpPostedFileBase file { get; set; }

        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }
        
        public List<Ministers> MinisterList { get; set; }

        public Ministers MinisterData { get; set; }

        public string ActionType { get; set; }

        public Sermons()
        {
            MinisterData = new Ministers();
        }

        public class AllowExtensionsAttribute : ValidationAttribute
        {
            public string Extensions { get; set; } = "png,jpg,jpeg,gif";

            public override bool IsValid(object value)
            {
                // Initialization  
                HttpPostedFileBase file = value as HttpPostedFileBase;
                bool isValid = true;

                // Settings.  
                List<string> allowedExtensions = this.Extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                // Verification.  
                if (file != null)
                {
                    // Initialization.  
                    var fileName = file.FileName;

                    // Settings.  
                    isValid = allowedExtensions.Any(y => fileName.EndsWith(y));
                }

                // Info  
                return isValid;
            }
        }
    }
}
