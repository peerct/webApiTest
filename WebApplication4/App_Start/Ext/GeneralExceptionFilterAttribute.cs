using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace WebApplication4
{
    /// <summary>
    /// 对所有异常进行统一处理
    /// </summary>
    public class GeneralExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常发生时触发的事件，统一返回state=200的错误信息
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is Models.IBaseMHException)//已知错误
            {
                var model = context.Exception as Models.IBaseMHException;
                context.Response = context.Request.CreateResponse(HttpStatusCode.OK, model.ErrorModel);
            }
            else
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.OK, new { HasError = 1, ErrorMessage = context.Exception.Message });
            }
        }
    }
}