namespace Jamsaz.CodeGenerator.Tool.Engine.Data.DataConverters
{
    public interface IDataTypeConverter
    {
        string GetCSharpDataType(string type);
    }
}