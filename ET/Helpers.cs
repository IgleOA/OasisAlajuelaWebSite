using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ET
{
    public class AzureStorage
    {
        public string FileType { get; set; }

        public HttpPostedFileBase File { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

    }
}
