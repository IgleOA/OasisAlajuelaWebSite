using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class MinistriesBL
    {
        private MinistriesDAL MDAL = new MinistriesDAL();

        public List<Ministries> List ()
        {
            return MDAL.List();
        }       

        public bool AddNew(Ministries min, string insertuser)
        {
            return MDAL.AddNew(min, insertuser);
        }

        public Ministries Details(int ministryid)
        {
            return MDAL.Details(ministryid);
        }

        public bool Update(Ministries ministry, string insertuser)
        {
            return MDAL.Update(ministry, insertuser);
        }
    }
}
