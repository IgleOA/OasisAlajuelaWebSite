using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class RightsBL
    {
        private RightsDAL RDAL = new RightsDAL();

        public List<Rights> List(int roleid)
        {
            return RDAL.List(roleid);
        }

        public bool Update(Rights detail, string insertuser)
        {
            return RDAL.Update(detail, insertuser);
        }

        public AccessRights ValidationRights(string username, string controller, string action)
        {
            return RDAL.ValidationRights(username, controller, action);
        }
    }
}
