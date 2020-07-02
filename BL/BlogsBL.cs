using DAL;
using ET;
using System.Collections.Generic;

namespace BL
{
    public class BlogsBL
    {
        private BlogsDAL PDAL = new BlogsDAL();

        public List<Blogs> List()
        {
            return PDAL.List();
        }

        public List<Blogs> History()
        {
            return PDAL.History();
        }

        public bool AddNew(Blogs ms, string insertuser)
        {
            return PDAL.AddNew(ms, insertuser);
        }

        public bool Update(Blogs ms, string insertuser)
        {
            return PDAL.Update(ms, insertuser);
        }

        public Blogs Details(int NewID)
        {
            return PDAL.Details(NewID);
        }
    }
}
