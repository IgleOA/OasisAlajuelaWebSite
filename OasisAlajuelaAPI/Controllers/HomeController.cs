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
        private ContactsBL CBL = new ContactsBL();

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

        public ActionResult ContactsPrintVersion(bool historyflag, int contacttypeid)
        {
            var CLR = new ContactListRequest()
            {
                ContactTypeID = contacttypeid,
                HistoryFlag = historyflag
            };

            var CTDetails = CBL.Details(contacttypeid);
            CTDetails.HistoryFlag = historyflag;
            CTDetails.ContactList = CBL.ContactList(CLR);

            return View(CTDetails);
        }

        public ActionResult ContactsPDF(bool historyflag, int contacttypeid)
        {
            var CLR = new ContactListRequest()
            {
                ContactTypeID = contacttypeid,
                HistoryFlag = historyflag
            };

            var CTDetails = CBL.Details(contacttypeid);
            CTDetails.HistoryFlag = historyflag;
            CTDetails.ContactList = CBL.ContactList(CLR);

            string filename = CTDetails.TypeTitle + "_" + DateTime.Today.ToString("dd_MM_yyyy") + ".pdf";

            return new ViewAsPdf("ContactsPrintVersion", CTDetails) { FileName = filename };

        }
    }
}
