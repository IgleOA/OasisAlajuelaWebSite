using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Ministries
    {
        [Key]
        public int MinistryID { get; set; }

        [Required]
        [Display(Name ="Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public string ActionLink { get; set; }

        [Display(Name ="Status")]
        public bool ActiveFlag { get; set; }

    }
}
