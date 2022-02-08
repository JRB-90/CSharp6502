using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace CS6502.UIConsole.Converters
{
    internal class IntToHzStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                if (intValue >= 0 && intValue < 1000)
                {
                    return $"{intValue} Hz";
                }
                if (intValue >= 1000 && intValue < 1000000)
                {
                    return $"{intValue / 1000}.{intValue % 1000} KHz";
                }
                else if (intValue >= 1000000 && intValue < 1000000000)
                {
                    return $"{intValue / 1000000}.{(intValue % 1000000) / 1000} MHz";
                }
                else if (intValue >= 1000000000)
                {
                    return $"{intValue / 1000000000}.{(intValue % 1000000000) / 1000000} GHz";
                }
                else
                {
                    return "-";
                }
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
