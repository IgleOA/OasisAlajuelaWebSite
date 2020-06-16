using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using DAL;

namespace BL
{
    public class PodcastsBL
    {
        private PodcastsDAL PDAL = new PodcastsDAL();

        public List<Podcasts> List()
        {
            return PDAL.List();
        }

        public bool AddNew(Podcasts ms, string insertuser)
        {
            return PDAL.AddNew(ms, insertuser);
        }

        public bool Update(Podcasts ms, string insertuser)
        {
            return PDAL.Update(ms, insertuser);
        }

        public Podcasts Details(int NewID)
        {
            return PDAL.Details(NewID);
        }
    }
}
