using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class MinistriesBL
    {
        private MinistriesDAL MBL = new MinistriesDAL();

        public List<Ministries> List (bool activeflag)
        {
            return MBL.List(activeflag);
        }       
    }
}
