using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace PrimesWithCircles.Infrastucture.Converters
{
    public class EnumValuesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type enumType && enumType.IsEnum)
                return Enum.GetValues(enumType);

            return null!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}

