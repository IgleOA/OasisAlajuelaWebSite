using ET;
using DAL;
using System;

namespace BL
{
    public class HomeBL
    {
        private HomeDAL HDL = new HomeDAL();

        public HomePage Home ()
        {
            return HDL.Home();
        }

        public bool AddHomePage(HomePage hp, string insertuser)
        {
            return HDL.AddHomePage(hp, insertuser);
        }

        public InitalHomePage HomePage(DateTime id)
        {
            return HDL.HomePage(id);
        }
    }
}
