using System.Windows;

namespace $safeprojectname$.FormFields
{
    public class TextBox : System.Windows.Controls.TextBox
    {
        #region DependencyObject

        public static readonly DependencyProperty IsValidTextProperty = DependencyProperty.Register(
            "IsValidText", typeof (bool), typeof (TextBox), new PropertyMetadata(default(bool)));

        public bool IsValidText
        {
            get { return (bool) GetValue(IsValidTextProperty); }
            set { SetValue(IsValidTextProperty, value); }
        }

        #endregion
    }
}