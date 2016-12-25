using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;

namespace $safeprojectname$.FormFields.ListView
{
    public class ListViewItem
    {
        public ListViewItem()
        {
            Content = "";
            Value = "";
            IsSelected = false;
        }

        public ListViewItem(string content, string value, bool isSelected = false)
        {
            Content = content;
            Value = value;
            IsSelected = isSelected;
        }

        public string Content { get; set; }
        public bool IsSelected { get; set; }
        public string Value { get; set; }
        public ICommand Checked => new DelegateCommand(OnChecked);

        private void OnChecked(object obj)
        {
            var listView = obj.GetType().GetProperty("ListView").GetValue(obj, null) as RadListBox;
            var control = obj.GetType().GetProperty("Control").GetValue(obj, null);
            var source = listView?.ItemsSource as IList<ListViewItem>;
            if (control is RadioButton)
            {
                var radioButton = control as RadioButton;
                listView?.SetValue(ItemsControlSelector.SelectedValueProperty, radioButton.Tag);
                listView?.SetValue(ItemsControlSelector.SelectedItemProperty, source.FirstOrDefault(x => x.IsSelected));
            }
            else if (control is CheckBox)
            {
                listView?.SetValue(ItemsControlSelector.SelectedValueProperty, null);
                listView?.SetValue(ItemsControlSelector.SelectedItemProperty, source.Where(x => x.IsSelected));
            }
        }
    }
}
