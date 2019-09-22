using System;
using ScheduleBot_misis_mendeleev_parser.Logic;

namespace ScheduleBot_misis_mendeleev_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("In progress...");
            new Schedule().ScheduleUpdate();
            Console.WriteLine("Done!");
        }
    }
}
