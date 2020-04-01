using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Rights
    {
        public int WebID { get; set; }

        public int RoleID { get; set; }

        [Display(Name = "Controlador")]
        public string DisplayName { get; set; }
                
        public int RightID { get; set; }

        [Display(Name = "Lectura")]
        public bool ReadRight { get; set; }

        [Display(Name = "Escritura")]
        public bool WriteRight { get; set; }

        public string ActionType { get; set; }

        public string ChangeType { get; set; }

    }

    public class AccessRights
    {
        [Display(Name = "Lectura")]
        public bool ReadRight { get; set; }

        [Display(Name = "Escritura")]
        public bool WriteRight { get; set; }
    }
}
