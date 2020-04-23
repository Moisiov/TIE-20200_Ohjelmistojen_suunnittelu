using System;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;

namespace FJ.Client.Core.ValueConverters
{
    public class BoolToBrushConverter : IValueConverter
    {
        private IBrush m_ifFalseBrush;
        private IBrush m_ifTrueBrush;
        public object IfFalse { get; set; }
        public object IfTrue { get; set; }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ResolveResources();
            var booleanValue = value as bool? ?? false;

            return booleanValue ? m_ifTrueBrush : m_ifFalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private void ResolveResources()
        {
            if (m_ifFalseBrush == null)
            {
                if (!(IfFalse is DynamicResourceExtension dynamicFalseResource)
                    || !Application.Current.TryFindResource(dynamicFalseResource.ResourceKey, out var resourceFalseColor)
                    || !(resourceFalseColor is IBrush targetFalseBrush))
                {
                    throw new InvalidOperationException(nameof(IfFalse));
                }

                m_ifFalseBrush = targetFalseBrush;
            }

            if (m_ifTrueBrush != null)
            {
                return;
            }
            
            if (!(IfTrue is DynamicResourceExtension dynamicTrueResource)
                || !Application.Current.TryFindResource(dynamicTrueResource.ResourceKey, out var resourceTrueColor)
                || !(resourceTrueColor is IBrush targetTrueBrush))
            {
                throw new InvalidOperationException(nameof(IfTrue));
            }

            m_ifTrueBrush = targetTrueBrush;
        }
    }
}
