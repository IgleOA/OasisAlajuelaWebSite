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

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name ="Fecha")]
        [DataType(DataType.Date)]
        [CheckDate(ErrorMessage ="Por ingrese una fecha superior a la actual.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [Display(Name = "Hora")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:hh\\:mm tt}")]
        public TimeSpan ScheduledTime { get; set; }

        [Display(Name ="Status")]
        public bool ActiveFlag { get; set; }
        
        public List<Ministers> MinisterList { get; set; }

        public Ministers MinisterData { get; set; }

        public string ActionType { get; set; }

        public string EventMonth { get; set; }

        public string EventDay { get; set; }

        public UpcommingEvents ()
        {
            MinisterData = new Ministers();
        }

        public class CheckDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                bool isValid = true;
                DateTime ScheduledDate = (DateTime)value;

                if (ScheduledDate < DateTime.Now)
                {
                    isValid = false;
                }

                return isValid;
            }
        }
    }
}
