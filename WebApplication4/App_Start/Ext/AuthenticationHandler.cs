using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace WebApplication4
{
    /// <summary>
    /// 权限过滤
    /// appid secret timestamp
    /// </summary>
    public class AuthenticationHandler : System.Net.Http.DelegatingHandler
    {
        /// <summary>
        /// 获取所有AppIDSecret
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static IDictionary<string, string> GetAllAppIDSecret(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode root = doc.DocumentElement;
            //获取节点列表
            XmlNodeList items = root.ChildNodes;
            IDictionary<string, string> dic = new Dictionary<string, string>();
            foreach (XmlNode item in items)
            {
                var AppID = item.Attributes["AppID"].InnerText;
                var Secret = item.Attributes["Secret"].InnerText;
                if (!string.IsNullOrWhiteSpace(AppID) && !string.IsNullOrWhiteSpace(Secret) && !dic.ContainsKey(AppID))
                    dic.Add(AppID, Secret);
            }
            dic.Add("", "");//从数据库中读取
            return dic;
        }

        /// <summary>
        /// appkey和secret的数据字典
        /// </summary>
        public static IDictionary<string, string> _AppIDSecret
        {
            get
            {
                IDictionary<string, string> dic = FileCacheHelper.GetCache<IDictionary<string, string>>("_AppIDSecret");
                if (dic == null)
                {
                    try
                    {
                        var path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath), "App_Data\\AppIDSecret.xml");
                        dic = GetAllAppIDSecret(path);
                        if (dic == null)
                            dic = new Dictionary<string, string>();
                        FileCacheHelper.SetCache("_AppIDSecret", dic, path);
                    }
                    catch
                    {
                        dic = new Dictionary<string, string>();
                    }
                }
                return dic;
            }
        }



        public string FindSecret(string appID)
        {
            string sec = string.Empty;
            _AppIDSecret.TryGetValue(appID, out sec);
            return sec;
        }
        public static string GetSwcSH1(string value)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("x2").ToUpperInvariant();
            }
            return sh1;
        }

        public static string GetSwcMD5(string value)
        {
            MD5 algorithm = MD5.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            string md5Str = "";
            for (int i = 0; i < data.Length; i++)
            {
                md5Str += data[i].ToString("x2").ToUpperInvariant();
            }
            return md5Str;
        }
        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<string> AppIDs;
                if (!request.Headers.TryGetValues("appid", out AppIDs))
                    return base.SendAsync(request, cancellationToken);
                IEnumerable<string> Secrets;
                if (!request.Headers.TryGetValues("secret", out Secrets))
                    return base.SendAsync(request, cancellationToken);
                IEnumerable<string> Timestamps;
                if (!request.Headers.TryGetValues("timestamp", out Timestamps))//20150528093456
                    return base.SendAsync(request, cancellationToken);
                if (AppIDs == null || Secrets == null || Timestamps == null || string.IsNullOrWhiteSpace(AppIDs.FirstOrDefault()) || string.IsNullOrWhiteSpace(Secrets.FirstOrDefault()) || string.IsNullOrWhiteSpace(Timestamps.FirstOrDefault()))
                    return base.SendAsync(request, cancellationToken);
                DateTime dt;
                if (!DateTime.TryParseExact(Timestamps.FirstOrDefault(), "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out dt))
                    return base.SendAsync(request, cancellationToken);
                var ExpiredTime = 600; //ConfigurationManager.AppSettings["ExpiredTime"].ToString();//权限过期时间
                if (Math.Abs((DateTime.Now - dt).TotalMinutes) > Convert.ToInt32(ExpiredTime))//前后时间相差不能超过15分钟
                    return base.SendAsync(request, cancellationToken);
                string appid = AppIDs.FirstOrDefault();
                string sec = FindSecret(appid);//获取明文secret
                if (string.IsNullOrWhiteSpace(sec))
                    return base.SendAsync(request, cancellationToken);
                string secret = string.Format("{0}{1}{2}", appid, sec, Timestamps.FirstOrDefault());
                secret = GetSwcMD5(secret);//md5加密（大写）
                if (secret != Secrets.FirstOrDefault())
                    return base.SendAsync(request, cancellationToken);
                if (!string.IsNullOrWhiteSpace(appid))//set user
                {
                    IPrincipal principalIns= new GenericPrincipal(new GenericIdentity(appid), null);
                    SetPrincipal(principalIns);
                }
                Boolean flag = HttpContext.Current.User.Identity.IsAuthenticated;
                return base.SendAsync(request, cancellationToken);
            }
            catch
            {
                return base.SendAsync(request, cancellationToken);
            }
        }

    }
}