using ET;
using DAL;

namespace BL
{
    public class AuditoriumLayoutBL
    {
        private AuditoriumLayoutDAL ADAL = new AuditoriumLayoutDAL();

        public AuditoriumLayout Layout(int EventID)
        {
            return ADAL.Layout(EventID);
        }
    }
}
