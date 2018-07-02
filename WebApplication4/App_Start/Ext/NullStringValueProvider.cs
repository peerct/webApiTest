using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApplication4
{
    /// <summary>
    /// string类型的空值
    /// </summary>
    public class NullStringValueProvider : IValueProvider
    {
        private readonly IValueProvider _underlyingValueProvider;
        public NullStringValueProvider(MemberInfo memberInfo)
        {
            _underlyingValueProvider = new DynamicValueProvider(memberInfo);
        }
        public object GetValue(object target)
        {
            return _underlyingValueProvider.GetValue(target) ?? string.Empty;
        }

        public void SetValue(object target, object value)
        {
            _underlyingValueProvider.SetValue(target, value);
        }
    }
}