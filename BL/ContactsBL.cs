using DAL;
using ET;
using System.Collections.Generic;

namespace BL
{
    public class ContactsBL
    {
        private ContactsDAL CDAL = new ContactsDAL();

        public bool Add(Contacts Model)
        {
            return CDAL.Add(Model);
        }

        public List<Contacts> List(bool HistoryFlag)
        {
            return CDAL.List(HistoryFlag);
        }

        public Contacts Details(int ContactID)
        {
            return CDAL.Details(ContactID);
        }
    }
}
