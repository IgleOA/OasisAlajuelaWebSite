using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class HomePage
    {
        [Key]
        public int SectionID { get; set; }

        [Required]
        [Display(Name ="Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "RouterLink")]
        public string RouterLink { get; set; }

        [Display(Name = "Imagen")]
        public string Image { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }

    //public class InitalHomePage
    //{
    //    public string DailyVerse { get; set; }
    //    public string DailyVerseReference { get; set; }
    //    public string ServicesTitle { get; set; }
    //    public string ServicesDescription { get; set; }
    //    public string PodcastTitle { get; set; }
    //    public string PodcastDescription { get; set; }
    //    public string SermonsTitle { get; set; }
    //    public string SermonsDescription { get; set; }
    //    public UpcommingEvents NextEvent { get; set; }
    //    public List<Blogs> HPBlogs { get; set; }
    //    public List<Services> HPServices { get; set; }
    //    public List<Banner> HPBanners { get; set; }
    //    public List<Sermons> HPSermons { get; set; }
    //}
}
