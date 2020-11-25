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

        public bool AdminResetPassword(int UserID, string InsertUser)
        {
            return UDAL.AdminResetPassword(UserID, InsertUser);
        }

        public List<Users> List()
        {
            return UDAL.List();
        }

        public List<Users> Subscribers(int ResourceID, bool IsPublic)
        {
            return UDAL.Subscribers(ResourceID, IsPublic);
        }

        public bool Update(Users user, string insertuser)
        {
            return UDAL.Update(user, insertuser);
        }

        public Users Details(int userid)
        {
            return UDAL.Details(userid);
        }

        public Users DetailsbyEmail(string email)
        {
            return UDAL.DetailsbyEmail(email);
        }

        public bool InsertActivity(string UserName, string Controller, string Action, DateTime ActivityDate)
        {
            return UDAL.InsertActivity(UserName, Controller, Action, ActivityDate);
        }

        public bool AddLogin (LoginRecord Login)
        {
            return UDAL.AddLogin(Login);
        }

        public bool RemoveSubscriber(string email)
        {
            return UDAL.RemoveSubscriber(email);
        }
    }
}
