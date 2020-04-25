using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using OasisAlajuelaWebSite.Models;

namespace OasisAlajuelaWebSite.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private GroupsBL GBL = new GroupsBL();
        private UsersBL UBL = new UsersBL();
        private RightsBL RRBL = new RightsBL();
        private ResourcesBL RBL = new ResourcesBL();

        public ActionResult Index()
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString());
            if (validation.ReadRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                ViewBag.WriteRight = validation.WriteRight;
                var data = GBL.FullList();
                return View(data.ToList());
            }
        }

        public ActionResult AddNew()
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Groups GP = new Groups();
                return View(GP);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Groups detail)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();
                var r = GBL.AddNew(detail, InsertUser);
                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    detail.ActionType = "CREATE";
                    return View(detail);
                }
            }
            else
            {
                return View(detail);
            }
        }

        public ActionResult Edit(int id)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            var validation = RRBL.ValidationRights(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), "Index");
            if (validation.WriteRight == false)
            {
                ViewBag.Mensaje = "Usted no esta autorizado para ingresar a esta seccion, si necesita acceso contacte con un administrador.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                Groups GP = GBL.Details(id);
                return View(GP);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Groups detail)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();
                detail.ActionType = "UPDATE";
                var r = GBL.Update(detail, InsertUser);
                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {                    
                    return View(detail);
                }
            }
            else
            {
                return View(detail);
            }
        }

        public ActionResult Remove(int id)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            string InsertUser = User.Identity.GetUserName();
            Groups GP = new Groups
            {
                GroupID = id,
                ActionType = "CHGST"
            };

            var r = GBL.Update(GP, InsertUser);

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

        public ActionResult AddNewUG(int id)
        {
            MultiSelectNewUG model = new MultiSelectNewUG
            {
                GroupID = id,
                SelectedMultiId = new List<int>(),
                SelectedLst = new List<Users>()
            };

            try
            {
                this.ViewBag.UsersList = this.GetUsersList(id);
                this.ViewBag.GroupID = id;
                Groups GP = GBL.Details(id);
                this.ViewBag.GroupName = GP.GroupName;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewUG(MultiSelectNewUG model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model.SelectedMultiId)
                    {
                        UsersGroups UG = new UsersGroups
                        {
                            UserID = item,
                            GroupID = model.GroupID
                        };
                        var r = GBL.AddUserGroup(UG, User.Identity.GetUserName());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            this.ViewBag.UsersList = this.GetUsersList(model.GroupID);
            this.ViewBag.GroupID = model.GroupID;
            Groups GP = GBL.Details(model.GroupID);
            this.ViewBag.GroupName = GP.GroupName;
            model.ActionType = "CREATE";
            return this.View(model);
        }

        public ActionResult RemoveUG(int id)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            string InsertUser = User.Identity.GetUserName();

            UsersGroups UG = new UsersGroups()
            {
                UserGroupID = id
            };

            var r = GBL.RemoveUG(UG, InsertUser);

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

        public ActionResult AddNewRG(int id)
        {
            MultiSelectNewRG model = new MultiSelectNewRG
            {
                GroupID = id,
                SelectedMultiId = new List<int>(),
                SelectedLst = new List<ResourceTypes>()
            };

            try
            {
                this.ViewBag.RTList = this.GetRTList(id);
                this.ViewBag.GroupID = id;
                Groups GP = GBL.Details(id);
                this.ViewBag.GroupName = GP.GroupName;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewRG(MultiSelectNewRG model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model.SelectedMultiId)
                    {
                        ResourcesGroups UG = new ResourcesGroups
                        {
                            ResourceTypeID = item,
                            GroupID = model.GroupID
                        };
                        var r = GBL.AddRTGroup(UG, User.Identity.GetUserName());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            this.ViewBag.RTList = this.GetRTList(model.GroupID);
            this.ViewBag.GroupID = model.GroupID;
            Groups GP = GBL.Details(model.GroupID);
            this.ViewBag.GroupName = GP.GroupName;
            model.ActionType = "CREATE";
            return this.View(model);
        }

        public ActionResult RemoveRG(int id)
        {
            UBL.InsertActivity(User.Identity.GetUserName(), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), DateTime.Now);
            string InsertUser = User.Identity.GetUserName();

            ResourcesGroups UG = new ResourcesGroups()
            {
                ResourceGroupID = id
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
        private IEnumerable<SelectListItem> GetUsersList(int id)
        {
            SelectList lstobj = null;

            try
            {
                var data = from r in UBL.List()
                           where !(from d in GBL.UserList(id)
                                   select d.UserID).Contains(r.UserID)
                           select r;

                //var list = this.GBL.List().Select(p => new SelectListItem { Value = p.GroupID.ToString(), Text = p.GroupName });
                var list = data.Select(p => new SelectListItem { Value = p.UserID.ToString(), Text = p.FullName});

                lstobj = new SelectList(list, "Value", "Text");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstobj;
        }

        private IEnumerable<SelectListItem> GetRTList(int id)
        {
            SelectList lstobj = null;

            try
            {
                var data = from r in RBL.TypeList(string.Empty)
                           where !(from d in GBL.RTList(id)
                                   select d.ResourceTypeID).Contains(r.ResourceTypeID)
                           select r;

                //var list = this.GBL.List().Select(p => new SelectListItem { Value = p.GroupID.ToString(), Text = p.GroupName });
                var list = data.Select(p => new SelectListItem { Value = p.ResourceTypeID.ToString(), Text = p.TypeName });

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