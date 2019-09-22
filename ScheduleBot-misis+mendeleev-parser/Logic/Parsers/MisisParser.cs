using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ScheduleBot_misis_mendeleev_parser.Logic.Request;
using ScheduleBot_misis_mendeleev_parser.Models;

namespace ScheduleBot_misis_mendeleev_parser.Logic.Parsers
{
    public class MisisParser
    {
        private readonly ScheduleRequest Schedule = new ScheduleRequest();
        
        public void ReadXls(string fileName)
        {
            //ScheduleController.CheckFile();

            Schedule.AddUniversity("НИТУ МИСиС");
            Schedule.AddFacility("НИТУ МИСиС", fileName);

            HSSFWorkbook hssfwb;
            using (FileStream file = new FileStream(@"Schedule Files\Misis\" + fileName + ".xls", FileMode.Open, FileAccess.Read))
            {
                hssfwb = new HSSFWorkbook(file);
            }


            for (int course = 1; course < 9; course++)
            {

                if (hssfwb.GetSheet(course + " курс") == null)
                    break;

                Schedule.AddCourse("НИТУ МИСиС", fileName, course.ToString());

                ISheet sheet = hssfwb.GetSheet(course + " курс");

                int group = 4;
                //int myFlag = 0;

                while (sheet.GetRow(1).GetCell(group - 1) != null)
                {

                    if (sheet.GetRow(1).GetCell(group - 1).NumericCellValue.ToString() == "1")
                    {
                        Schedule.AddGroup("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 1 подгруппа",2);
                    }
                    else if (sheet.GetRow(1).GetCell(group - 1).NumericCellValue.ToString() == "2")
                    {
                        if (sheet.GetRow(0).GetCell(group - 1).ToString() == "")
                            Schedule.AddGroup("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 3).StringCellValue + " 2 подгруппа",2);
                        else
                            Schedule.AddGroup("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 2 подгруппа",2);
                    }
                    else if (sheet.GetRow(0).GetCell(group - 1).StringCellValue != "")
                    {
                        Schedule.AddGroup("НИТУ МИСиС", fileName, course.ToString(),
                            sheet.GetRow(0).GetCell(group - 1).StringCellValue,2);
                    }
                    else
                    {
                        group += 2;
                        continue;
                    }
                        

                    ScheduleWeek week1 = new ScheduleWeek();
                    ScheduleWeek week2 = new ScheduleWeek();

                    week1.Week = 1;
                    week1.Day = new List<ScheduleDay>();
    

                    week2.Week = 2;
                    week2.Day = new List<ScheduleDay>();


                    for (int dayofweek = 3; dayofweek < 100; dayofweek += 14)
                    {
                        ScheduleDay day1 = new ScheduleDay();
                        ScheduleDay day2 = new ScheduleDay();

                        day1.Day = dayofweek / 14 + 1;
                        day1.Lesson = new List<Lesson>();
                        day2.Day = dayofweek / 14 + 1;
                        day2.Lesson = new List<Lesson>();

                        for (int para = dayofweek; para < dayofweek + 14; para += 2)
                        {

                            if (sheet.GetRow(para - 1).GetCell(group - 1) != null &&
                                sheet.GetRow(para - 1).GetCell(group - 1).StringCellValue != "")
                            {


                                Lesson a = new Lesson()
                                {
                                    Name = GetName(sheet.GetRow(para - 1).GetCell(group - 1).StringCellValue),
                                    Time = sheet.GetRow(para - 1).GetCell(2).StringCellValue,
                                    Room = sheet.GetRow(para - 1).GetCell(group).StringCellValue,
                                    Teacher = GetTeacher(sheet.GetRow(para - 1).GetCell(group - 1).StringCellValue),
                                    Type = GetType(sheet.GetRow(para - 1).GetCell(group - 1).StringCellValue),
                                    Number = ((para - dayofweek) / 2 + 1).ToString()
                                };
                                day1.Lesson.Add(a);
                            }

                            if (sheet.GetRow(para).GetCell(group - 1) != null &&
                                sheet.GetRow(para).GetCell(group - 1).StringCellValue != "")
                            {

                                day2.Lesson.Add(new Lesson()
                                {
                                    Name = GetName(sheet.GetRow(para).GetCell(group - 1).StringCellValue),
                                    Time = sheet.GetRow(para - 1).GetCell(2).StringCellValue,
                                    Room = sheet.GetRow(para).GetCell(group).StringCellValue,
                                    Teacher = GetTeacher(sheet.GetRow(para).GetCell(group - 1).StringCellValue),
                                    Type = GetType(sheet.GetRow(para).GetCell(group - 1).StringCellValue),
                                    Number = ((para - dayofweek) / 2 + 1).ToString()
                                });
                            }
                        }

                        week1.Day.Add(day1);
                        week2.Day.Add(day2);

                    }

                    if (sheet.GetRow(1).GetCell(group - 1).NumericCellValue.ToString() == "1")
                    {
                        Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 1 подгруппа", week1);
                        Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 1 подгруппа", week2);
                    }
                    else if (sheet.GetRow(1).GetCell(group - 1).NumericCellValue.ToString() == "2")
                    {
                        if (sheet.GetRow(0).GetCell(group - 1).ToString() == "")
                        {
                            Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 3).StringCellValue + " 2 подгруппа", week1);
                            Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 3).StringCellValue + " 2 подгруппа", week2);
                        }
                        else
                        {
                            Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 2 подгруппа", week1);
                            Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 2 подгруппа", week2);
                        }
                    }
                    else
                    {
                        Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue, week1);
                        Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue, week2);
                    }

                    group += 2;
                }
            }
        }

        public void ReadXlsx(string fileName)
        {
            //schedule.CheckFile();

            Schedule.AddUniversity("НИТУ МИСиС");
            Schedule.AddFacility("НИТУ МИСиС", fileName);


            XSSFWorkbook hssfwb;
            using (FileStream file = new FileStream(@"Schedule Files\Misis\" + fileName + ".xlsx", FileMode.Open, FileAccess.Read))
            {
                hssfwb = new XSSFWorkbook(file);
            }

            for (int course = 1; course < 9; course++)
            {

                if (hssfwb.GetSheet(course + " курс") == null)
                    break;
                Schedule.AddCourse("НИТУ МИСиС", fileName, course.ToString());

                ISheet sheet = hssfwb.GetSheet(course + " курс");

                int group = 4;
                //int myFlag = 0;

                while (sheet.GetRow(1).GetCell(group - 1) != null)
                {

                    if (sheet.GetRow(1).GetCell(group - 1).NumericCellValue.ToString() == "1")
                    {
                        Schedule.AddGroup("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 1 подгруппа",2);
                    }
                    else if (sheet.GetRow(1).GetCell(group - 1).NumericCellValue.ToString() == "2")
                    {
                        if (sheet.GetRow(0).GetCell(group - 1).ToString() == "")
                            Schedule.AddGroup("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 3).StringCellValue + " 2 подгруппа",2);
                        else
                            Schedule.AddGroup("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 2 подгруппа",2);
                    }
                    else
                    {
                        Schedule.AddGroup("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue,2);
                    }

                    ScheduleWeek week1 = new ScheduleWeek();
                    ScheduleWeek week2 = new ScheduleWeek();

                    week1.Week = 1;
                    week1.Day = new List<ScheduleDay>();
                    

                    week2.Week = 2;
                    week2.Day = new List<ScheduleDay>();
                 

                    for (int dayofweek = 3; dayofweek < 100; dayofweek += 14)
                    {
                        ScheduleDay day1 = new ScheduleDay();
                        ScheduleDay day2 = new ScheduleDay();

                        day1.Day = dayofweek / 14 + 1;
                        day1.Lesson = new List<Lesson>();
                        day2.Day = dayofweek / 14 + 1;
                        day2.Lesson = new List<Lesson>();



                        for (int para = dayofweek; para < dayofweek + 14; para += 2)
                        {
                            if (sheet.GetRow(para - 1).GetCell(group - 1) != null)
                                if (sheet.GetRow(para - 1).GetCell(group - 1).StringCellValue != "")
                                {
                                    try
                                    {
                                        if (sheet.GetRow(para - 1).GetCell(group - 1).StringCellValue=="")
                                            continue;
                                        Lesson a = new Lesson() { Name = GetName(sheet.GetRow(para - 1).GetCell(group - 1).StringCellValue), Time = sheet.GetRow(para - 1).GetCell(2).StringCellValue, Room = sheet.GetRow(para - 1).GetCell(group).StringCellValue, Teacher = GetTeacher(sheet.GetRow(para - 1).GetCell(group - 1).StringCellValue), Type = GetType(sheet.GetRow(para - 1).GetCell(group - 1).StringCellValue), Number = ((para - dayofweek) / 2 + 1).ToString() };
                                        day1.Lesson.Add(a);
                                    }
                                    catch
                                    {
                                        continue;
                                    }

                                }

                            if (sheet.GetRow(para).GetCell(group - 1) != null)
                                if (sheet.GetRow(para).GetCell(group - 1).StringCellValue != "")
                                    day2.Lesson.Add(new Lesson() { Name = sheet.GetRow(para).GetCell(group - 1).StringCellValue, Time = sheet.GetRow(para - 1).GetCell(2).StringCellValue, Room = sheet.GetRow(para).GetCell(group).StringCellValue, Teacher = GetTeacher(sheet.GetRow(para).GetCell(group - 1).StringCellValue), Type = GetType(sheet.GetRow(para).GetCell(group - 1).StringCellValue), Number = ((para - dayofweek) / 2 + 1).ToString() });
                        }

                        week1.Day.Add(day1);
                        week2.Day.Add(day2);

                    }

                    if (sheet.GetRow(1).GetCell(group - 1).NumericCellValue.ToString() == "1")
                    {
                        Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 1 подгруппа", week1);
                        Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 1 подгруппа", week2);
                    }
                    else if (sheet.GetRow(1).GetCell(group - 1).NumericCellValue.ToString() == "2")
                    {
                        if (sheet.GetRow(0).GetCell(group - 1).ToString() == "")
                        {
                            Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 3).StringCellValue + " 2 подгруппа", week1);
                            Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 3).StringCellValue + " 2 подгруппа", week2);
                        }
                        else
                        {
                            Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 2 подгруппа", week1);
                            Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue + " 2 подгруппа", week2);
                        }
                    }
                    else
                    {
                        Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue, week1);
                        Schedule.AddScheduleWeek("НИТУ МИСиС", fileName, course.ToString(), sheet.GetRow(0).GetCell(group - 1).StringCellValue, week2);
                    }

                    group += 2;
                }
            }
        }

        private Teacher GetTeacher(string cell)
        {
        Regex teacherRegex = new Regex(@"[А-Я][а-я]+ [А-Я]\. ?[А-Я]\.");

        MatchCollection teachersMatches =
            teacherRegex.Matches(cell);
                                

        if (teachersMatches.Count > 0)
        {
            return new Teacher{Name = teachersMatches[0].Value};
        }
        
        return null;

        
        }

        private string GetType(string cell)
        {
        Regex typeRegex = new Regex(@"\([А-Я][а-я]*\)");
MatchCollection typeMatches =
                typeRegex.Matches(cell);

            if (typeMatches.Count > 0)
            {
                return typeMatches[0].Value;
            }
            else
            {
                return "";
            }
        }
        private string GetName(string cell)
        {
            Regex nameRegex = new Regex(@"[^(]*");
            MatchCollection nameMatches =
                nameRegex.Matches(cell);

            if (nameMatches.Count > 0)
            {
                return nameMatches[0].Value;
            }
            else
            {
                return "";
            }
        }
        
    }

}
