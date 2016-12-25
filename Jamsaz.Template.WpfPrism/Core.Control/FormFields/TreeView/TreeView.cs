using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;

namespace $safeprojectname$.FormFields.TreeView
{
    public class TreeView : ContentControl
    {
        #region Part Names

        private const string PartTreeView = "PART_RadTreeView";

        #endregion

        #region Ctor

        static TreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(typeof(TreeView)));
        }

        #endregion

        #region DependencyProperties

        #region ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(object), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => { }));

        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        #endregion

        #region SelectionMode

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            "SelectionMode", typeof(SelectionMode), typeof(TreeView), new FrameworkPropertyMetadata(SelectionMode.Single, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => { }));

        public SelectionMode SelectionMode
        {
            get { return (SelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        #endregion

        #region Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(object), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => { }));

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
           var listbox = (RadTreeView)Template.FindName(PartTreeView, this);
            if (listbox != null)
            {
                listbox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ItemsSource") { Source = this, Mode = BindingMode.TwoWay });
                listbox.SetBinding(RadTreeView.SelectionModeProperty, new Binding("SelectionMode") { Source = this, Mode = BindingMode.TwoWay });
                listbox.SetBinding(RadTreeView.SelectedItemProperty, new Binding("Value") { Source = this, Mode = BindingMode.TwoWay });
            }
            base.OnApplyTemplate();
        }

        #endregion

    }
}
