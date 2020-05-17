using DAL;
using System.Collections.Generic;
using ET;

namespace BL
{
    public class LeadershipBL
    {
        private LeadershipDAL LDAL = new LeadershipDAL();

        public List<Leadership> List()
        {
            return LDAL.List();
        }

        public bool AddNew(Leadership Leader, string insertuser)
        {
            return LDAL.AddNew(Leader, insertuser);
        }

        public Leadership Details(int leaderID)
        {
            return LDAL.Details(leaderID);
        }

        public bool Update(Leadership Leader, string insertuser)
        {
            return LDAL.Update(Leader, insertuser);
        }
    }
}
