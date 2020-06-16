using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class HomePage
    {
        [Key]
        public int HomePageID { get; set; }

        [Required]
        [Display(Name ="Versículo Principal")]
        public string DailyVerse { get; set; }

        [Required]
        [Display(Name = "Referencia")]
        public string DailyVerseReference { get; set; }

        [Required]
        [Display(Name = "Servicios - Subtitulo")]
        public string ServicesTitle { get; set; }

        [Required]
        [Display(Name = "Servicios - Descripción")]
        public string ServicesDescription { get; set; }

        [Required]
        [Display(Name = "Podcast- Subtitulo")]
        public string PodcastTitle { get; set; }

        [Required]
        [Display(Name = "Podcast - Descripción")]
        public string PodcastDescription { get; set; }

        [Required]
        [Display(Name = "Prédicas - Subtitulo")]
        public string SermonsTitle { get; set; }

        [Required]
        [Display(Name = "Prédicas - Descripción")]
        public string SermonsDescription { get; set; }

        public string ActionType { get; set; }
    }
}
