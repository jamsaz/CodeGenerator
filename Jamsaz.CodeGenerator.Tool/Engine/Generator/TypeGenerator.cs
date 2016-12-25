using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Models;
using Jamsaz.CodeGenerator.Tool.Presentaion.Views;
using Newtonsoft.Json.Linq;
using EditorAttribute = Telerik.Windows.Controls.Data.PropertyGrid.EditorAttribute;

namespace Jamsaz.CodeGenerator.Tool.Engine.Generator
{
    public class TypeGenerator
    {
        #region Private Variables

        private readonly AssemblyBuilder _currentAssemblyBuilder;
        private readonly JObject _pattern;

        #endregion

        #region Ctor

        /// <summary>
        /// Generate a runtime assembly and then generate types for that.
        /// </summary>
        /// <param name="assemblyName">Name of your new runtime assembly</param>
        /// <param name="pattern">Pattern you want create type for them</param>
        public TypeGenerator(string assemblyName, JObject pattern)
        {
            _pattern = pattern;
            _currentAssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName),
                 AssemblyBuilderAccess.Run);
        }

        /// <summary>
        /// Generate types for an exsiting runtime assembly.
        /// </summary>
        /// <param name="currentAssemblyBuilder">Assembly you want generateType for that</param>
        /// <param name="pattern">Patterns you want create type for them</param>
        public TypeGenerator(AssemblyBuilder currentAssemblyBuilder, JObject pattern)
        {
            _currentAssemblyBuilder = currentAssemblyBuilder;
            _pattern = pattern;
        }

        #endregion

        #region Implementing Methods

        /// <summary>
        /// Generate a type for runtime assembly and add this to that.
        /// </summary>
        /// <returns>Return type of runtime assembly that generated from json pattern</returns>
        public Type Generate()
        {
            var dynamicModule = _currentAssemblyBuilder.DefineDynamicModule("PatternRunTimeAssemblyModule");
            var rootDynamictype = dynamicModule.DefineType(_pattern["Name"].Value<string>(), TypeAttributes.Public, typeof(BaseModel));
            var propertyGenerator = new PropertyGenerator(rootDynamictype);
            var i = 0;
            foreach (var patternDefinition in _pattern["Definitaion"].Value<JObject>()["Value"].Values<JObject>())
            {
                var propertyType = patternDefinition["ValueType"] != null
                    ? Type.GetType($"System.{patternDefinition["ValueType"].Value<string>()}")
                    : typeof(object);
                var isListType = patternDefinition["ValueType"] != null &&
                                 patternDefinition["ValueType"].Value<string>() == "List";
                if (patternDefinition["Value"].GetType().Name.Contains("JArray"))
                {
                    if (patternDefinition["ValueType"] != null &&
                        patternDefinition["ValueType"].Value<string>() == "Enum")
                    {
                        propertyType = CreateEnumTypeProperties(patternDefinition, dynamicModule, ref i);
                    }
                    else
                    {
                        propertyType = CreateTypeProperties(patternDefinition, dynamicModule, ref i);
                    }
                }

                if (isListType)
                    propertyType = typeof(ObservableCollection<>).MakeGenericType(propertyType);

                var prop = propertyGenerator.AddPropertyToType(patternDefinition["Key"].Value<string>(), propertyType);
                if (patternDefinition["Describtion"] != null && patternDefinition["Describtion"].Value<string>() != string.Empty)
                    propertyGenerator.AddAttributeToProperty(ref prop, typeof(DescriptionAttribute), new[] { typeof(string) },
                        new object[] { patternDefinition["Describtion"].Value<string>() });

                if (patternDefinition["GroupName"] != null && patternDefinition["GroupName"].Value<string>() != string.Empty)
                    propertyGenerator.AddAttributeToProperty(ref prop, typeof(CategoryAttribute), new[] { typeof(string) },
                        new object[] { patternDefinition["GroupName"].Value<string>() });

                if (patternDefinition["ValueType"] != null && patternDefinition["ValueType"].Value<string>() == "MasterFormType")
                    propertyGenerator.AddAttributeToProperty(ref prop, typeof(EditorAttribute), new[] { typeof(Type), typeof(string) },
                       new object[] { typeof(MasterFormTypeComboBoxEditorControl), "SelectedItem" });
            }
            return rootDynamictype.CreateType();
        }

        /// <summary>
        /// Create instance from generated Type.
        /// </summary>
        /// <param name="type">Name of your type</param>
        /// <returns>Return object of pattern</returns>
        public object GetInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Create runtime type for each properties in pattern for each object
        /// </summary>
        /// <param name="patternDefinition">Parent object of property as json object</param>
        /// <param name="module">Runtime assembly module</param>
        /// <param name="i">Number of property in object</param>
        /// <returns>Return runtime type from object property</returns>
        private Type CreateTypeProperties(JObject patternDefinition, ModuleBuilder module, ref int i)
        {
            var value = ((JArray)patternDefinition["Value"]);
            var newDynamicType = module.DefineType(patternDefinition["Key"].Value<string>() + "_" + i + "_AnonymousType", TypeAttributes.Public, typeof(BaseModel));
            var propertyGenerator = new PropertyGenerator(newDynamicType);
            i += 1;
            foreach (var jToken in value)
            {
                var propes = (JObject)jToken;
                var propertyType = propes["ValueType"] != null
                    ? Type.GetType($"System.{propes["ValueType"].Value<string>()}")
                    : typeof(object);
                var isListType = propes["ValueType"] != null &&
                                 propes["ValueType"].Value<string>() == "List";
                if (propes["Value"].GetType().Name.Contains("JArray"))
                {
                    if (propes["ValueType"] != null && propes["ValueType"].Value<string>() == "Enum")
                    {
                        propertyType = CreateEnumTypeProperties(propes, module, ref i);
                    }
                    else if (propes["ValueType"] != null && ((propes["ValueType"].Value<string>() == "MasterFormTypes") || (propes["ValueType"].Value<string>() == "GenerateType")))
                    {
                        propertyType = Type.GetType($"Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels.{propes["ValueType"].Value<string>()}");
                    }
                    else
                    {
                        propertyType = CreateTypeProperties(propes, module, ref i);
                    }
                }

                if (isListType)
                    propertyType = typeof(ObservableCollection<>).MakeGenericType(propertyType);

                var prop = propertyGenerator.AddPropertyToType(propes["Key"].Value<string>(), propertyType);
                if (propes["Describtion"] != null && propes["Describtion"].Value<string>() != string.Empty)
                    propertyGenerator.AddAttributeToProperty(ref prop, typeof(DescriptionAttribute), new[] { typeof(string) },
                        new object[] { propes["Describtion"].Value<string>() });

                if (propes["GroupName"] != null && propes["GroupName"].Value<string>() != string.Empty)
                    propertyGenerator.AddAttributeToProperty(ref prop, typeof(CategoryAttribute), new[] { typeof(string) },
                        new object[] { propes["GroupName"].Value<string>() });

                if (propes["ValueType"] != null && propes["ValueType"].Value<string>() == "MasterFormType")
                    propertyGenerator.AddAttributeToProperty(ref prop, typeof(EditorAttribute), new[] { typeof(Type), typeof(string) },
                       new object[] { typeof(MasterFormTypeComboBoxEditorControl), "SelectedItem" });
            }
            return newDynamicType.CreateType();
        }

        /// <summary>
        /// Create runtime type for each properties in pattern for each object that is enum
        /// </summary>
        /// <param name="patternDefinition">Parent object of property as json object</param>
        /// <param name="module">Runtime assembly module</param>
        /// <param name="i">Number of property in object</param>
        /// <returns>Return runtime type from object property</returns>
        private Type CreateEnumTypeProperties(JObject patternDefinition, ModuleBuilder module, ref int i)
        {
            var value = ((JArray)patternDefinition["Value"]);
            var newDynamicType = module.DefineEnum(patternDefinition["Key"].Value<string>() + "_" + i + "_AnonymousEnumType",
                TypeAttributes.Public, typeof(int));
            i += 1;
            foreach (var jToken in value)
            {
                var propes = (JObject)jToken;
                newDynamicType.DefineLiteral(propes["Key"].Value<string>(), propes["Value"].Value<int>());
            }
            return newDynamicType.CreateType();
        }

        #endregion
    }
}
