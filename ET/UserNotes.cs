using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ET
{
    public class UserNotes
    {
        [Key]
        public int NoteID { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public int UserID { get; set; }

        [Required]
        [Display(Name = "Mensaje")]
        public string RequestNote { get; set; }

        [Display(Name ="Fecha")]
        public DateTime NoteDate { get; set; }

        [Display(Name ="Remitente")]
        public string InsertedBy { get; set; }

        [Required]
        [Display(Name ="Requiere Respuesta?")]
        public bool ResponseRequired { get; set; }

        [Display(Name ="Respuesta")]
        public string ResponseNote { get; set; }

        public DateTime ResponseDate { get; set; }

        [Display(Name ="Status")]
        public bool ReadFlag { get; set; }

        public List<Users> UserList { get; set; }

        public string ActionType { get; set; }
    }

    public class ResponseUserNote
    {
        [Key]
        public int NoteID { get; set; }

        [Required]
        [Display(Name ="Respuesta")]
        public string ResponseNote { get; set; }

        public string ActionType { get; set; }

    }

    public class GroupNote
    {
        [Required]
        [Display(Name = "Equipo de Trabajo")]
        public int GroupID { get; set; }

        [Required]
        [Display(Name = "Mensaje")]
        public string RequestNote { get; set; }

        [Required]
        [Display(Name = "Requiere Respuesta?")]
        public bool ResponseRequired { get; set; }

        public List<Groups> GroupList { get; set; }

        public string ActionType { get; set; }
    }

    public class AllNote
    {
        [Required]
        [Display(Name = "Mensaje")]
        public string RequestNote { get; set; }

        [Required]
        [Display(Name = "Requiere Respuesta?")]
        public bool ResponseRequired { get; set; }

        public List<Users> UsersList { get; set; }

        public string ActionType { get; set; }
    }
}
