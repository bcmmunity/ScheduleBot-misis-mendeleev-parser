namespace ScheduleBot_misis_mend_parsers.Models
{
	public class Group
	{
		public int GroupId { get; set; }
		public string Name { get; set; }

        public byte ScheduleType { get; set; }
        public Course Course { get; set; }
	}
}
