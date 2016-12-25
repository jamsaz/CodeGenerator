using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Generator
{
    public class ObjectGenerator
    {
        private readonly TypeGenerator _typeGenerator;

        public ObjectGenerator(TypeGenerator typeGenerator)
        {
            _typeGenerator = typeGenerator;
        }

        public object CreateInstanceWithData(Type type, JArray data)
        {
            var obj = _typeGenerator.GetInstance(type);
            var runtimeFields = obj.GetType().GetRuntimeProperties();
            var propertyInfos = runtimeFields as PropertyInfo[] ?? runtimeFields.ToArray();
            foreach (var o in data)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
                {
                    var innerObj = Activator.CreateInstance(type.GetGenericArguments()[0]);
                    var innerObjRuntimeFields = innerObj.GetType().GetRuntimeProperties();
                    var innerObjPropertyInfos = innerObjRuntimeFields as PropertyInfo[] ?? innerObjRuntimeFields.ToArray();
                    foreach (var runtimeField in innerObjPropertyInfos)
                    {
                        if (runtimeField.PropertyType.IsEnum)
                        {
                            var val = o[runtimeField.Name]?.Value<int>();
                            if (val == null) continue;
                            var enumVal = Enum.ToObject(runtimeField.PropertyType, val);
                            runtimeField.SetValue(innerObj, enumVal);
                        }
                        else if (runtimeField.PropertyType.Namespace == null || !runtimeField.PropertyType.Namespace.Equals("System"))
                        {
                            if (!o[runtimeField.Name].GetType().Name.Contains("JArray"))
                            {
                                var arrayData = new JArray(o[runtimeField.Name]);
                                runtimeField.SetValue(innerObj,
                                    CreateInstanceWithData(runtimeField.PropertyType, arrayData));
                            }
                            else
                            {
                                runtimeField.SetValue(innerObj,
                                    CreateInstanceWithData(runtimeField.PropertyType, o[runtimeField.Name].Value<JArray>()));
                            }
                        }
                        else
                        {
                            var val = o[runtimeField.Name]?.Value<object>();
                            if (val != null && o[runtimeField.Name].Type.ToString() != "Null")
                                runtimeField.SetValue(innerObj, Convert.ChangeType(val, runtimeField.PropertyType));
                        }
                    }
                    obj.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, obj, new[] { innerObj });
                }
                else
                {
                    foreach (var runtimeField in propertyInfos)
                    {
                        if (runtimeField.PropertyType.IsEnum)
                        {
                            var val = o[runtimeField.Name]?.Value<int>();
                            if (val == null) continue;
                            var enumVal = Enum.ToObject(runtimeField.PropertyType, val);
                            runtimeField.SetValue(obj, enumVal);
                        }
                        else if (runtimeField.PropertyType.Namespace == null || !runtimeField.PropertyType.Namespace.Equals("System"))
                        {
                            runtimeField.SetValue(obj,
                                CreateInstanceWithData(runtimeField.PropertyType, o[runtimeField.Name].Value<JArray>()));
                        }
                        else
                        {
                            var val = o[runtimeField.Name]?.Value<object>();
                            if (val != null && o[runtimeField.Name].Type.ToString() != "Null")
                                runtimeField.SetValue(obj, Convert.ChangeType(val, runtimeField.PropertyType));
                        }
                    }
                }
            }
            return obj;
        }

        public object CreateInstance(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
            {
                var outobj = Activator.CreateInstance(type);
                var innerObj = Activator.CreateInstance(type.GetGenericArguments()[0]);
                var innerObjRuntimeFields = innerObj.GetType().GetRuntimeProperties();
                var innerObjPropertyInfos = innerObjRuntimeFields as PropertyInfo[] ?? innerObjRuntimeFields.ToArray();
                foreach (var runtimeField in innerObjPropertyInfos)
                {
                    if (runtimeField.PropertyType.IsEnum)
                    {
                        var val = GetDefaultValue(runtimeField.PropertyType);
                        if (val == null) continue;
                        var enumVal = Enum.ToObject(runtimeField.PropertyType, val);
                        runtimeField.SetValue(innerObj, enumVal);
                    }
                    else if (runtimeField.PropertyType.Namespace == null ||
                        !runtimeField.PropertyType.Namespace.Equals("System"))
                    {
                        runtimeField.SetValue(innerObj,
                            CreateInstance(runtimeField.PropertyType));
                    }
                    else
                    {
                        var val = GetDefaultValue(runtimeField.PropertyType);
                        if (val != null && runtimeField.SetMethod != null)
                            runtimeField.SetValue(innerObj, Convert.ChangeType(val, runtimeField.PropertyType));
                    }
                }
                outobj.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, outobj, new[] { innerObj });
                return outobj;
            }
            var obj = Activator.CreateInstance(type);
            var runtimeFields = obj.GetType().GetRuntimeProperties();
            var propertyInfos = runtimeFields as PropertyInfo[] ?? runtimeFields.ToArray();
            foreach (var runtimeField in propertyInfos)
            {
                if (runtimeField.PropertyType.IsEnum)
                {
                    var val = GetDefaultValue(runtimeField.PropertyType);
                    if (val == null) continue;
                    var enumVal = Enum.ToObject(runtimeField.PropertyType, val);
                    runtimeField.SetValue(obj, enumVal);
                }
                else if (runtimeField.PropertyType.Namespace == null ||
                    !runtimeField.PropertyType.Namespace.Equals("System"))
                {
                    runtimeField.SetValue(obj,
                        CreateInstance(runtimeField.PropertyType));
                }
                else
                {
                    var val = GetDefaultValue(runtimeField.PropertyType);
                    if (val != null && runtimeField.SetMethod != null)
                        runtimeField.SetValue(obj, Convert.ChangeType(val, runtimeField.PropertyType));
                }
            }

            return obj;
        }

        private static object GetDefaultValue(Type t)
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }
    }
}
