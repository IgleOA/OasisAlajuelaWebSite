using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ET
{
    public class Banner
    {
        [Key]
        public int BannerID { get; set; }

        [Display(Name ="Imagen")]
        public byte[] BannerPicture { get; set; }

        [Display(Name ="Leyenda")]
        public string BannerName { get; set; }

        [Display(Name = "Ubicación")]
        public string LocationBanner { get; set; }

        [Display(Name ="Status")]
        public bool ActiveFlag { get; set; }

        [Display(Name = "Orden")]
        public int Order { get; set; }

        public Bitmap BannerImage { get; set; }

        public string ActionType { get; set; }
    }
}
