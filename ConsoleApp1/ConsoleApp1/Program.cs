using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }

       


        static void Main(string[] args)
        {
            var httpStatusCode = 200;
            HttpWebResponse rsp=null;
        try
        { 
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create("http://localhost:62205/api/Values/3");
            rq.Method = "GET";
            SetHeaderValue(rq.Headers, "appid", "LeHealthAndroid");
            SetHeaderValue(rq.Headers, "secret", "29A813EE1021386F1159FFEBCE44224A");
            SetHeaderValue(rq.Headers, "timestamp", "20180702171408");
            /// 获取响应流
            rsp = rq.GetResponse() as HttpWebResponse; 
            httpStatusCode = (int)rsp.StatusCode;
        }
        catch (WebException ex)
        {
                rsp = ex.Response as HttpWebResponse;
                httpStatusCode = (int) rsp.StatusCode;
         }
            if (httpStatusCode == 200)
            {
                using (Stream stream = rsp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.Default);
                    string responseString = reader.ReadToEnd();
                    Console.WriteLine(responseString);
                    Console.WriteLine(httpStatusCode);
                }
            }
            else
            {
                Console.WriteLine(httpStatusCode);
            }

        }
    }
}
