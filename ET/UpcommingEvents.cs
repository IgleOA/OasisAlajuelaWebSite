using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class UpcommingEvents
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [Display(Name ="Titulo")]
        public string Title { get; set; }

        [Required]
        [Display(Name ="Ministro")]
        public int MinisterID { get; set; }

        [Display(Name = "Ministro")]
        public string MinisterName { get; set; }

        [Required]
        [Display(Name ="Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime ScheduledDate { get; set; }

        [Display(Name ="Status")]
        public bool ActiveFlag { get; set; }

        public int Order { get; set; }

        public List<Ministers> MinisterList { get; set; }

        public Ministers MinisterData { get; set; }

        public string ActionType { get; set; }

        public UpcommingEvents ()
        {
            MinisterData = new Ministers();
        }
    }
}
