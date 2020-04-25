using ET;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OasisAlajuelaWebSite.Models
{
    public class MultiSelectDropDownViewModel
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public List<int> SelectedMultiGroupId { get; set; }

        public List<Groups> SelectedGroupLst { get; set; }

        public string ActionType { get; set; }
    }

    public class MultiSelectNewRTG
    {
        [Required]
        public int ResourceTypeID { get; set; }

        [Required]
        public List<int> SelectedMultiGroupId { get; set; }

        public List<Groups> SelectedGroupLst { get; set; }

        public string ActionType { get; set; }
    }

    public class MultiSelectNewUG
    {
        [Required]
        public int GroupID { get; set; }

        [Required]
        public List<int> SelectedMultiId { get; set; }

        public List<Users> SelectedLst { get; set; }

        public string ActionType { get; set; }
    }

    public class MultiSelectNewRG
    {
        [Required]
        public int GroupID { get; set; }

        [Required]
        public List<int> SelectedMultiId { get; set; }

        public List<ResourceTypes> SelectedLst { get; set; }

        public string ActionType { get; set; }
    }
}