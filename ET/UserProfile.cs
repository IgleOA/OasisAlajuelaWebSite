using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ET
{
    public class UserProfile
    {
        [Key]
        public int UserID { get; set; }

        [Display(Name ="Rol")]
        public int RoleID { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; }

        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Display(Name = "Correo Electrónico")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }

        [Display(Name = "Ultima actividad")]
        [DataType(DataType.DateTime)]
        public DateTime LastActivityDate { get; set; }

        [Display(Name = "Fecha de Incorporación")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Rol")]
        public string RoleName { get; set; }

        [Display(Name = "Imagen")]
        public string Photo { get; set; }

        [DataType(DataType.Upload)]
        [AllowExtensions(Extensions = "png,jpg,jpeg,gif", ErrorMessage = "Por favor seleccione solo archivos soportados .png, .jpg, .jpeg, .gif")]
        [Display(Name = "Archivo")]
        public HttpPostedFileBase file { get; set; }

        public string PhotoExt { get; set; }

        [Display(Name = "Teléfono")]
        [RegularExpression(@"^(?<countryCode>[\+][1-9]{1}[0-9]{0,2}\s)?(?<areaCode>0?[1-9]\d{0,4})(?<number>\s[1-9][\d]{5,12})(?<extension>\sx\d{0,4})?$", ErrorMessage = "Ingrese un número de teléfono válido")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Celular")]
        [RegularExpression(@"^(?<countryCode>[\+][1-9]{1}[0-9]{0,2}\s)?(?<areaCode>0?[1-9]\d{0,4})(?<number>\s[1-9][\d]{5,12})(?<extension>\sx\d{0,4})?$", ErrorMessage = "Ingrese un número de teléfono válido")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        [Display(Name = "Facebook")]
        [RegularExpression(@"(?:(?:http|https):\/\/)?(?:www.)?facebook.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-]*\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?", ErrorMessage = "Ingrese una cuenta de Facebook válida")]
        [DataType(DataType.Url)]
        public string Facebook { get; set; }

        [Display(Name = "Twitter")]
        [RegularExpression(@"(?:(?:http|https):\/\/)?(?:www.)?twitter.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-]*\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?", ErrorMessage = "Ingrese una cuenta de Facebook válida")]
        [DataType(DataType.Url)]
        public string Twitter { get; set; }

        [Display(Name = "Snapchat")]
        [RegularExpression(@"(?:(?:http|https):\/\/)?(?:www.)?snapchat.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-]*\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?", ErrorMessage = "Ingrese una cuenta de Facebook válida")]
        [DataType(DataType.Url)]
        public string Snapchat { get; set; }

        [Display(Name = "Instragram")]
        [RegularExpression(@"(?:(?:http|https):\/\/)?(?:www.)?instragram.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-]*\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?", ErrorMessage = "Ingrese una cuenta de Facebook válida")]
        [DataType(DataType.Url)]
        public string Instragram { get; set; }

        [Display(Name = "Pais")]
        public string Country { get; set; }

        [Display(Name = "Estado o Provincia")]
        public string State { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        public List<Groups> GroupList { get; set; }

        public List<Reservations> ReservationsList { get; set; }

        public string ActionType { get; set; }
        public class AllowExtensionsAttribute : ValidationAttribute
        {
            public string Extensions { get; set; } = "png,jpg,jpeg,gif";

            public override bool IsValid(object value)
            {
                // Initialization  
                HttpPostedFileBase file = value as HttpPostedFileBase;
                bool isValid = true;

                // Settings.  
                List<string> allowedExtensions = this.Extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                // Verification.  
                if (file != null)
                {
                    // Initialization.  
                    var fileName = file.FileName;

                    // Settings.  
                    isValid = allowedExtensions.Any(y => fileName.EndsWith(y));
                }

                // Info  
                return isValid;
            }
        }
    }
}
