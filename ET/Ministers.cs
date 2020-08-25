using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Ministers
    {
        [Key]
        public int MinisterID { get; set; }

        [Required]
        [Display(Name ="Titulo")]
        public string Title { get; set; }

        [Required]
        [Display(Name ="Nombre")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Foto")]
        public string Photo { get; set; }

        [Display(Name ="Status")]
        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
