using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BL;
using ET;
using Microsoft.AspNet.Identity;
using PagedList;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class SermonsController : Controller
    {
        private YouTubeBL YBL = new YouTubeBL();
        private SermonsBL SBL = new SermonsBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL USBL = new UsersBL();
        private MinistersBL MBL = new MinistersBL();

        private String UploadedVideoId { get; set; }

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                //var data = from d in YBL.Youtubelist(50)
                //           orderby d.PublishedAt descending
                //           select d;

                var list = from d in SBL.List(true)
                           select d;

                var u = from us in USBL.List()
                        where us.UserName == User.Identity.GetUserName()
                        select us;

                int roleID = u.FirstOrDefault().RoleID;

                if (roleID == 2 || roleID == 3 || roleID == 4)
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";                    
                }

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                ViewBag.Write = validation.WriteRight;

                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(b => b.Title.Contains(searchString) || b.Tags.Contains(searchString));
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));

            }
        }

        [Authorize]
        public ActionResult History(string currentFilter, string searchString, int? page)
        {
            var list = from n in SBL.List(false)
                       select n;

            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(b => b.Title.Contains(searchString) || b.Tags.Contains(searchString));
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }

        }

        [Authorize]
        public ActionResult AddNew()
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Sermons MS = new Sermons()
                {
                    MinisterList = MBL.List(true),
                    SermonDate = DateTime.Today                    
                };
                return View(MS);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNew(Sermons MS)
        {
            String FileExt = Path.GetExtension(MS.file.FileName).ToUpper();

            MS.BannerExt = FileExt;

            Stream str = MS.file.InputStream;
            BinaryReader Br = new BinaryReader(str);
            Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

            MS.BannerData = FileDet;

            string InsertUser = User.Identity.GetUserName();

            YouTubeVideo NewVideo = new YouTubeVideo()
            {
                Title = MS.Title,
                Description = MS.Description,
                Tags = MS.Tags,
                VideoData = MS.fileVideo.InputStream
            };

            string ytID = await YBL.Insert(NewVideo);

            if (ytID != null)
            {
                MS.SermonURL = "https://youtu.be/" + ytID;

                var r = SBL.AddNew(MS, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    MS.ActionType = "CREATE";
                    MS.MinisterList = MBL.List(true);
                    return View(MS);
                }
            }
            else
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [Authorize]
        public ActionResult ChangeStatus(int id)
        {

            string InsertUser = User.Identity.GetUserName();

            Sermons New = new Sermons()
            {
                SermonID = id,
                SermonDate = DateTime.Today
            };

            var r = SBL.Update(New, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return this.RedirectToAction("Index");
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Sermons Event = SBL.Details(id);
                Event.MinisterList = MBL.List(true);
                return View(Event);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sermons MS)
        {
            string InsertUser = User.Identity.GetUserName();

            if (MS.file != null)
            {
                String FileExt = Path.GetExtension(MS.file.FileName).ToUpper();

                MS.BannerExt = FileExt;

                if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                {
                    Stream str = MS.file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                    MS.BannerData = FileDet;

                    var r = SBL.Update(MS, InsertUser);

                    if (!r)
                    {
                        ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        MS.ActionType = "UPDATE";
                        MS.MinisterList = MBL.List(true);
                        return View(MS);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(String.Empty, "Por favor seleccione un banner en formato JPG, JPEG o PNG.");
                    return View(MS);
                }
            }
            else
            {
                var r = SBL.Update(MS, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    MS.ActionType = "UPDATE";
                    MS.MinisterList = MBL.List(true);
                    return View(MS);
                }
            }
        }

        
    }
}