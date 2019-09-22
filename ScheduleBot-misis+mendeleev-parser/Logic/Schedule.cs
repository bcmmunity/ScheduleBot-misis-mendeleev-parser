using ScheduleBot_misis_mendeleev_parser.Logic.Parsers;

namespace ScheduleBot_misis_mendeleev_parser.Logic
{
    public class Schedule
    {
        public void ScheduleUpdate()
        {
            MisisParser misisParser = new MisisParser();
            misisParser.ReadXls("ИТАСУ");
            misisParser.ReadXls("ИНМИН");
            misisParser.ReadXls("МГИ");
            misisParser.ReadXls("ЭУПП");
            misisParser.ReadXls("ЭкоТех");


            MendleevParser mendleevParser = new MendleevParser();
            mendleevParser.ReadXlsx("1 course");
            mendleevParser.ReadXlsx("2 course");
            mendleevParser.ReadXlsx("3 course");
            mendleevParser.ReadXlsx("4 course");
            mendleevParser.ReadXlsx("5 course");
            mendleevParser.ReadXlsx("6 course");
            mendleevParser.ReadXlsx("7 course");

            //  misisParser.ReadXlsx("ИБО");

        }
    }
}
