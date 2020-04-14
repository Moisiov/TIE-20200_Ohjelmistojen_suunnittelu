using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using FJ.DomainObjects;

// ReSharper disable InconsistentNaming

namespace FJ.Client.Core.UIElements
{
    /// <summary>
    /// Time range picker containing two <see cref="T:FJ.Client.Core.UIElements.FJTimePicker"/>s
    /// </summary>
    public class FJTimeRangePicker : TemplatedControl
    {
        static FJTimeRangePicker()
        {
            FocusableProperty.OverrideDefaultValue<FJTimeRangePicker>(true);
        }
        
        private const string c_startTimePickerName = "PART_StartTimePicker";
        private const string c_endTimePickerName = "PART_EndTimePicker";

        private FJTimePicker m_startTimePicker;
        private FJTimePicker m_endTimePicker;

        private IDisposable m_startTimeChangedSub;
        private IDisposable m_endTimeChangedSub;
        
        #region DependencyProperties
        /// <summary>
        /// Gets or set the watermark text that is displayed by the start
        /// <see cref="T:FJ.Client.Core.UIElements.FJTimePicker"/> when empty.
        /// </summary>
        public string StartWatermark
        {
            get => GetValue(StartWatermarkProperty);
            set => SetValue(StartWatermarkProperty, value);
        }
        public static readonly StyledProperty<string> StartWatermarkProperty =
            AvaloniaProperty.Register<FJTimeRangePicker, string>(nameof(StartWatermark), "hh:mm");
        
        /// <summary>
        /// Gets or set the watermark text that is displayed by the end
        /// <see cref="T:FJ.Client.Core.UIElements.FJTimePicker"/> when empty.
        /// </summary>
        public string EndWatermark
        {
            get => GetValue(EndWatermarkProperty);
            set => SetValue(EndWatermarkProperty, value);
        }
        public static readonly StyledProperty<string> EndWatermarkProperty =
            AvaloniaProperty.Register<FJTimeRangePicker, string>(nameof(EndWatermark), "hh:mm");

        /// <summary>
        /// Gets or sets the brush used in separator part's background.
        /// </summary>
        public IBrush SeparatorBackground
        {
            get => GetValue(SeparatorBackgroundProperty);
            set => SetValue(SeparatorBackgroundProperty, value);
        }
        public static readonly StyledProperty<IBrush> SeparatorBackgroundProperty =
            AvaloniaProperty.Register<FJTimeRangePicker, IBrush>(nameof(SeparatorBackground), Brushes.Transparent);

        private TimeRange m_selectedTimeRange;

        public TimeRange SelectedTimeRange
        {
            get => m_selectedTimeRange;
            set => SetAndRaise(SelectedTimeRangeProperty, ref m_selectedTimeRange, value);
        }
        
        // TODO Implement support for two-way binding if needed
        public static readonly DirectProperty<FJTimeRangePicker, TimeRange> SelectedTimeRangeProperty =
            AvaloniaProperty.RegisterDirect<FJTimeRangePicker, TimeRange>(
                nameof(SelectedTimeRange),
                o => o.SelectedTimeRange,
                (o, v) => o.SelectedTimeRange = v,
                defaultBindingMode: BindingMode.OneWayToSource,
                enableDataValidation: true);
        #endregion

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            m_startTimePicker = e.NameScope.Find<FJTimePicker>(c_startTimePickerName);
            m_endTimePicker = e.NameScope.Find<FJTimePicker>(c_endTimePickerName);
            
            m_startTimeChangedSub?.Dispose();
            m_endTimeChangedSub?.Dispose();

            m_startTimeChangedSub = m_startTimePicker.GetObservable(FJTimePicker.SelectedTimeProperty)
                .Subscribe(time => TimeRangeSelectionChanged());
            m_endTimeChangedSub = m_endTimePicker.GetObservable(FJTimePicker.SelectedTimeProperty)
                .Subscribe(time => TimeRangeSelectionChanged());
            
            base.OnTemplateApplied(e);
        }

        private void TimeRangeSelectionChanged()
        {
            SelectedTimeRange = new TimeRange
            {
                Start = m_startTimePicker.SelectedTime,
                End = m_endTimePicker.SelectedTime
            };
        }
    }
}
