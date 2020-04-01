
using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class WebDirectoryBL
    {
        private WebDirectoryDAL WDAL = new WebDirectoryDAL();

        public List<WebDirectory> List()
        {
            return WDAL.List();
        }

        public bool AddNew(WebDirectory detail, string insertuser)
        {
            return WDAL.AddNew(detail, insertuser);
        }

        public List<WebDirectory> WDByUser(string username)
        {
            return WDAL.WDByUser(username);
        }
    }
}
