using System;
using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using OasisAlajuelaWebSite.Models;

namespace OasisAlajuelaWebSite.Controllers
{
    public class SermonsController : Controller
    {
        YouTubeBL YBL = new YouTubeBL();
        SermonsBL SBL = new SermonsBL();

        public ActionResult Index(string id)
        {
            //var data = from d in YBL.Youtubelist(50)
            //           orderby d.PublishedAt descending
            //           select d;

            var data = SBL.List();

            return View(data.ToList());
        }
    }
}