using ET;
using DAL;

namespace BL
{   
    public class UserProfileBL
    {
        private UserProfileDAL UDAL = new UserProfileDAL();

        public UserProfile Detail(int userid)
        {
            return UDAL.Detail(userid);
        }
    }
}
