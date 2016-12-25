using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Telerik.Windows.Controls;
using $safeprojectname$.GridView;
using GridViewColumn = Telerik.Windows.Controls.GridViewColumn;
using GridViewColumnCollection = $safeprojectname$.GridView.GridViewColumnCollection;

namespace $safeprojectname$.FormFields.ComboBox
{
    [DefaultProperty("Columns")]
    [ContentProperty("Columns")]
    public class GridComboBox : System.Windows.Controls.ComboBox
    {
        #region Part Names

        private const string partPopupDataGrid = "PART_PopupDataGrid";

        #endregion

        #region Private Fields

        //Attached DataGrid control
        private RadGridView popupDataGrid;

        #endregion

        #region Ctor

        static GridComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GridComboBox),
                new FrameworkPropertyMetadata(typeof(GridComboBox)));
        }

        #endregion

        #region DependencyProperties

        #region Columns

        //The property is default and Content property for CustComboBox
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns", typeof(GridViewColumnCollection), typeof(GridComboBox), new PropertyMetadata(default(GridViewColumnCollection)));

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GridViewColumnCollection Columns
        {
            get
            {
                var columns = (GridViewColumnCollection)GetValue(ColumnsProperty);
                return columns ?? new GridViewColumnCollection();
            }
            set { SetValue(ColumnsProperty, value); }
        }

        #endregion

        #endregion

        #region Events

        //Synchronize selection between Combo and DataGrid popup
        private void popupDataGrid_SelectionChanged(object sender, SelectionChangeEventArgs selectionChangeEventArgs)
        {
            //When open in Blend prevent raising exception 
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            var dg = sender as RadGridView;
            if (dg == null) return;
            SelectedItem = dg.SelectedItem;
        }

        //Event for DataGrid popup MouseDown
        private void popupDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var dg = sender as RadGridView;
            if (dg != null)
            {
                var dep = (DependencyObject)e.OriginalSource;

                // iteratively traverse the visual tree and stop when dep is one of ..
                while ((dep != null) &&
                       !(dep is DataGridCell) &&
                       !(dep is DataGridColumnHeader))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                if (dep == null)
                    return;

                if (dep is DataGridColumnHeader)
                {
                    // do something
                }
                //When user clicks to DataGrid cell, popup have to be closed
                if (dep is DataGridCell)
                {
                    this.IsDropDownOpen = false;
                }
            }
        }


        #endregion

        #region Overrides

        //Apply theme and attach columns to DataGrid popupo control
        public override void OnApplyTemplate()
        {
            if (popupDataGrid == null)
            {
                popupDataGrid = Template.FindName(partPopupDataGrid, this) as RadGridView;
                if (popupDataGrid != null && Columns != null)
                {
                    //Add columns to DataGrid columns
                    foreach (var column in Columns)
                    {
                        var columnType = column.ColumnType;
                        switch (columnType)
                        {
                            case GridViewColumnTypes.DataColumn:
                                popupDataGrid.Columns.Add(new GridViewDataColumn
                                {
                                    Header = column.HeadText,
                                    DataMemberBinding = new Binding(column.BindingPath)
                                });
                                break;
                            case GridViewColumnTypes.CheckBoxColumn:
                                popupDataGrid.Columns.Add(new GridViewCheckBoxColumn
                                {
                                    Header = column.HeadText,
                                    DataMemberBinding = new Binding(column.BindingPath)
                                });
                                break;
                            case GridViewColumnTypes.ComboBoxColumn:
                                popupDataGrid.Columns.Add(new GridViewComboBoxColumn
                                {
                                    Header = column.HeadText,
                                    DataMemberBinding = new Binding(column.BindingPath)
                                });
                                break;
                            case GridViewColumnTypes.TextBoxColumn:
                                popupDataGrid.Columns.Add(new GridViewColumn
                                {
                                    Header = column.HeadText,
                                });
                                break;
                        }
                    }

                    //Add event handler for DataGrid popup
                    popupDataGrid.MouseDown += popupDataGrid_MouseDown;
                    popupDataGrid.SelectionChanged += popupDataGrid_SelectionChanged;

                }
            }
            //Call base class method
            base.OnApplyTemplate();
        }
        //When selection changed in combobox (pressing  arrow key down or up) must be synchronized with opened DataGrid popup
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (popupDataGrid == null)
                return;

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (IsDropDownOpen)
                {
                    popupDataGrid.SelectedItem = SelectedItem;
                    if (popupDataGrid.SelectedItem != null)
                    {
                        object value = "[هیچ مقداری انتخاب نشده است]";
                        foreach (var prop in popupDataGrid.SelectedItem.GetType().GetProperties().Where(prop => prop.Name.Equals(DisplayMemberPath)))
                        {
                            value = prop.GetValue(popupDataGrid.SelectedItem);
                            popupDataGrid.ScrollIntoView(value);
                            return;
                        }
                        popupDataGrid.ScrollIntoView(value);
                    }
                }
            }
        }

        protected override void OnDropDownOpened(EventArgs e)
        {
            popupDataGrid.SelectedItem = SelectedItem;

            base.OnDropDownOpened(e);

            if (popupDataGrid.SelectedItem != null)
                popupDataGrid.ScrollIntoView(popupDataGrid.SelectedItem);
        }


        #endregion

    }
}
