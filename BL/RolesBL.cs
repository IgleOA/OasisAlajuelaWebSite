using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class RolesBL
    {
        private RolesDAL RDAL = new RolesDAL();

        public List<Roles> List()
        {
            return RDAL.List();
        }

        public bool AddNew(Roles detail, string insertuser)
        {
            return RDAL.AddNew(detail, insertuser);
        }
    }
}
