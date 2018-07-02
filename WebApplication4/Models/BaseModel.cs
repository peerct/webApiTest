using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class BaseModel : IBaseModel
    {
        public int HasError { get; set; }

        public string ErrorMessage { get; set; }
    }
}