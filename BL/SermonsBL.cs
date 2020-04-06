using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class SermonsBL
    {
        private SermonsDAL SDAL = new SermonsDAL();

        public List<Sermons> List(bool activeflag)
        {
            return SDAL.List(activeflag);
        }

        public bool AddNew(Sermons ms, string insertuser)
        {
            return SDAL.AddNew(ms, insertuser);
        }

        public bool Update(Sermons ms, string insertuser)
        {
            return SDAL.Update(ms, insertuser);
        }

        public Sermons Details(int NewID)
        {
            return SDAL.Details(NewID);
        }
    }
}
