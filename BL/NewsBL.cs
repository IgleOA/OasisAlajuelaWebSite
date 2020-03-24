using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class NewsBL
    {
        private NewsDAL NDAL = new NewsDAL();

        public List<News> List(bool activeflag)
        {
            return NDAL.List(activeflag);
        }
    }
}
