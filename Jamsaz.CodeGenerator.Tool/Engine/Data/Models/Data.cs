namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models
{
    public class Data
    {
        public string PrimaryKeyName { get; set; }
        public int SchemaId { get; set; }
        public string SchemaName { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; }
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public bool IsComputed { get; set; }
        public bool Nullable { get; set; }
        public short MaxLength { get; set; }
        public string DataType { get; set; }
        public bool IsForeignKey { get; set; }
        public string ForeignKeyRefrenceTable { get; set; }
        public int ForeignKeyRefrenceTableId { get; set; }
        public string ForeignKeyRefrenceColumn { get; set; }
        public string ForeignKeyName { get; set; }
        public string Default { get; set; }
        public string ForeignKeyRefrenceSchema { get; set; }
        public string ForeignKeyParentSchema { get; set; }
    }
}
