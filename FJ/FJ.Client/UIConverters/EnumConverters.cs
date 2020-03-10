using System;
using System.Globalization;
using Avalonia.Data.Converters;
using FJ.Utils;

namespace FJ.Client.UIConverters
{
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                if (parameter is string p && p == "s")
                {
                    return enumValue.GetShortDescription();
                }

                return enumValue.GetDescription();
            }

            throw new ArgumentException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
