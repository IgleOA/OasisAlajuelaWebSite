using BL;
using ET;
using OasisAlajuelaAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace OasisAlajuelaAPI.Controllers
{
    [BasicAuthentication]
    [RequireHttps]
    public class AccountController : ApiController
    {
        private UsersBL UBL = new UsersBL();
        private RolesBL RBL = new RolesBL();
        private GroupsBL GBL = new GroupsBL();

        // GET api/<controller>
        public Users Get()
        {
            var authenticationToken = Request.Headers.Authorization.Parameter;
            var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
            var usernamePasswordArray = decodedAuthenticationToken.Split(':');
            var userName = usernamePasswordArray[0];
            var password = usernamePasswordArray[1];

            // Replace this with your own system of security / means of validating credentials
            Login login = new Login()
            {
                UserName = userName,
                Password = password
            };

            Users LoginUser = UBL.Login(login);

            Users Details = UBL.Details(LoginUser.UserID);

            Details.RolesData = RBL.List().Where(x => x.RoleID == Details.RoleID).FirstOrDefault();
            Details.GroupList = GBL.ListbyUser(Details.UserID);

            return Details;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public string Post([FromBody] string value)
        {
            return "value";
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}