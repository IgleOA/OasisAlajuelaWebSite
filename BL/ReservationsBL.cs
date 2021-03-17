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

        public List<ReservationResult> AddNew (ReservationRequest model, string insertuser)
        {
            return RDAL.AddNew(model, insertuser);
        }

        public List<Reservations> List (ReservationListRequest model)
        {
            return RDAL.List(model);
        }

        public List<Reserver> Reservers()
        {
            return RDAL.Reservers();
        }

        public bool Update(Reservations model, string insertuser)
        {
            return RDAL.Update(model, insertuser);
        }

        public bool RegisterAttend(RegisterAttend model)
        {
            return RDAL.RegisterAttend(model);
        }
    }
}
