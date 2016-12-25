using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FarsiLibrary.WPF.Controls;

namespace $safeprojectname$.FormFields
{
    public class DatePicker : ContentControl
    {
        #region PartNames

        private const string PartDatePicker = "PART_DatePicker";

        #endregion

        #region Ctor

        static DatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (DatePicker),
                new FrameworkPropertyMetadata(typeof (DatePicker)));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            var datepicker = Template.FindName(PartDatePicker, this) as FXDatePicker;
            datepicker?.SetBinding(FXDatePicker.SelectedDateTimeProperty,
                new Binding("Value") {Source = this, Mode = BindingMode.TwoWay});
            base.OnApplyTemplate();
        }

        #endregion

        #region DependencyProperties

        #region Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(DateTime?), typeof(DatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => { }));

        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        #endregion

        #endregion

    }
}
