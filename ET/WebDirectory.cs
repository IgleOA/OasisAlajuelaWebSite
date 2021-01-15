using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class WebDirectory
    {
        [Key]
        public int WebID { get; set; }

        public int AppID { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public bool PublicMenu { get; set; }

        public bool AdminMenu { get; set; }

        public string DisplayName { get; set; }

        public string Parameter { get; set; }
        
        public int Order { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }

    public class WebDirectoryRequest
    {
        public int AppID { get; set; }
        public int UserID { get; set; }
    }
}
