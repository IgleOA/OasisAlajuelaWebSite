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

        public List<Contacts> List(ContactListRequest model)
        {
            return CDAL.List(model);
        }

        public Contacts Details(int ContactID)
        {
            return CDAL.Details(ContactID);
        }
    }
}
