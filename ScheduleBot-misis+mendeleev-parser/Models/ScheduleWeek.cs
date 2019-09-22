using System.Collections.Generic;
using ScheduleBot_misis_mend_parsers.Models;

namespace ScheduleBot_misis_mendeleev_parser.Models
{
	public class ScheduleWeek
	{
		public int ScheduleWeekId { get; set; }
        public int Week { get; set; }
		public Group Group { get; set; }
		public ICollection<ScheduleDay> Day { get; set; }

		public ScheduleWeek()
		{
			Day = new List<ScheduleDay>();
		}
	}
}