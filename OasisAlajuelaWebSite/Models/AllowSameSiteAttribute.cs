using System.Web.Mvc;

namespace OasisAlajuelaWebSite.Models
{
    public class AllowSameSiteAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = filterContext.RequestContext.HttpContext.Response;

            if (response != null)
            {
                response.AddHeader("Set-Cookie", "HttpOnly;Secure;SameSite=Strict");
                //response.AddHeader("Set-Cookie", "HttpOnly;Secure;SameSite=Strict");
                //Add more headers...
            }

            base.OnActionExecuting(filterContext);
        }
    }
}