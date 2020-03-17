using ET;
using DAL;

namespace BL
{
    public class HomeBL
    {
        private HomeDAL HDL = new HomeDAL();

        public HomePage Home ()
        {
            return HDL.Home();
        }
    }
}
