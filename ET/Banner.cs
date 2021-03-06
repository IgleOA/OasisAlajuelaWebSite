﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ET
{
    public class Banner
    {
        [Key]
        public int BannerID { get; set; }

        [Display(Name ="Imagen")]
        [Required]
        public string BannerPath { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el archivo")]
        [DataType(DataType.Upload)]
        [AllowExtensions(Extensions = "png,jpg,jpeg,gif", ErrorMessage = "Por favor seleccione solo archivos soportados .png, .jpg, .jpeg, .gif")]
        [Display(Name = "Archivo")]
        public HttpPostedFileBase UploadFile { get; set; }

        [Required]
        public string  BannerExt { get; set; }

        [Required]
        [Display(Name ="Leyenda")]
        public string BannerName { get; set; }

        [Required]
        [Display(Name = "Ubicación")]
        public int LocationID { get; set; }

        public string LocationBanner { get; set; }

        [Display(Name ="Status")]
        public bool ActiveFlag { get; set; }

        public int Slide { get; set; }
        
        public string ActionType { get; set; }
         
        public List<BannersLocation> LList { get; set; }

        public BannersLocation BLData { get; set; }


        public Banner()
        {
            BLData = new BannersLocation();
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
/*
 * BANNER FOR HOME PAGE 800x400px
 */