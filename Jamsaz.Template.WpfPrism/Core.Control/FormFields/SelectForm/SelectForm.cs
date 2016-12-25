using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Telerik.Windows.Controls;
using $safeprojectname$.Dialog;
using $safeprojectname$.FormFields.ComboBox;
using GridViewColumnCollection = $safeprojectname$.GridView.GridViewColumnCollection;

namespace $safeprojectname$.FormFields.SelectForm
{
    public class SelectForm : ContentControl
    {
        #region Part Names

        private const string GridContentPart = "PART_GridContent";

        #endregion

        #region Private Fields

        #region Commands
        private SelectFormButtonCommand Command => new SelectFormButtonCommand(OnButtonClick);

        private void OnButtonClick(object o)
        {
            var dialog = new Dialog.Dialog { Title = "لطفا انتخاب کنید" };
            var gridview = new GridView.GridView { DataContext = DataContext };
            gridview.SetBinding(GridView.GridView.ColumnsProperty,
                new Binding("Columns") { Source = this, Mode = BindingMode.TwoWay });
            gridview.SetBinding(GridView.GridView.DataSourceProperty,
                new Binding("ItemSource") { Source = this, Mode = BindingMode.TwoWay });
            gridview.SelectionChanged = (sender, e) =>
            {
                dialog.DialogResult = true;
                SelectedItem = gridview.SelectedItem;
                dialog.Close();
            };
            dialog.Content = gridview;
            dialog.Buttons = new DialogButtonCollection
            {
                new DialogButton
                {
                    Content = "انتخاب",
                    Command = new DelegateCommand(p =>
                    {
                        dialog.DialogResult = true;
                        SelectedItem = gridview.SelectedItem;
                        dialog.Close();
                    }),
                    CommandParameter = o
                },
                new DialogButton
                {
                    Content = "انصراف",
                    Command = new DelegateCommand(obj =>
                    {
                        dialog.DialogResult = false;
                        dialog.Close();
                    }),
                    CommandParameter = o
                }
            };
            dialog.ShowDialog();
        }

        #endregion


        #endregion

        #region Ctor

        static SelectForm()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectForm),
                new FrameworkPropertyMetadata(typeof(SelectForm)));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            if (FieldType != null)
            {
                switch (FieldType.FieldTypes)
                {
                    case SelectFormFieldTypes.Form:
                        var button = new Button { Content = ". . .", Command = Command };
                        var grid = Template.FindName(GridContentPart, this) as Grid;
                        if (grid != null)
                        {
                            grid.Visibility = Visibility.Visible;
                            FrameworkElement formElement = new TextBox();
                            switch (FieldType.Field)
                            {
                                case FieldTypes.Combobox:
                                    formElement = new ComboBox.ComboBox();
                                    formElement.SetBinding(ItemsControl.ItemsSourceProperty,
                                        new Binding("ItemSource") { Source = this, Mode = BindingMode.TwoWay });
                                    formElement.SetBinding(Selector.SelectedItemProperty,
                                        new Binding("SelectedItem") { Source = this, Mode = BindingMode.TwoWay });
                                    formElement.SetBinding(ItemsControl.DisplayMemberPathProperty,
                                        new Binding("DisplayMemberPath") { Source = this, Mode = BindingMode.TwoWay });
                                    formElement.SetBinding(Selector.SelectedValuePathProperty,
                                       new Binding("SelectedValuePath") { Source = this, Mode = BindingMode.TwoWay });
                                    break;
                                case FieldTypes.DateTimebox:
                                    formElement = new DatePicker();
                                    formElement.SetBinding(Selector.SelectedItemProperty,
                                        new Binding("SelectedItem") { Source = this, Mode = BindingMode.TwoWay });
                                    break;
                                default:
                                    formElement.SetBinding(TextBox.TagProperty,
                                        new Binding("SelectedItem") { Source = this, Mode = BindingMode.TwoWay });
                                    formElement.SetBinding(TextBox.TextProperty,
                                        new Binding($"Tag.{DisplayMemberPath}") { Source = formElement, Mode = BindingMode.TwoWay });
                                    break;
                            }
                            grid.Children.Add(button);
                            grid.Children.Add(formElement);
                            formElement.SetValue(Grid.ColumnProperty, 0);
                            button.SetValue(Grid.ColumnProperty, 1);
                        }
                        break;
                    default:
                        var combo = new GridComboBox();
                        combo.SetBinding(Selector.SelectedItemProperty,
                            new Binding("SelectedItem") { Mode = BindingMode.TwoWay, Source = this });
                        combo.SetBinding(ItemsControl.ItemsSourceProperty,
                            new Binding("ItemSource") { Mode = BindingMode.TwoWay, Source = this });
                        combo.SetBinding(GridComboBox.ColumnsProperty,
                            new Binding("Columns") { Mode = BindingMode.TwoWay, Source = this });
                        combo.SetBinding(ItemsControl.DisplayMemberPathProperty,
                            new Binding("DisplayMemberPath") { Mode = BindingMode.TwoWay, Source = this });
                        Content = combo;
                        break;
                }
            }
            base.OnApplyTemplate();
        }

        #endregion

        #region DependencyProperties

        #region FieldTypes

        public static readonly DependencyProperty FieldTypeProperty = DependencyProperty.Register(
            "FieldType", typeof(SelectFormField), typeof(SelectForm), new FrameworkPropertyMetadata(null));

        public SelectFormField FieldType
        {
            get { return (SelectFormField)GetValue(FieldTypeProperty); }
            set { SetValue(FieldTypeProperty, value); }
        }

        #endregion

        #region ItemSource

        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
            "ItemSource", typeof(object), typeof(SelectForm), new PropertyMetadata(default(object)));

        public object ItemSource
        {
            get { return (object)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        #endregion

        #region SelectedItem

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(SelectForm), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) =>
            {


            }));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        #endregion

        #region Columns

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns", typeof(GridViewColumnCollection), typeof(SelectForm), new PropertyMetadata(default(GridViewColumnCollection)));

        public GridViewColumnCollection Columns
        {
            get { return (GridViewColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        #endregion

        #region DisplayMemberPath

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
            "DisplayMemberPath", typeof(string), typeof(SelectForm), new PropertyMetadata(default(string)));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        #endregion

        #region SelectedValuePath

        public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register(
            "SelectedValuePath", typeof(object), typeof(SelectForm), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => { }));

        public object SelectedValuePath
        {
            get { return (object)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        #endregion

        #region LabelText

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
            "LabelText", typeof(string), typeof(SelectForm), new PropertyMetadata(default(string)));

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        #endregion

        #endregion
    }
}
