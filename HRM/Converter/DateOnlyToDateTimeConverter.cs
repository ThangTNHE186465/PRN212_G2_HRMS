using System.Globalization;
using System.Windows.Data;

namespace HRM.Converter;

public class DateOnlyToDateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateOnly dateOnly)
        {
            // Chuyển đổi DateOnly sang DateTime, với thời gian mặc định là 00:00:00
            return dateOnly.ToDateTime(TimeOnly.MinValue);
        }
        return DateTime.MinValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            // Chuyển đổi DateTime sang DateOnly
            return DateOnly.FromDateTime(dateTime);
        }
        return null;
    }
}