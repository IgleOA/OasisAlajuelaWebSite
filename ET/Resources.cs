using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ET
{
    public class ResourceTypes
    {
        [Key]
        public int ResourceTypeID { get; set; }

        [Required]
        [Display(Name ="Tipo de Recurso")]
        public string TypeName { get; set; }
        
        public string TypeImagePath { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Imagen de Fondo")]
        public HttpPostedFileBase UploadFile { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Acceso")]
        public bool IsPublic { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }

        public List<Groups> GroupList { get; set; }

        public List<Resources> TopResources { get; set; }

    }

    public class ResourcesGroups
    {
        public int ResourceGroupID { get; set; }

        public int ResourceTypeID { get; set; }

        public int GroupID { get; set; }

        public string TypeName { get; set; }

        public List<Groups> GroupList { get; set; }

    }

    public class Resources
    {
        [Key]
        public int ResourceID { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        public int ResourceTypeID { get; set; }

        public string TypeName { get; set; }

        [Required]
        [Display(Name = "Tipo de archivo")]
        public string FileType { get; set; }

        public string FilePath { get; set; }

        public string FileExt { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo")]
        public HttpPostedFileBase UploadFile { get; set; }

        [Required]
        [Display(Name = "Nombre del Recurso")]
        public string FileName { get; set; }

        [Display(Name = "Link del Video")]
        public string FileURL { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Activo")]
        public bool ActiveFlag { get; set; }

        [Display(Name = "Acceso Limitado")]
        public bool AccessLimited { get; set; }

        [Display(Name = "Inicio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd hh\\:mm tt}")]
        public DateTime? EnableStart { get; set; }

        [Display(Name = "Fin")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd hh\\:mm tt}")]
        public DateTime? EnableEnd { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime ESDate { get; set; }

        [Required]
        [Display(Name = "Hora")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:hh\\:mm tt}")]
        public TimeSpan ESTime { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime EEDate { get; set; }

        [Required]
        [Display(Name = "Hora")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:hh\\:mm tt}")]
        public TimeSpan EETime { get; set; }

        public List<ResourceTypes> TypeList { get; set; }

        public enum FileTypesList
        {
            Video, Documento, Audio
        }
        public ResourceTypes TypeData { get; set; }

        public string ActionType { get; set; }

        public Resources()
        {
            TypeData = new ResourceTypes();
        }        
    }
    public class ResourceRequest
    {
        [Required]
        public int ResourceTypeID { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
    }
}
