using System;
using System.Collections.Generic;

namespace ScheduleBot_misis_mendeleev_parser.Models
{
	public class ScheduleDay
	{
		public int ScheduleDayId { get; set; }
        public DateTime Date { get; set; }
		public int Day { get; set; }
		public ICollection<Lesson> Lessons { get; set; }

		public ScheduleDay()
		{
			Lessons = new List<Lesson>();
		}
	}
}
