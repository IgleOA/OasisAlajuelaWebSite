﻿using System;
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

        public bool RemoveGUID(string GUID, string InsertUser)
        {
            return RDAL.RemoveGUID(GUID, InsertUser);
        }

        public List<Reservations> ReservationsFullInfo(int EventID, int UserID)
        {
            return RDAL.ReservationsFullInfo(EventID, UserID);
        }

        public List<ReservationLevel1> ReservationsMainInfo(int EventID, int UserID)
        {
            return RDAL.ReservationsMainInfo(EventID, UserID);
        }

        public List<ReservationLevel1> ReservationsMaster()
        {
            return RDAL.ReservationsMaster();
        }
    }
}
