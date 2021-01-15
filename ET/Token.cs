using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Token
    {
        [Key]
        public string TokenID { get; set; }

        public int UserID { get; set; }

        public DateTime ExpiresDate { get; set; }
    }
}
