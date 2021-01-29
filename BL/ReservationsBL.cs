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

        public bool AddNew (ReservationRequest model, string insertuser)
        {
            return RDAL.AddNew(model, insertuser);
        }
    }
}
