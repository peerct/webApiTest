using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApplication4
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //权限过滤
            config.MessageHandlers.Add(new AuthenticationHandler());
            //通用错误处理过滤器，保证错误信息都是以{HasError:1,ErrorMessage:""}的格式
            config.Filters.Add(new GeneralExceptionFilterAttribute());
            //转换POST传递过来的参数值
            config.ParameterBindingRules.Insert(0, SimplePostVariableParameterBinding.HookupParameterBinding);

            #region 格式化json日期
            System.Net.Http.Formatting.JsonMediaTypeFormatter jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            jsonFormatter.SerializerSettings.ContractResolver = new CancelNullResolver();//取消json空值
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;  //树结构无法格式化的问题
            #endregion

            //让所有的接口都支持跨域(NuGet搜索microsoft.aspnet.webapi.cors)
            GlobalConfiguration.Configuration.EnableCors(new EnableCorsAttribute("*", "*", "*"));
        }
    }
}
