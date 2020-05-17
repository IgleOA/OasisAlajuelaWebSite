using ET;
using DAL;

namespace BL
{
    public class ReservationEventDetailBL
    {
        private ReservationEventDetailDAL WDAL = new ReservationEventDetailDAL();

        public ReservationEventDetail Details(int WorhsipID, int UserID)
        {
            return WDAL.Details(WorhsipID, UserID);
        }
    }
}
