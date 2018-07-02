using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApplication4
{
    /// <summary>
    /// 处理string空值
    /// </summary>
    public class NullableValueProvider : IValueProvider
    {
        private readonly object _defaultValue;
        private readonly IValueProvider _underlyingValueProvider;
        public NullableValueProvider(MemberInfo memberInfo, Type underlyingType)
        {
            _underlyingValueProvider = new DynamicValueProvider(memberInfo);
            _defaultValue = Activator.CreateInstance(underlyingType);
        }
        public object GetValue(object target)
        {
            return _underlyingValueProvider.GetValue(target) ?? _defaultValue;
        }

        public void SetValue(object target, object value)
        {
            _underlyingValueProvider.SetValue(target, value);
        }
    }
}