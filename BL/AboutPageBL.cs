using ET;
using DAL;

namespace BL
{
    public class AboutPageBL
    {
        private AboutPageDAL ADAL = new AboutPageDAL();

        public AboutPage About()
        {
            return ADAL.About();
        }
    }
}
