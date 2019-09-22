using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScheduleBot_misis_mendeleev_parser.Models
{
    public class PostRequest
    {
        public string University { get; set; }
        public string Facility { get; set; }
        public string Course { get; set; }
        public string Group { get; set; }
        public byte Type { get; set; }

        public ScheduleWeek Week { get; set; }
    }
}

