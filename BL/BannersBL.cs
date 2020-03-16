using System.Collections.Generic;
using ET;
using DAL;


namespace BL
{
    public class BannersBL
    {
        private BannnersDAL BDAL = new BannnersDAL();

        public List<Banner> Banners(string location, bool activeflag)
        {
            return BDAL.Banners(location, activeflag);
        }
    }
}
