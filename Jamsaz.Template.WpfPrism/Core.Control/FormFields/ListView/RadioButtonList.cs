using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;

namespace $safeprojectname$.FormFields.ListView
{
    public class RadioButtonList : ContentControl
    {
        #region Part Names

        private const string PartListbox = "PART_RadListBox";

        #endregion

        #region Ctor

        static RadioButtonList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonList), new FrameworkPropertyMetadata(typeof(RadioButtonList)));
        }

        #endregion

        #region DependencyProperties

        #region SelectedValue

        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
            "SelectedValue", typeof(object), typeof(RadioButtonList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) =>
            {

            }));

        public object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        #endregion

        #region SelectedItem

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(RadioButtonList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) =>
            {

            }));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        #endregion

        #region SelectedValuePath

        public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register(
            "SelectedValuePath", typeof(object), typeof(RadioButtonList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => { }));

        public object SelectedValuePath
        {
            get { return (object)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        #endregion

        #region ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(object), typeof(RadioButtonList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => { }));

        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        #endregion

        #region SelectionMode

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            "SelectionMode", typeof(SelectionMode), typeof(RadioButtonList), new FrameworkPropertyMetadata(SelectionMode.Single, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => { }));

        public SelectionMode SelectionMode
        {
            get { return (SelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        #endregion

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            var listbox = (RadListBox)Template.FindName(PartListbox, this);
            if (listbox != null)
            {
                listbox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ItemsSource") { Source = this, Mode = BindingMode.TwoWay });
                listbox.SetBinding(ItemsControlSelector.SelectionModeProperty, new Binding("SelectionMode") { Source = this, Mode = BindingMode.TwoWay });
                listbox.SetBinding(ItemsControlSelector.SelectedItemProperty, new Binding("SelectedItem") { Source = this, Mode = BindingMode.TwoWay });
                listbox.SetBinding(ItemsControlSelector.SelectedValuePathProperty, new Binding("SelectedValuePath") { Source = this, Mode = BindingMode.TwoWay });
                listbox.SetBinding(ItemsControlSelector.SelectedValueProperty, new Binding("SelectedValue") { Source = this, Mode = BindingMode.TwoWay });
            }
            base.OnApplyTemplate();
        }

        #endregion

    }
}
