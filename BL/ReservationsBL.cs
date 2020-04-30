using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using DAL;

namespace BL
{
    public class ReservationsBL
    {
        private ReservationsDAL RDAL = new ReservationsDAL();

        public List<ReserveDetail> AddReservation(Reservations Reservation, string InsertUser)
        {
            return RDAL.AddReservation(Reservation, InsertUser);
        }

        public List<Reservations> Details (string GUID)
        {
            return RDAL.Details(GUID);
        }

        public string Remove(int ReservationID, string InsertUser)
        {
            return RDAL.Remove(ReservationID, InsertUser);
        }
    }
}
