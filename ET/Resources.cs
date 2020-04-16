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
        
        public byte[] TypeImage { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Imagen de Fondo")]
        public HttpPostedFileBase file { get; set; }

        public string TypeImageExt { get; set; }

        public string Description { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }

    public class Resources
    {
        [Key]
        public int ResourceID { get; set; }

        [Required]
        [Display(Name ="Tipo")]
        public int ResourceTypeID { get; set; }

        public string TypeName { get; set; }

        [Required]
        [Display(Name = "Tipo de archivo")]
        public string  FileType { get; set; }

        public byte[] FileData { get; set; }
                
        [DataType(DataType.Upload)]
        [Display(Name = "Archivo")]
        public HttpPostedFileBase file { get; set; }

        public string FileExt { get; set; }

        [Required]
        [Display(Name = "Nombre del Recurso")]
        public string FileName { get; set; }

        [Display(Name = "URL del Recurso")]
        public string FileURL { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }

        public List<ResourceTypes> TypeList { get; set; }

        public enum FileTypesList
        {
            Video,Documento,Audio,URL
        }
        public ResourceTypes TypeData { get; set; }

        public string ActionType { get; set; }

        public Resources()
        {
            TypeData = new ResourceTypes();
        }
    }
}
