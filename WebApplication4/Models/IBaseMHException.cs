using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    interface IBaseMHException
    {
        IBaseModel ErrorModel { get; set; }
    }
}