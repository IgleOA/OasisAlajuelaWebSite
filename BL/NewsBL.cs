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

        public bool AddNew(News ms, string insertuser)
        {
            return NDAL.AddNew(ms, insertuser);
        }

        public bool Update(News ms, string insertuser)
        {
            return NDAL.Update(ms, insertuser);
        }

        public News Details(int NewID)
        {
            return NDAL.Details(NewID);
        }
    }
}
