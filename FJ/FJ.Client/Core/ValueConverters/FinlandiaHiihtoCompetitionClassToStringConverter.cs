using System;
using System.Globalization;
using Avalonia.Data.Converters;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.Utils.FinlandiaUtils;

namespace FJ.Client.Core.ValueConverters
{
    public class FinlandiaHiihtoCompetitionClassToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FinlandiaHiihtoCompetitionClass cc)
            {
                return cc.AsString();
            }

            throw new ArgumentException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
