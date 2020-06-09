using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Enrollments
    {
        [Key]
        public int EnrollmentID { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name ="Grupo")]
        public int GroupID { get; set; }

        [Display(Name = "Grupo")]
        public string GroupName { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Apertura")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime OpenRegister { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Cierre")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CloseRegister { get; set; }

        public bool ApprovalFlag { get; set; }

        public List<Groups> GroupList { get; set; }

        public string ActionType { get; set; }

        public List<EnrolledUsers> UserList { get; set; }
    }

    public class EnrolledUsers
    {
        [Key]
        public int RegisterID { get; set; }

        [Required(ErrorMessage = "El Usuario es un campo obligatorio")]
        [Display(Name ="Usuario")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "El Usuario es un campo obligatorio")]
        [Display(Name = "Usuario")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "El Teléfono es un campo obligatorio")]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "El Curso es un campo obligatorio")]
        [Display(Name = "Curso")]
        public int EnrollmentID { get; set; }

        [Display(Name ="Autorizado")]
        public bool ApprovalFlag { get; set; }

        public List<Enrollments> Courses { get; set; }

        public string ActionType { get; set; }

    }

    public class AdminEnroller
    {
        [Required(ErrorMessage = "El Usuario es un campo obligatorio")]
        [Display(Name = "Usuario")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "El Curso es un campo obligatorio")]
        [Display(Name = "Curso")]
        public int EnrollmentID { get; set; }
               
        public List<Enrollments> Courses { get; set; }

        public List<Users> UserList { get; set; }

        public string ActionType { get; set; }
    }

    public class EnrollerInfo
    {
        public string GroupName { get; set; }

        public string FullName { get; set; }

    }
}
