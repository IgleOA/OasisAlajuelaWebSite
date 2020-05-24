using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class UserNotesBL
    {
        private UserNotesDAL UDAL = new UserNotesDAL();

        public List<UserNotes> List (string UserName, bool HistoryFlag)
        {
            return UDAL.List(UserName, HistoryFlag);
        }

        public bool AddNote (UserNotes Note, string InsertUser)
        {
            return UDAL.AddNote(Note, InsertUser);
        }

        public bool UpdateNote (ResponseUserNote Note, string InsertUser)
        {
            return UDAL.UpdateNote(Note, InsertUser);
        }

        public UserNotes Details (int NoteID)
        {
            return UDAL.Details(NoteID);
        }
    }
}
