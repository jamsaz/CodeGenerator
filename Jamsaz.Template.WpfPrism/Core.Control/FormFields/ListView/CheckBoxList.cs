using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace $safeprojectname$.FormFields.ListView
{
    public class CheckBoxList : RadListBox
    {
        #region Ctor

        public CheckBoxList()
        {
            ItemTemplate = CreateRectangleDataTemplate();
        }

        #endregion

        #region DataTemplates

        public static DataTemplate CreateRectangleDataTemplate()
        {
            var checkBoxFactory = new FrameworkElementFactory(typeof(CheckBox));
            checkBoxFactory.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsSelected"));
            checkBoxFactory.SetBinding(ContentControl.ContentProperty, new Binding("Content"));
            return new DataTemplate
            {
                VisualTree = checkBoxFactory,
            };
        }

        #endregion
    }
}
