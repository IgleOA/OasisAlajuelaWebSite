using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OasisAlajuelaWebSite.Models
{
    public class Detail
    {
        public int rowid { get; set; }
        public int Capacity { get; set; }
    }

    public class Auditorium
    {
        public object block { get; set; }
        public IList<Detail> details { get; set; }
    }

    public class AuditoriumModel
    {
        public IList<Auditorium> Auditorium { get; set; }
    }


}