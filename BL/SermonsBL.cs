using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class SermonsBL
    {
        private SermonsDAL SDAL = new SermonsDAL();

        public List<Sermons> List()
        {
            return SDAL.List();
        }
    }
}
