using DAL;
using ET;
using System.Collections.Generic;

namespace BL
{
    public class EnrollmentsBL
    {
        private EnrollmentsDAL EDAL = new EnrollmentsDAL();

        public bool Add(Enrollments Details, string InsertUser)
        {
            return EDAL.Add(Details, InsertUser);
        }

        public List<Enrollments> List(bool HistoryFlag)
        {
            return EDAL.List(HistoryFlag);
        }

        public bool AddUser(EnrolledUsers Detail, string InsertUser)
        {
            return EDAL.AddUser(Detail, InsertUser);
        }

        public bool RemoveEnrollment (int EnrollmentID, string InsertUser)
        {
            return EDAL.RemoveEnrollment(EnrollmentID, InsertUser);
        }

        public bool RemoveUser(int RegisterID)
        {
            return EDAL.RemoveUser(RegisterID);
        }

        public bool ApproveEnrollment(int EnrollmentID)
        {
            return EDAL.ApproveEnrollment(EnrollmentID);
        }

        public List<EnrolledUsers> UserList(int EnrollmentID)
        {
            return EDAL.UserList(EnrollmentID);
        }

        public bool Update(Enrollments Details, string InsertUser)
        {
            return EDAL.Update(Details, InsertUser);
        }

        public Enrollments Details (int EnrollmentID)
        {
            return EDAL.Details(EnrollmentID);
        }
    }
}
