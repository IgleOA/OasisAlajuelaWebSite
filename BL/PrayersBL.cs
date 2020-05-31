using System.Collections.Generic;
using DAL;
using ET;

namespace BL
{
    public class PrayersBL
    {
        private PrayersDAL PDAL = new PrayersDAL();

        public bool Add(Prayers Prayer)
        {
            return PDAL.Add(Prayer);
        }

        public List<Prayers> List(bool HistoryFlag)
        {
            return PDAL.List(HistoryFlag);
        }

        public Prayers Details(int PrayerID)
        {
            return PDAL.Details(PrayerID);
        }
    }
}
