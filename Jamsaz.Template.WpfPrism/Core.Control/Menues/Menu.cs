using System;
using System.Windows;
using System.Windows.Controls;

namespace $safeprojectname$.Menues
{
    public class Menu : Control
    {
        static Menu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(typeof(Menu)));
        }

        #region DependencyProperties

        #region Items

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", typeof(MenuItemCollection), typeof(Menu), new PropertyMetadata(default(MenuItemCollection)));

        public MenuItemCollection Items
        {
            get { return (MenuItemCollection)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }

        }

        #endregion

        #region ItemTemplate

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate", typeof(DataTemplate), typeof(Menu), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        #endregion


        #endregion
    }
}
