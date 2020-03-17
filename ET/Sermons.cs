using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Sermons
    {
        [Key]
        public int SermonID { get; set; }

        [Required]
        [Display(Name = "Titulo")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Ministro")]
        public int MinisterID { get; set; }

        [Display(Name = "Ministro")]
        public string MinisterName { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Descripción")]
        public string Tags { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime SermonDate { get; set; }

        [Required]
        [Display(Name = "Link")]
        [DataType(DataType.Url)]
        public string SermonURL { get; set; }

        [Display(Name = "Imagen de Fondo")]
        [Required]
        public byte[] BackgroundImage { get; set; }

        [Required]
        public string BackgroundExt { get; set; }

        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }
        
        public List<Ministers> MinisterList { get; set; }

        public Ministers MinisterData { get; set; }

        public string ActionType { get; set; }

        public Sermons()
        {
            MinisterData = new Ministers();
        }
    }
}
