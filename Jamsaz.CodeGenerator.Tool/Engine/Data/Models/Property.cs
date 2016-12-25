using System.Collections.ObjectModel;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models
{
    public class Property
    {
        public string Name { get; set; }
        public string ParentObjectNameSpace { get; set; }
        public string ParentObjectPrimaryKeyName { get; set; }
        public string ParentObjectName { get; set; }
        public string CapitalizeNameSpace { get; set; }
        public string SQLName { get; set; }
        public int Priority { get; set; }
        public string SQLDataType { get; set; }
        public string DataType { get; set; }
        public bool IsComputed { get; set; }
        public ControlType ControlType { get; set; }
        public string Check { get; set; }
        public int MaxLength { get; set; }
        public bool Lookups { get; set; }
        public bool Visible { get; set; }
        public bool ReadOnly { get; set; }
        public bool Optional { get; set; }
        public string Sort { get; set; }
        public string Default { get; set; }
        public ObservableCollection<EnumItem> EnumItems { get; set; }
        public string Attributes { get; set; }
        public object DefaultAtClient { get; set; }
        public bool Nullable { get; set; }
        public bool InTitleLocation { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public bool InGrid { get; set; }
        public bool InLookup { get; set; }
        public string DisplayName { get; set; }
        public int ColumnIndex { get; set; }
        public int ColumnSpan { get; set; }
        public int RowIndex { get; set; }
        public int RowSpan { get; set; }
    }
}
