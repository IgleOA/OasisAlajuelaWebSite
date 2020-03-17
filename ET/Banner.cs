using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ET
{
    public class Banner
    {
        [Key]
        public int BannerID { get; set; }

        [Display(Name ="Imagen")]
        [Required]
        public byte[] BannerData { get; set; }

        [Required]
        public string  BannerExt { get; set; }

        [Required]
        [Display(Name ="Leyenda")]
        public string BannerName { get; set; }

        [Required]
        [Display(Name = "Ubicación")]
        public string LocationBanner { get; set; }

        [Display(Name ="Status")]
        public bool ActiveFlag { get; set; }

        [Required]
        [Display(Name = "Orden")]
        public int Order { get; set; }

        public int Slide { get; set; }
        
        public string ActionType { get; set; }
    }
}
/*
 * BANNER FOR HOME PAGE 800x400px
 */