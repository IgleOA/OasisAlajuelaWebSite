using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class BannersLocationBL
    {
        private BannersLocationDAL BDAL = new BannersLocationDAL();
        
        public List<BannersLocation> List()
        {
            return BDAL.List();
        }

    }
}
