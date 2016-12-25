using System.Collections.Generic;

namespace $safeprojectname$.FormFields.TreeView
{
    public class TreeViewItem
    {
        public string Content { get; set; }
        public string Value { get; set; }
        public IEnumerable<TreeViewItem> Children { get; set; }
    }
}
