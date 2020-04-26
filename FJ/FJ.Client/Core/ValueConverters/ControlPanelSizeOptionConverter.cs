using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using FJ.Client.Core.Common;

namespace FJ.Client.Core.ValueConverters
{
    public class ControlPanelSizeOptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ControlPanelSizeOption sizeOption)
            {
                return sizeOption switch
                {
                    ControlPanelSizeOption.Expanded => new GridLength(280.0, GridUnitType.Pixel),
                    ControlPanelSizeOption.Minimized => new GridLength(70.0, GridUnitType.Pixel),
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
