using System.Data.Entity;
using System.IO;
using System.Linq;
using Jamsaz.CodeGenerator.Tool.Engine.Data.DataConverters;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Global;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Concrete
{
    public class SqlServerDataProvider : IReadableDataProvider<IQueryable<Models.Data>>, IDataTypeConverter, IDataProvider
    {
        private readonly IConfigurationManager _configurationManager;

        public SqlServerDataProvider(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public bool HasCash { get; set; }

        public IQueryable<Models.Data> GetDatas()
        {
            var json = _configurationManager.ConfigProvider.GetDatas();
            var queryString = string.IsNullOrEmpty(json["Project"].Value<JObject>("Data").Value<string>("UserName"))
                ? $"Data Source={json["Project"].Value<JObject>("Data").Value<string>("Path")};Initial Catalog={json["Project"].Value<JObject>("Data").Value<string>("Name")};Integrated Security=True"
                : $"Data Source={json["Project"].Value<JObject>("Data").Value<string>("Path")};Initial Catalog={json["Project"].Value<JObject>("Data").Value<string>("Name")};Persist Security Info=True;User ID={json["Project"].Value<JObject>("Data").Value<string>("UserName")};Password={json["Project"].Value<JObject>("Data").Value<string>("Password")}";
            var db = new DbContext(queryString);
            return db.Database.SqlQuery<Models.Data>(File.ReadAllText(_configurationManager.ConfigurationFilePath + json["Project"].Value<JObject>("Data").Value<string>("Query"))).Select(x => new Models.Data
            {
                ColumnId = x.ColumnId,
                ColumnName = x.ColumnName,
                DataType = GetCSharpDataType(x.DataType),
                Default = x.Default,
                ForeignKeyName = x.ForeignKeyName,
                ForeignKeyParentSchema = x.ForeignKeyParentSchema,
                ForeignKeyRefrenceColumn = x.ForeignKeyRefrenceColumn,
                ForeignKeyRefrenceSchema = x.ForeignKeyRefrenceSchema,
                ForeignKeyRefrenceTable = x.ForeignKeyRefrenceTable,
                ForeignKeyRefrenceTableId = x.ForeignKeyRefrenceTableId,
                IsComputed = x.IsComputed,
                IsForeignKey = x.IsForeignKey,
                MaxLength = x.MaxLength,
                Nullable = x.Nullable,
                PrimaryKeyName = x.PrimaryKeyName,
                SchemaId = x.SchemaId,
                SchemaName = x.SchemaName,
                TableId = x.TableId,
                TableName = x.TableName
            }).AsQueryable();
        }

        public string GetCSharpDataType(string type)
        {
            switch (type.ToLower())
            {
                case "bigint":
                    return "long";

                case "binary":
                case "image":
                case "timestamp":
                case "varbinary":
                    return "byte[]";

                case "bit":
                    return "bool";

                case "char":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "text":
                case "varchar":
                case "xml":
                    return "string";

                case "datetime":
                case "smalldatetime":
                case "date":
                case "time":
                case "datetime2":
                    return "DateTime";

                case "decimal":
                case "money":
                case "smallmoney":
                    return "decimal";

                case "float":
                    return "double";

                case "int":
                    return "int";

                case "real":
                    return "float";

                case "uniqueidentifier":
                    return "Guid";

                case "smallint":
                    return "short";

                case "tinyint":
                    return "byte";

                case "variant":
                case "udt":
                    return "object";

                case "structured":
                    return "DataTable";

                case "datetimeoffset":
                    return "DateTimeOffset";

                default:
                    return "";
            }
        }
    }
}