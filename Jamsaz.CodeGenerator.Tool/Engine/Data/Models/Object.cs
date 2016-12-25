using System.Collections.Generic;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models
{
    public class Object
    {
        public int Id { get; set; }
        public string PrimaryKey { get; set; }
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string Title { get; set; }
        public string DisplayName { get; set; }
        public bool Add { get; set; }
        public string AddButtonText { get; set; }
        public bool Edit { get; set; }
        public string EditButtonText { get; set; }
        public bool Delete { get; set; }
        public string DeleteButtonText { get; set; }
        public string SaveButtonText { get; set; }
        public string CancelButtonText { get; set; }
        public string DeleteMessageText { get; set; }
        public string DeleteMessageTitle { get; set; }
        public string GridRowDeletedMessage { get; set; }
        public string SaveMessageText { get; set; }
        public string SaveMessageTitle { get; set; }
        public string GridRowNotSelectedMessage { get; set; }
        public bool InlineEditing { get; set; }
        public IEnumerable<Property> Properties { get; set; }
        public IEnumerable<object> Relations { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public bool Visible { get; set; }
        public bool VisibleForm { get; set; }
        public int ColumnOfForm { get; set; }
        public int RowOfForm { get; set; }
        public LayoutTypes Layout { get; set; }
        public bool MasterForm { get; set; }
        public IEnumerable<MasterFormType> MasterFormTypes { get; set; }
        public int ParentModule { get; set; }
    }

}
