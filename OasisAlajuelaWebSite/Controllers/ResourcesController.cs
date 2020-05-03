using ET;
using BL;
using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Web;
using OasisAlajuelaWebSite.Models;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class ResourcesController : Controller
    {
        private RightsBL RRBL = new RightsBL();
        private UsersBL USBL = new UsersBL();
        private ResourcesBL RBL = new ResourcesBL();
        private YouTubeBL YBL = new YouTubeBL();
        private GroupsBL GBL = new GroupsBL();

        public ActionResult Index()
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));

            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Users user = USBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                if (user.RoleName.Contains("Admin"))
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                }

                var data = RBL.TypeList(User.Identity.GetUserName());
                ViewBag.Write = validation.WriteRight;               
                return View(data.ToList());

            }
        }

        public ActionResult Type(int id, string currentFilter, string searchString, int? page)
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));

            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Users user = USBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                if (user.RoleName.Contains("Admin"))
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

                var list = from l in RBL.ResourceList(id, true)
                           select l;

                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(b => b.FileName.Contains(searchString) || b.Description.Contains(searchString));
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);

                ResourceTypes TypeData = (from r in RBL.TypeList(User.Identity.GetUserName())
                                          where r.ResourceTypeID == id
                                          select r).FirstOrDefault();

                ViewBag.Resource = TypeData.TypeName;
                ViewBag.ResourceTypeID = TypeData.ResourceTypeID;

                ViewBag.Write = validation.WriteRight;
                ViewBag.CountResults = list.Count();

                return View(list.ToPagedList(pageNumber, pageSize));

            }

        }

        public ActionResult AddNewRT()
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));

            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(),"Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                ResourceTypes RT = new ResourceTypes();
                RT.IsPublic = true;
                return View(RT);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewRT(ResourceTypes RT)
        {
            String FileExt = Path.GetExtension(RT.file.FileName).ToUpper();

            RT.TypeImageExt = FileExt;

            if (FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
            {
                Stream str = RT.file.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                RT.TypeImage = FileDet;

                string InsertUser = User.Identity.GetUserName();

                var r = RBL.AddNewResourceType(RT, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {

                    RT.ActionType = "CREATE";
                    return View(RT);
                }
            }
            else
            {
                this.ModelState.AddModelError(String.Empty, "La imagen selecciona es de un formato invalido o no aceptado.");
                return View(RT);
            }
        }

        public ActionResult AddNewResource(int id = 0)
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Resources MS = new Resources();

                if(id == 0)
                {
                    MS.TypeList = RBL.TypeList(User.Identity.GetUserName());
                }
                else
                {
                    var data = from r in RBL.TypeList(User.Identity.GetUserName())
                               where r.ResourceTypeID == id
                               select r;

                    MS.TypeList = data.ToList();
                }                

                return View(MS);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewResource(Resources MS)
        {
            if (MS.FileType == "Video")
            {
                var valURL = UrlIsValid(MS.FileURL);

                if (!valURL)
                {
                    var data = from rd in RBL.TypeList(User.Identity.GetUserName())
                               where rd.ResourceTypeID == MS.ResourceTypeID
                               select rd;

                    MS.TypeList = data.ToList();
                    this.ModelState.AddModelError(String.Empty, "El video proporcionado no existe en YouTube o es incorrecto.");
                    return View(MS);
                }
            }
            else
            {
                String FileExt = Path.GetExtension(MS.file.FileName).ToUpper();

                MS.FileExt = FileExt;

                var valformat = ValidationFormat(MS.FileType, MS.FileExt);

                if (!valformat)
                {
                    var data = from rd in RBL.TypeList(User.Identity.GetUserName())
                               where rd.ResourceTypeID == MS.ResourceTypeID
                               select rd;

                    MS.TypeList = data.ToList();
                    this.ModelState.AddModelError(String.Empty, "El formato de archivo no concuerda con el tipo de archivo seleccionado o es un formato invalido.");
                    return View(MS);
                }

                else
                {
                    Stream str = MS.file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                    MS.FileData = FileDet;
                }
            }

            string InsertUser = User.Identity.GetUserName();

            var r = RBL.AddNewResource(MS, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                MS.ActionType = "CREATE";

                var data = from rd in RBL.TypeList(User.Identity.GetUserName())
                           where rd.ResourceTypeID == MS.ResourceTypeID
                           select rd;

                MS.TypeList = data.ToList();
                return View(MS);
            }
        }

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            var FileById = RBL.ResourceDetails(id);
            var FileType = "application/" + FileById.FileExt;
            var FileName = FileById.FileName.Replace(" ", "_") + "." + FileById.FileExt;

            return File(FileById.FileData, FileType, FileName);
        }

        public ActionResult GetFile(int id)
        {
            var FileById = RBL.ResourceDetails(id);

            string strFile = FileById.FileName.Replace(" ", "_") + "." + FileById.FileExt;

            return File(FileById.FileData, System.Net.Mime.MediaTypeNames.Application.Octet, strFile);
        }

        public ActionResult Edit(int id = 0)
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Resources MS = RBL.ResourceDetails(id);

                MS.TypeList = RBL.TypeList(User.Identity.GetUserName());                

                return View(MS);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Resources MS)
        {
            if (MS.ResourceTypeID > 0 && MS.FileName.Length > 0 && MS.Description.Length > 0)
            {
                string InsertUser = User.Identity.GetUserName();

                if (MS.FileType == "Video")
                {
                    var valURL = UrlIsValid(MS.FileURL);

                    if (!valURL)
                    {
                        MS.TypeList = RBL.TypeList(User.Identity.GetUserName());
                        this.ModelState.AddModelError(String.Empty, "El video proporcionado no existe en YouTube o es incorrecto.");
                        return View(MS);
                    }
                }
                 
                var r = RBL.Update(MS, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    MS.ActionType = "UPDATE";
                    MS.TypeList = RBL.TypeList(User.Identity.GetUserName());
                    return View(MS);
                }
            }
            else
            {
                MS.TypeList = RBL.TypeList(User.Identity.GetUserName());
                this.ModelState.AddModelError(String.Empty, "Todos los campos son obligatorios.");
                return View(MS);                
            }
        }

        public ActionResult ChangeStatus(int id = 0)
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Resources MS = RBL.ResourceDetails(id);
                MS.ActionType = "CHGST";
                string InsertUser = User.Identity.GetUserName();

                var r = RBL.Update(MS, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    return this.RedirectToAction("Type", new { id = MS.ResourceTypeID });
                }                
            }
        }

        public bool ValidationFormat(string FileType, string FileExt)
        {
            string Extensions = string.Empty;
            bool isValid = false;

            if(FileType == "Audio")
            {
                Extensions = "MP3,AAC,OGG,WAV";
                List<string> allowedExtensions = Extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                isValid = allowedExtensions.Any(x => FileExt.EndsWith(x));
            }

            if (FileType == "Documento")
            {
                Extensions = "PDF,TXT,DOC,DOCX";
                List<string> allowedExtensions = Extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                isValid = allowedExtensions.Any(x => FileExt.EndsWith(x));
            }
            return isValid;
        }

        public bool UrlIsValid(string url)
        {
            bool isValid = false;

            var uri = new Uri(url);

            // you can check host here => uri.Host <= "www.youtube.com"

            var query = HttpUtility.ParseQueryString(uri.Query);

            var videoId = string.Empty;

            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }

            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                request.Method = "HEAD"; //Get only the header information -- no need to download any content

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    int statusCode = (int)response.StatusCode;

                    var r = YBL.YoutubeVideoValidation(videoId); // Check validation from YouTube

                    if (statusCode >= 100 && statusCode < 400 && r.ActiveFlag == true) //Good requests
                    {
                        isValid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                isValid = false;
            }

            return isValid;
        }

        public ActionResult AddGroup(int id)
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));
            MultiSelectNewRTG model = new MultiSelectNewRTG
            {
                ResourceTypeID = id,
                SelectedMultiGroupId = new List<int>(),
                SelectedGroupLst = new List<Groups>()
            };

            try
            {
                this.ViewBag.GroupList = this.GetGroupList(id);
                this.ViewBag.RTID = id;
                var data = from d in RBL.TypeList(string.Empty)
                           where d.ResourceTypeID == id
                           select d;
                this.ViewBag.TypeName = data.FirstOrDefault().TypeName;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGroup(MultiSelectNewRTG model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model.SelectedMultiGroupId)
                    {
                        ResourcesGroups UG = new ResourcesGroups
                        {
                            ResourceTypeID = model.ResourceTypeID,
                            GroupID = item
                        };
                        var r = GBL.AddRTGroup(UG, User.Identity.GetUserName());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            this.ViewBag.GroupList = this.GetGroupList(model.ResourceTypeID);
            this.ViewBag.RTID = model.ResourceTypeID;
            var data = from d in RBL.TypeList(string.Empty)
                       where d.ResourceTypeID == model.ResourceTypeID
                       select d;
            this.ViewBag.TypeName = data.FirstOrDefault().TypeName;
            model.ActionType = "CREATE";
            return this.View(model);
        }

        public ActionResult RemoveURG(int ResourceTypeID, int GroupID)
        {
            USBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now.AddHours(-6));
            string InsertUser = User.Identity.GetUserName();

            ResourcesGroups UG = new ResourcesGroups()
            {
                ResourceTypeID = ResourceTypeID,
                GroupID = GroupID
            };

            var r = GBL.RemoveRG(UG, InsertUser);

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

        private IEnumerable<SelectListItem> GetGroupList(int id)
        {
            SelectList lstobj = null;

            try
            {
                var data = from r in GBL.List()
                           where !(from d in GBL.ListbyRT(id)
                                   select d.GroupID).Contains(r.GroupID)
                           select r;

                //var list = this.GBL.List().Select(p => new SelectListItem { Value = p.GroupID.ToString(), Text = p.GroupName });
                var list = data.Select(p => new SelectListItem { Value = p.GroupID.ToString(), Text = p.GroupName });

                lstobj = new SelectList(list, "Value", "Text");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstobj;
        }
    }
}