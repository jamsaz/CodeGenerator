using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace $safeprojectname$.Dialog
{
    public class Dialog : Window
    {
        #region Part Names

        private const string ButtonsContainerPart = "PART_ButtonsContainer";

        #endregion

        #region Ctor

        static Dialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Dialog),
                new FrameworkPropertyMetadata(typeof(Dialog)));
        }
        public Dialog()
        {
            FlowDirection = FlowDirection.RightToLeft;
            WindowStyle = WindowStyle.SingleBorderWindow;
            Background = new SolidColorBrush(Color.FromRgb(0XFF, 0XFF, 0XFF));
        }


    #endregion

    #region DependencyProperties

    #region Buttons

    public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(
            "Buttons", typeof(DialogButtonCollection), typeof(Dialog), null);

        public DialogButtonCollection Buttons
        {
            get { return (DialogButtonCollection)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }

        #endregion

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            var buttonContainer = Template.FindName(ButtonsContainerPart, this) as StackPanel;
            if (buttonContainer != null)
            {
                if (Buttons != null)
                {
                    foreach (var button in Buttons)
                    {
                        buttonContainer.Children.Add(new RadButton
                        {
                            Content = button.Content,
                            Command = button.Command,
                            CommandParameter = button.CommandParameter,
                        });
                    }
                }
            }
            base.OnApplyTemplate();
        }

        #endregion
    }
}
