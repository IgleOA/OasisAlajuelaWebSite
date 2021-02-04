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

        public List<Contacts> ContactList(ContactListRequest model)
        {
            return CDAL.ContactList(model);
        }

        public List<ContactType> ContactTypesList ()
        {
            return CDAL.ContactTypeList();
        }

        public ContactType Details(int contacttypeid)
        {
            return CDAL.Details(contacttypeid);
        }
    }
}
