using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace $safeprojectname$.GridView
{
    public class GridView : ContentControl
    {
        #region Ctor

        static GridView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GridView), new FrameworkPropertyMetadata(typeof(GridView)));
        }

        public GridView()
        {
            Setting.CanSort = false;
            Setting.CanGroup = false;
            Setting.CanExport = false;
            Setting.CanFilter = false;
            Setting.CanPaging = false;
            Setting.CanUserInsertRows = false;
            Setting.CanUserDeleteRows = false;
        }

        #endregion

        #region Part Names

        private const string RootPart = "PART_RootLayout";
        private const string RadGridPart = "PART_RadGridView";
        private const string RadPagerPart = "PART_RadDataPager";
        private const string RadGridButtonsPart = "PART_GridViewButtonContainer";

        #endregion

        #region DependencyProperties

        #region Columns

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns", typeof(GridViewColumnCollection), typeof(GridView), new PropertyMetadata(default(GridViewColumnCollection)));

        public GridViewColumnCollection Columns
        {
            get { return (GridViewColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        #endregion

        #region Buttons

        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(
            "Buttons", typeof(GridViewButtonCollection), typeof(GridView), new PropertyMetadata(default(GridViewButtonCollection)));

        public GridViewButtonCollection Buttons
        {
            get { return (GridViewButtonCollection)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }

        #endregion

        #region DataSource

        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(
            "DataSource", typeof(object), typeof(GridView), new PropertyMetadata(default(object)));

        public object DataSource
        {
            get { return (object)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        #endregion

        #region PageSize

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register(
            "PageSize", typeof(int), typeof(GridView), new PropertyMetadata(50));

        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        #endregion

        #region Setting

        public static readonly DependencyProperty SettingProperty = DependencyProperty.Register(
            "Setting", typeof(GridViewSetting), typeof(GridView), new PropertyMetadata(new GridViewSetting()));

        public GridViewSetting Setting
        {
            get { return (GridViewSetting)GetValue(SettingProperty); }
            set { SetValue(SettingProperty, value); }
        }

        #endregion

        #region SelectedItem

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(GridView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => { }));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        #endregion

        #endregion

        #region Events

        public Action<object, MouseButtonEventArgs> SelectionChanged;
        private void RadGridViewOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            var radGridView = Template.FindName(RadGridPart, this) as RadGridView;
            var radGridButtonContainer = Template.FindName(RadGridButtonsPart, this) as StackPanel;
            var rootLayout = Template.FindName(RootPart, this) as Grid;
            var radDataPager = Template.FindName(RadPagerPart, this) as RadDataPager;
            if (radGridView != null && radGridButtonContainer != null && rootLayout != null && radDataPager != null)
            {
                #region Columns
                if (Columns != null)
                    foreach (var column in Columns.Where(x=>x.Visible))
                    {
                        switch (column.ColumnType)
                        {
                            case GridViewColumnTypes.DataColumn:
                                radGridView.Columns.Add(new GridViewDataColumn
                                {
                                    Header = column.HeadText,
                                    DataMemberBinding = new Binding(column.BindingPath)
                                });
                                break;
                            case GridViewColumnTypes.CheckBoxColumn:
                                radGridView.Columns.Add(new GridViewCheckBoxColumn
                                {
                                    Header = column.HeadText,
                                    DataMemberBinding = new Binding(column.BindingPath)
                                });
                                break;
                            case GridViewColumnTypes.ComboBoxColumn:
                                radGridView.Columns.Add(new GridViewComboBoxColumn
                                {
                                    Header = column.HeadText,
                                    DataMemberBinding = new Binding(column.BindingPath),
                                    ItemsSourceBinding = new Binding(column.ItemSourceBindingPath) { Source = radGridView.DataContext, Mode = BindingMode.TwoWay },
                                    SelectedValueMemberPath = column.SelectedValueMemberPath,
                                    DisplayMemberPath = column.DisplayMemberPath,
                                });
                                break;
                        }
                    }
                #endregion
                #region Setting
                radDataPager.SetBinding(RadDataPager.SourceProperty,
                    new Binding("DataSource") { Source = this, Mode = BindingMode.TwoWay });
                radDataPager.SetBinding(RadDataPager.PageSizeProperty,
                    new Binding("PageSize") { Source = this, Mode = BindingMode.TwoWay });
                radGridView.SetBinding(DataControl.ItemsSourceProperty,
                    new Binding("PagedSource") { Source = radDataPager });
                radGridView.SetBinding(DataControl.SelectedItemProperty,
                    new Binding("SelectedItem") { Source = this, Mode = BindingMode.TwoWay });
                radGridView.MouseDoubleClick += RadGridViewOnMouseDoubleClick;
                if (Setting != null)
                {
                    radGridButtonContainer.Visibility = Setting.CanExport
                        ? Visibility.Visible
                        : Visibility.Hidden;
                    if (Setting.CanPaging)
                    {
                        rootLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
                    }
                    rootLayout.RowDefinitions.Add(new RowDefinition());
                    radDataPager.Visibility = Setting.CanPaging ? Visibility.Visible : Visibility.Hidden;
                    if (Setting.CanPaging)
                    {
                        rootLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                    radGridView.IsFilteringAllowed = Setting.CanFilter;
                    radGridView.ShowGroupPanel = Setting.CanGroup;
                    radGridView.CanUserSortColumns = Setting.CanSort;
                    radGridView.CanUserSortGroups = Setting.CanSort;
                    radGridView.CanUserDeleteRows = Setting.CanUserDeleteRows;
                    radGridView.CanUserInsertRows = Setting.CanUserInsertRows;
                }
                #endregion
                #region Buttons
                if (Buttons != null)
                {
                    foreach (var radButton in Buttons.Select(btn => new RadButton
                    {
                        Content = btn.Content,
                        Command = new ButtonCommand(),
                        CommandParameter =
                            new GridViewButtonCommandParameter
                            {
                                Parent = radGridView,
                                Action = btn.GridViewButtonTypes,
                                Parameter = btn.Parameter
                            }
                    }))
                    {
                        radGridButtonContainer.Children.Add(radButton);
                    }
                }
                #endregion
            }
            base.OnApplyTemplate();
        }

        #endregion
    }
}
