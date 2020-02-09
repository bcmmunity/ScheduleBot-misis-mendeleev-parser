using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Text;
using Newtonsoft.Json;
using ScheduleBot_misis_mendeleev_parser.Models;

namespace ScheduleBot_misis_mendeleev_parser.Logic.Request
{
    public class ScheduleRequest
    {

        //public void AddUniversity(string name)
        //{
        //    PostRequest request = new PostRequest
        //    {
        //        University = name,

        //    };
        //    Send(request);
            
        //}

        //public void AddFacility(string university, string name)
        //{
        //    PostRequest request = new PostRequest
        //    {
        //        University = university,
        //        Facility = name,

        //    };
        //    Send(request);
         
        //}

        //public void AddCourse(string university, string facility, string name)
        //{
        //    PostRequest request = new PostRequest
        //    {
        //        University = university,
        //        Facility = facility,
        //        Course = name,
        //    };
        //    Send(request);
           
        //}

        //public void AddGroup(string university, string facility, string course, string name, byte type)
        //{
        //    PostRequest request = new PostRequest
        //    {
        //        University = university,
        //        Facility = facility,
        //        Course = course,
        //        Group = name,
        //        Type = type

        //    };
        //    Send(request);
           
        //}



        public void AddScheduleWeek(string university, string facility, string course, string group, byte type, ScheduleWeek week)
        {
            List<ScheduleWeek> weeks = new List<ScheduleWeek>();
            weeks.Add(week);
            PostRequest request = new PostRequest
            {
                Key = "76546b5ay2604648egh0dlbg1a0c44067k8dd167a0je2920b3c24f8e657effb9850eb13d4978c0da58959",
                University = university,
                Facility = facility,
                Course = course,
                Group = group,
                Weeks = weeks,
                Type = type
            };
            Send(request);

        }
        private void Send(object obj)
        {
            const string url = "http://dev.studystat.ru/api/schedule";

            string json = JsonConvert.SerializeObject(obj);

            var body = Encoding.UTF8.GetBytes(json);
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = body.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
                stream.Close();
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    response.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(json);
            }
            

        }
    }
}