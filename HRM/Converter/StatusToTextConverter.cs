using System.Globalization;
using System.Windows.Data;
using HRM.Models.Enum;

namespace HRM.Converter
{
    public class StatusToTextConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is EmployeeStatus status)
            {
                if (status == EmployeeStatus.Inactive)
                {
                    return "Chưa hoạt động";
                }
                if (status == EmployeeStatus.Active)
                {
                    return "Đang hoạt động";
                }
                if (status == EmployeeStatus.OnLeave)
                {
                    return "Nghỉ phép";
                }

                if (status == EmployeeStatus.Terminated)
                {
                    return "Đã nghỉ việc";
                }
            }
            return "Unknown";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}