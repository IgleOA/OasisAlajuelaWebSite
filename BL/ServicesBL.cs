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
    }
}
