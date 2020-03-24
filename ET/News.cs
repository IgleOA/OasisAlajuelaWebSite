using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }
    }
}
