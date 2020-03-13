using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class HomeBL
    {
        private HomeDAL HDAL = new HomeDAL();

        public List<Banner> Banners (string location)
        {
            return HDAL.Banners(location);
        }
    }
}
