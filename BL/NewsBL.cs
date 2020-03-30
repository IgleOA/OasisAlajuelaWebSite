using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class NewsBL
    {
        private NewsDAL NDAL = new NewsDAL();

        public List<News> List()
        {
            return NDAL.List();
        }

        public bool AddNew(News ms, string insertuser)
        {
            return NDAL.AddNew(ms, insertuser);
        }
    }
}
