using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Jamsaz.CodeGenerator.Tool.Engine.Generator
{
    public class PropertyGenerator
    {
        #region Private Variables

        private readonly TypeBuilder _currentTypeBuilder;

        #endregion

        #region Ctor

        /// <summary>
        /// Generate a property with specific type then add that to a runtime type.
        /// </summary>
        /// <param name="currentTypeBuilder">Current runtime type builder you want add property to that.</param>
        public PropertyGenerator(TypeBuilder currentTypeBuilder)
        {
            _currentTypeBuilder = currentTypeBuilder;
        }

        #endregion

        #region Implementing Methods

        /// <summary>
        /// Add property to a type
        /// </summary>
        /// <param name="propertyName">Name of property you want create it</param>
        /// <param name="propertyType">Type of property you want create it</param>
        /// <returns>Return generated property as builder</returns>
        public PropertyBuilder AddPropertyToType(string propertyName, Type propertyType)
        {
            const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;
            var field = _currentTypeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            var property = _currentTypeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType,
            new[] { propertyType });
            var getMethodBuilder = _currentTypeBuilder.DefineMethod("get_value", getSetAttr, propertyType,
            Type.EmptyTypes);
            var getIl = getMethodBuilder.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, field);
            getIl.Emit(OpCodes.Ret);
            var setMethodBuilder = _currentTypeBuilder.DefineMethod("set_value", getSetAttr, null,
            new[] { propertyType });
            var setIl = setMethodBuilder.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, field);
            setIl.Emit(OpCodes.Ret);
            property.SetGetMethod(getMethodBuilder);
            property.SetSetMethod(setMethodBuilder);
            return property;
        }

        /// <summary>
        /// Add attribute to a property
        /// </summary>
        /// <param name="property">Property which you want add attribute to that.</param>
        /// <param name="attrType">Type of attribute you want create it</param>
        /// <param name="paramTypes">Attribute parameter types</param>
        /// <param name="param">Attribute parameter values</param>
        public void AddAttributeToProperty(ref PropertyBuilder property,Type attrType, Type[] paramTypes, object[] param)
        {
            var attributeBuilder = new CustomAttributeBuilder(attrType.GetConstructor(paramTypes), param);
            property.SetCustomAttribute(attributeBuilder);
        }


        #endregion
    }
}
