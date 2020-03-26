using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Roles
    {
        [Key]
        [Required]
        public int RoleID { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public string RoleName { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string RoleDescription { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }
    }

}