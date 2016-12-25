using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Global.Helpers
{
    public static class ReflectionHelper
    {
        public static void SetPropertyValue(this object source, string propertyName, object value)
        {
            source.GetType().GetProperty(propertyName).SetValue(source, value);
        }

        public static object GetPropertyValueAsObject(this object source, string propertyName)
        {
            if (!(source is JObject) && !(source is JToken))
                return source.GetType().GetProperty(propertyName).GetValue(source, null);
            var obj = (JToken)source;
            return obj[propertyName].Value<JObject>();
        }

        public static T GetPropertyValueAs<T>(this object source, string propertyName)
        {
            if (!(source is JObject) && !(source is JToken))
                return (T) source.GetType().GetProperty(propertyName).GetValue(source, null);
            var obj = (JToken) source;
            if (typeof (T).IsEnum)
            {
                return (T) Enum.ToObject(typeof (T), obj[propertyName].Value<int>());
            }
            return obj[propertyName].Value<T>();
        }

        public static string GetPropertyValueAsString(this object source, string propertyName)
        {
            if (!(source is JObject) && !(source is JToken))
                return source.GetType().GetProperty(propertyName).GetValue(source, null).ToString();
            var obj = (JToken)source;
            return obj[propertyName].Value<string>();
        }
        
        public static IEnumerable<object> GetPropertyValueAsEnumerate(this object source, string propertyName)
        {
            if (!(source is JObject) && !(source is JToken))
                return (IEnumerable<object>)source.GetType().GetProperty(propertyName).GetValue(source, null);
            var obj = (JToken)source;
            var objects = obj[propertyName].Value<JArray>();
            return objects;
        }
    }
}
