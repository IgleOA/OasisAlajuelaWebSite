using ET;
using BL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RotativaHQ.MVC5;

namespace OasisAlajuelaAPI.Controllers
{
    public class HomeController : Controller
    {
        private ReservationsBL RBL = new ReservationsBL();
        private UpcommingEventsBL UBL = new UpcommingEventsBL();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }        

        public ActionResult ReservationsPrintVersion(int id)
        {
            ReservationListRequest ListRequest = new ReservationListRequest()
            {
                EventID = id,
                GUID = null
            };

            var EventDetails = UBL.Details(id);

            EventDetails.ReservationList = RBL.List(ListRequest);

            return View(EventDetails);
        }

        public ActionResult ReservationsPDF(int id)
        {
            ReservationListRequest ListRequest = new ReservationListRequest()
            {
                EventID = id,
                GUID = null
            };

            var EventDetails = UBL.Details(id);

            EventDetails.ReservationList = RBL.List(ListRequest);

            string filename = "Reservas_" 
                              + EventDetails.Title + "_"
                              + EventDetails.ScheduledDate.ToString().Substring(0,13) + ".pdf";

            return new ViewAsPdf("ReservationsPrintVersion", EventDetails) { FileName = filename };

        }
    }
}
