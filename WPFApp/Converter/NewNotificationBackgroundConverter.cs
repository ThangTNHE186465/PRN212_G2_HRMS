using System.Drawing;
using System.Windows.Data;

namespace WPFApp.Converter;

public class NewNotificationBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is bool isNew && isNew)
        {
            return Brushes.LightYellow; // Nền màu vàng nhạt cho thông báo mới
        }
        return Brushes.Transparent; // Nền trong suốt cho thông báo cũ
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
