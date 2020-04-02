﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Users
    {
        public string ActionType { get; set; }

        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo invalido. Por favor ingrese un correo electrónico valido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{6,20}$", ErrorMessage = "Contraseña invalida, debe tener entre 6 - 20 caracteres y contener al menos un número y una mayúscula")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y su confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Rol")]
        public int RoleID { get; set; }

        [Display(Name = "Rol")]
        public string RoleName { get; set; }

        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }

        [Display(Name = "Ultima actividad")]
        public Nullable<DateTime> LastActivityDate { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime CreationDate { get; set; }

        public List<Roles> RolesList { get; set; }

        public Roles RolesData { get; set; }

        public Users()
        {
            RolesData = new Roles();
        }
    }

    public class Login
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }
    }
    public class AuthorizationCode
    {
        public string GUID { get; set; }

        public int UserID { get; set; }

        public string FullName { get; set; }
    }

    public class ResetPasswordModel
    {

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{6,20}$", ErrorMessage = "Contraseña invalida, debe tener entre 6 - 20 caracteres y contener al menos un número y una mayúscula")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y su confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        public string GUID { get; set; }
    }
}