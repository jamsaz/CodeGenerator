using System;
using System.Windows;
using System.Windows.Controls;

namespace $safeprojectname$
{
    public class Form : ContentControl
    {
        public Form()
        {
            SetResourceReference(StyleProperty, typeof(Form));
        }

        static Form()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Form), new FrameworkPropertyMetadata(typeof(Form)));
        }

        #region DependencyProperties

        #region Header

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof (object), typeof (Form), new PropertyMetadata(default(object)));

        public object Header
        {
            get { return (object) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        #endregion

        #region Id

        public static readonly DependencyProperty FormIdProperty = DependencyProperty.Register(
            "FormId", typeof (Guid), typeof (Form), new PropertyMetadata(default(Guid)));

        public Guid FormId
        {
            get { return (Guid) GetValue(FormIdProperty); }
            set { SetValue(FormIdProperty, value); }
        }

        #endregion

        #endregion
    }
}
