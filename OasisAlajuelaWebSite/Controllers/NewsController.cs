using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;
using BL;
using System.IO;
using Microsoft.AspNet.Identity;

namespace OasisAlajuelaWebSite.Controllers
{
    public class NewsController : Controller
    {
        private NewsBL NBL = new NewsBL();

        public ActionResult Index()
        {
            var list = NBL.List();

            return View(list);
        }

        [Authorize]
        public ActionResult AddNew()
        {
            News MS = new News();
            return View(MS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(News MS)
        {
            String FileExt = Path.GetExtension(MS.file.FileName).ToUpper();

            MS.BannerExt = FileExt;

            Stream str = MS.file.InputStream;
            BinaryReader Br = new BinaryReader(str);
            Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

            MS.BannerData = FileDet;
            MS.InsertDate = DateTime.Now;

            string InsertUser = User.Identity.GetUserName();

            var r = NBL.AddNew(MS, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                MS.ActionType = "CREATE";

                return View(MS);
            }            
        }
    }
}