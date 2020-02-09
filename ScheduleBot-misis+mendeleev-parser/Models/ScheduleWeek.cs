using System.Collections.Generic;
using ScheduleBot_misis_mend_parsers.Models;

namespace ScheduleBot_misis_mendeleev_parser.Models
{
	public class ScheduleWeek
	{
		public int ScheduleWeekId { get; set; }
        public int Week { get; set; }
		public Group Group { get; set; }
		public ICollection<ScheduleDay> Days { get; set; }

		public ScheduleWeek()
		{
			Days = new List<ScheduleDay>();
		}
	}
}