using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.ModelBinding;

namespace OasisAlajuelaAPI.Filters
{
    public class ApiKeyAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.GetValues("ApiKey");

            if(authHeader !=null)
            {
                IEnumerable<string> Authvalues = actionContext.Request.Headers.GetValues("ApiKey");
                var authToken = Authvalues.FirstOrDefault();
                var isValid = authToken == "929c2771z5rcvp7";

                if(isValid)
                {
                    var principal = new GenericPrincipal(new GenericIdentity(authToken), null);
                    Thread.CurrentPrincipal = principal;
                    return;
                }
            }
            HandleUnathorized(actionContext);
        }

        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);            
        }
    }
}