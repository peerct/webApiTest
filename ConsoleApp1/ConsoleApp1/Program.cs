using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {

        #region HttpWebRequest
        //组织requset的header
        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
        public static void ExecuteRequet()
        {
            var httpStatusCode = 200;
            HttpWebResponse rsp = null;
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
                httpStatusCode = (int)rsp.StatusCode;
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
        #endregion
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); // 开始监视代码运行时间
            var client = new RestClient("http://localhost:62205");
            var request = new RestRequest("api/Values/{id}", Method.GET);
            // replaces matching token in request.Resource
            request.AddUrlSegment("id", "123"); 
            // add HTTP Headers
            request.AddHeader("appid", "LeHealthAndroid");
            request.AddHeader("secret", "29A813EE1021386F1159FFEBCE44224A");
            request.AddHeader("timestamp", "20180702171408");
            // 1.raw content as string
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            Console.WriteLine(content);
            //2. easy async support
            //client.ExecuteAsync(request, response => {
            //    Console.WriteLine(response.Content);
            //});

            stopwatch.Stop(); // 停止监视
            TimeSpan timespan = stopwatch.Elapsed; // 获取当前实例测量得出的总时间
            string milliseconds = timespan.TotalMilliseconds.ToString("#0.00000000 "); // 总毫秒数
            Console.WriteLine(string.Format("执行总毫秒数{0}", milliseconds));
            string hours = timespan.TotalHours.ToString("#0.00000000 "); // 总小时
            string minutes = timespan.TotalMinutes.ToString("#0.00000000 "); // 总分钟
            string seconds = timespan.TotalSeconds.ToString("#0.00000000 "); // 总秒数
            
            Console.Read();

        }
    }
}
