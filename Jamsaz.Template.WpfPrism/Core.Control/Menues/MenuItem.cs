using System;
using System.Windows;
using System.Windows.Input;

namespace $safeprojectname$.Menues
{
    public class MenuItem : DependencyObject
    {
        public MenuItem() { }
        public MenuItem(string title, string iconPath, MenuItemCollection children = null)
        {
            Title = title;
            IconPath = iconPath;
            Children = children ?? new MenuItemCollection();
        }
        public Guid UniqId { get; } = Guid.NewGuid();

        #region DependencyProperties

        #region Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof (string), typeof (MenuItem), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        #region IconPath

        public static readonly DependencyProperty IconPathProperty = DependencyProperty.Register(
            "IconPath", typeof (string), typeof (MenuItem), new PropertyMetadata(default(string)));

        public string IconPath
        {
            get { return (string) GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        #endregion

        #region Children

        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(
            "Children", typeof (MenuItemCollection), typeof (MenuItem), new PropertyMetadata(default(MenuItemCollection)));

        public MenuItemCollection Children
        {
            get { return (MenuItemCollection) GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }

        #endregion

        #region Command

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(MenuItem), new PropertyMetadata(default(ICommand)));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(MenuItem), new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #endregion

        #endregion
    }
}
