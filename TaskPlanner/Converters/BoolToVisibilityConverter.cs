using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TaskPlanner.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если задача выполнена (IsCompleted = true), то скрываем кнопку
            if (value is bool boolValue && boolValue)
                return Visibility.Collapsed;  // Скрыть
            return Visibility.Visible;        // Показать
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}