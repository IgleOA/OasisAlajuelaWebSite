using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using OasisAlajuelaWebSite.Models;
using PagedList;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private UserNotesBL UNBL = new UserNotesBL();
        private RightsBL RRBL = new RightsBL();
        private UsersBL USBL = new UsersBL();
        private GroupsBL GBL = new GroupsBL();

        public ActionResult _InitialNote()
        {
            UserNotes Note = UNBL.List(User.Identity.GetUserName(), false).FirstOrDefault();

            ViewBag.FullName = USBL.Details(Note.UserID).FullName;

            return View(Note);
        }

        public ActionResult AddNew()
        {
            MultiSelectNewNote model = new MultiSelectNewNote
            {
                SelectedMultiId = new List<int>(),
                SelectedLst = new List<Users>()
            };

            try
            {
                this.ViewBag.UsersList = this.GetUsersList();                
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return this.View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(MultiSelectNewNote model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model.SelectedMultiId)
                    {
                        UserNotes UG = new UserNotes
                        {
                            UserID = item,
                            RequestNote = model.RequestNote,
                            ResponseRequired = model.ResponseRequired
                        };
                        var r = UNBL.AddNote(UG, User.Identity.GetUserName());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            this.ViewBag.UsersList = this.GetUsersList();
            model.ActionType = "CREATE";
            return this.View(model);
        }

        private IEnumerable<SelectListItem> GetUsersList()
        {
            SelectList lstobj = null;

            try
            {
                var data = from r in USBL.List()
                           select r;

                var list = data.Select(p => new SelectListItem { Value = p.UserID.ToString(), Text = p.FullName });

                lstobj = new SelectList(list, "Value", "Text");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstobj;
        }

        public ActionResult AddNewByUser(int id)
        {
            UserNotes model = new UserNotes()
            {
                UserList = USBL.List().Where(x => x.UserID == id).ToList()
            };

            return this.View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewByUser(UserNotes model)
        {
            if (ModelState.IsValid)
            {                   
                var r = UNBL.AddNote(model, User.Identity.GetUserName());

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    model.ActionType = "CREATE";
                    model.UserList = USBL.List().Where(x => x.UserID == model.UserID).ToList();
                    return this.View(model);
                }
            }
            model.UserList = USBL.List().Where(x => x.UserID == model.UserID).ToList();
            return this.View(model);
        }

        public ActionResult AddNewByGroup(int id)
        {
            GroupNote model = new GroupNote()
            {
                GroupList = GBL.List().Where(x => x.GroupID == id).ToList()
            };

            return this.View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewByGroup(GroupNote model)
        {
            if (ModelState.IsValid)
            {
                List<UsersGroups> Users = GBL.UserList(model.GroupID);

                foreach(var user in Users)
                {
                    UserNotes Note = new UserNotes()
                    {
                        UserID = user.UserID,
                        RequestNote = model.RequestNote,
                        ResponseRequired = model.ResponseRequired
                    };

                    var r = UNBL.AddNote(Note, User.Identity.GetUserName());
                }
                
                model.ActionType = "CREATE";
                model.GroupList = GBL.List().Where(x => x.GroupID == model.GroupID).ToList();
                return this.View(model);
                
            }
            model.GroupList =  GBL.List().Where(x => x.GroupID == model.GroupID).ToList();
            return this.View(model);
        }


        public ActionResult AddNewAll()
        {
            AllNote Note = new AllNote();

            return this.View(Note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewAll(AllNote model)
        {
            if (ModelState.IsValid)
            {
                List<Users> Userslist = USBL.List();

                foreach (var user in Userslist)
                {
                    UserNotes Note = new UserNotes()
                    {
                        UserID = user.UserID,
                        RequestNote = model.RequestNote,
                        ResponseRequired = model.ResponseRequired
                    };

                    var r = UNBL.AddNote(Note, User.Identity.GetUserName());
                }

                model.ActionType = "CREATE";
                return this.View(model);

            }
            return this.View(model);
        }

        public void ReadNote(int id)
        {
            ResponseUserNote Note = new ResponseUserNote()
            {
                NoteID = id,
                ResponseNote = "READ MESSAGE",
                ActionType = "READ"
            };

            var r = UNBL.UpdateNote(Note, User.Identity.GetUserName());
        }

        public ActionResult ResponseNote(int id)
        {
            ResponseUserNote Note = new ResponseUserNote()
            {
                NoteID = id
            };

            Users user = USBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

            if (user.RoleName.Contains("Admin"))
            {
                ViewBag.Admin = true;
            }
            else
            {
                ViewBag.Admin = false;
            }

            UserNotes MainNote = UNBL.Details(id);
            ViewBag.RequestNote = MainNote.RequestNote;
            return View(Note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResponseNote(ResponseUserNote model)
        {
            if (ModelState.IsValid)
            {
                model.ActionType = "UPDATE";

                var r = UNBL.UpdateNote(model, User.Identity.GetUserName());

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    return this.View(model);
                }
            }
            
            return this.View(model);
        }

        public ActionResult Index(string currentFilter, string searchString, int? page)
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
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var list = from b in UNBL.List(User.Identity.GetUserName(),true)
                           select b;

                Users user = USBL.List().Where(x => x.UserName == User.Identity.GetUserName()).FirstOrDefault();

                if (user.RoleName.Contains("Admin"))
                {
                    ViewBag.Layout = "~/Views/Shared/_AdminLayout.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/_MainLayout.cshtml";
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(b => b.InsertedBy.ToLower().Contains(searchString.ToLower()) | b.RequestNote.ToLower().Contains(searchString.ToLower()) | b.ResponseNote.ToLower().Contains(searchString.ToLower()));
                }

                ViewBag.UsersCount = list.Count();
                ViewBag.Write = validation.WriteRight;
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult _MessageMenu()
        {
            List<UserNotes> data = UNBL.List(User.Identity.GetUserName(),true).Where(x => x.ReadFlag == false).ToList();

            ViewBag.MessageNbr = data.Count();
            return View();
        }
    }
}