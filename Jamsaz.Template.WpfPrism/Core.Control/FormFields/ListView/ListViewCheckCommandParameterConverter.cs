using System;
using System.Globalization;
using System.Windows.Data;

namespace $safeprojectname$.FormFields.ListView
{
    public class ListViewCheckCommandParameterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new { ListView = values[0], Control = values[1] };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
