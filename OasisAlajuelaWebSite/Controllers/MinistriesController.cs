﻿using System;
using System.Linq;
using System.Web.Mvc;
using ET;
using BL;
using OasisAlajuelaWebSite.Models;

namespace OasisAlajuelaWebSite.Controllers
{ 
    [AllowSameSite]
    public class MinistriesController : Controller
    {
        private MinistriesBL MBL = new MinistriesBL();
        // GET: Ministries
        public ActionResult Index()
        {
            var list = MBL.List(true);

            return View(list.ToList());
        }

        public ActionResult _FooterMinistries()
        {
            var list = MBL.List(true);

            return View(list.ToList());
        }
    }
}