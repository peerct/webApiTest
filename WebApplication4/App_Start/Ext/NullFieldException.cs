using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4
{
    /// <summary>
    /// 字段空异常--这是我们不愿发生的错误！
    /// </summary>
    public class NullFieldException : Exception, Models.IBaseMHException
    {
        public NullFieldException(string message)
        {
            this.ErrorModel = new Models.BaseModel() { HasError = 1, ErrorMessage = message };
        }

        public Models.IBaseModel ErrorModel { get; set; }
    }
}