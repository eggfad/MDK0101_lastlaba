using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TaskPlanner.Converters
{
    public class DeadlineColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isOverdue && isOverdue)
                return new SolidColorBrush(Color.FromRgb(220, 50, 47)); // красный
            return new SolidColorBrush(Color.FromRgb(42, 161, 152)); // бирюзовый
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}