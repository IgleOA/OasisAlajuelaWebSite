using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class AuditoriumLayout
    {
        public List<Block> Blocks { get; set; }
    }

    public class Block
    {
        public string BlockID { get; set; }

        public int Rows { get; set; }

        public List<Row> RowData { get; set; }
    }
    public class Row
    {
        public string RowID { get; set; }

        public string BlockID { get; set; }

        public int Label { get; set; }

        public int SeatsNbr { get; set; }

        public List<Seats> SeatsData { get; set; }
    }
    public class Seats
    {
        public string SeatID { get; set; }

        public string RowID { get; set; }

        public int Label { get; set; }

        public bool Reserved { get; set; }

        public int Booked { get; set; }

        public string BookedFor { get; set; }

    }
}
