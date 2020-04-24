using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class GroupsBL
    {
        private GroupsDAL GDAL = new GroupsDAL();
        public List<Groups> List()
        {
            return GDAL.List();
        }
        public List<Groups> ListbyRT(int ResourceTypeID)
        {
            return GDAL.ListbyRT(ResourceTypeID);
        }
        public List<Groups> ListbyUser(int UserID)
        {
            return GDAL.ListbyUser(UserID);
        }
    }
}
