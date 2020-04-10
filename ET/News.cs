﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ET
{
    public class News
    {
        [Key]
        public int NewID { get; set; }

        [Required]
        [Display(Name ="Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Banner")]        
        public byte[] BannerData { get; set; }

        [Required(ErrorMessage = "Por favor seleccione un archivo")]
        [DataType(DataType.Upload)]
        [AllowExtensions(Extensions = "png,jpg,jpeg,gif", ErrorMessage = "Por favor seleccione solo archivos soportados .png, .jpg, .jpeg, .gif")]
        [Display(Name = "Banner")]
        public HttpPostedFileBase file { get; set; }

        [Required]
        public string BannerExt { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime InsertDate { get; set; }

        [Display(Name = "Visibilidad")]
        public bool ShowFlag { get; set; }

        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }

        public string NewYear { get; set; }

        public string NewMonth { get; set; }

        public string NewDay { get; set; }

        public string ActionType { get; set; }

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
