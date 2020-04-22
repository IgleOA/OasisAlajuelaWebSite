using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ET
{
    public class AboutPage
    {
        [Required]
        [Display(Name = "Historia")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string History { get; set; }

        [Required]
        [Display(Name = "Misión")]
        [UIHint("tinymce_jquery_full"),AllowHtml]
        public string Mision { get; set; }

        [Required]
        [Display(Name = "Visión")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Vision { get; set; }

        [Required]
        [Display(Name = "Pastores")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Pastors { get; set; }

        public string ActionType { get; set; }
    }
}
