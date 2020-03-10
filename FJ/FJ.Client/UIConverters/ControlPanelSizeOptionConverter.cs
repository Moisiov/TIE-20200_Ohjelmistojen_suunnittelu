using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using FJ.Client.UIUtils;

namespace FJ.Client.UIConverters
{
    public class ControlPanelSizeOptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ControlPanelSizeOption sizeOption)
            {
                return sizeOption switch
                {
                    ControlPanelSizeOption.Expanded => new GridLength(double.Parse(parameter.ToString(), culture), GridUnitType.Star),
                    ControlPanelSizeOption.Minimized => new GridLength(double.Parse(parameter.ToString(), culture) / 5, GridUnitType.Star),
                    _ => throw new NotImplementedException(),
                };
            }

            throw new ArgumentException(nameof(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
