using ET;
using DAL;
using System.Collections.Generic;
using System;

namespace BL
{
    public class UsersBL
    {
        private UsersDAL UDAL = new UsersDAL();

        public bool CheckAvailability(string txtvalidate)
        {
            return UDAL.CheckAvailability(txtvalidate);
        }

        public bool AddUser(Users user, string insertuser)
        {
            return UDAL.AddUser(user, insertuser);
        }

        public Users Login(Login User)
        {
            return UDAL.Login(User);
        }

        public AuthorizationCode AuthCode(string email)
        {
            return UDAL.AuthCode(email);
        }

        public int ValidateGUID(string guid)
        {
            return UDAL.ValidateGUID(guid);
        }

        public bool ResetPassword(ResetPasswordModel model)
        {
            return UDAL.ResetPassword(model);
        }

        public List<Users> List()
        {
            return UDAL.List();
        }

        public bool Update(Users user, string insertuser)
        {
            return UDAL.Update(user, insertuser);
        }

        public Users Details(int userid)
        {
            return UDAL.Details(userid);
        }

        public bool InsertActivity(string UserName, string Controller, string Action, DateTime ActivityDate)
        {
            return UDAL.InsertActivity(UserName, Controller, Action, ActivityDate);
        }

        public bool AddLogin (LoginRecord Login)
        {
            return UDAL.AddLogin(Login);
        }
    }
}
