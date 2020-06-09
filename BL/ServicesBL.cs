using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class ServicesBL
    {
        private ServicesDAL SDAL = new ServicesDAL();

        public List<Services> List(bool activeflag)
        {
            return SDAL.List(activeflag);
        }

        public bool AddNew(Services service, string insertuser)
        {
            return SDAL.AddNew(service, insertuser);
        }

        public bool Update(Services service, string insertuser)
        {
            return SDAL.Update(service, insertuser);
        }

        public Services Details(int ServiceID)
        {
            return SDAL.Details(ServiceID);
        }
    }
}
