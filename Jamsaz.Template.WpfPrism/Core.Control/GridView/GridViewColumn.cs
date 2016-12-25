using System.Collections;
using System.Windows.Data;

namespace $safeprojectname$.GridView
{
    public class GridViewColumn
    {
        public GridViewColumn()
        {
            HeadText = "";
            BindingPath = "";
            ColumnType = GridViewColumnTypes.DataColumn;
            Visible = true;
        }

        public GridViewColumn(string columnHeadText, string bindingPath, GridViewColumnTypes type = GridViewColumnTypes.DataColumn)
        {
            HeadText = columnHeadText;
            BindingPath = bindingPath;
            ColumnType = type;
            Visible = true;
        }
        public string HeadText { get; set; }
        public string BindingPath { get; set; }
        public GridViewColumnTypes ColumnType { get; set; }
        public string ItemSourceBindingPath { get; set; }
        public string SelectedValueMemberPath { get; set; }
        public string DisplayMemberPath { get; set; }
        public bool Visible { get; set; }
    }
}
