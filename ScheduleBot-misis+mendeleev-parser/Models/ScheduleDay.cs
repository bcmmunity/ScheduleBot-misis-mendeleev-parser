using System;
using System.Collections.Generic;

namespace ScheduleBot_misis_mendeleev_parser.Models
{
	public class ScheduleDay
	{
		public int ScheduleDayId { get; set; }
        public DateTime Date { get; set; }
		public int Day { get; set; }
		public ICollection<Lesson> Lesson { get; set; }

		public ScheduleDay()
		{
			Lesson = new List<Lesson>();
		}
	}
}
