using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using DAL;

namespace BL
{
    public class WorshipsBL
    {
        private WorshipsDAL WDAL = new WorshipsDAL();

        public Worships Details(int WorhsipID)
        {
            return WDAL.Details(WorhsipID);
        }
    }
}
