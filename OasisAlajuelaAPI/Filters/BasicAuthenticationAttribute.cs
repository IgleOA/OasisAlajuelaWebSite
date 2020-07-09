using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BL;
using ET;

namespace OasisAlajuelaAPI.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private UsersBL UBL = new UsersBL();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
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

                //var isValid = userName == "andy" && password == "password";

                if (LoginUser.UserID > 0)
                {
                    var principal = new GenericPrincipal(new GenericIdentity(userName), null);
                    Thread.CurrentPrincipal = principal;

                    //actionContext.Response =
                    //   actionContext.Request.CreateResponse(HttpStatusCode.OK,
                    //      "User " + userName + " successfully authenticated");

                    return;
                }
            }

            HandleUnathorized(actionContext);
        }

        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Data' location = 'http://localhost:");
        }
    }
}