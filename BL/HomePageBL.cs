using ET;
using DAL;
using System.Collections.Generic;

namespace BL
{
    public class HomePageBL
    {
        private HomePageDAL HDL = new HomePageDAL();

        public List<HomePage> HomePage(bool activeflag)
        {
            return HDL.HomePage(activeflag);
        }

        public bool AddHomePage(HomePage hp, string insertuser)
        {
            return HDL.AddHomePage(hp, insertuser);
        }

        public bool UpdateHomePage(HomePage hp, string insertuser)
        {
            return HDL.UpdateHomePage(hp, insertuser);
        }
    }
}
