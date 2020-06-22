using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Groups
    {
        [Key]
        public int GroupID { get; set; }

        [Required]
        [Display(Name ="Area")]
        public string GroupName { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }

        public List<UsersGroups> UserList { get; set; }

        public List<ResourcesGroups> RTypesList { get; set; }
    }
    
}
