using System;
using Avalonia;
using Avalonia.Data;
using Avalonia.Styling;
using FJ.DomainObjects;

// ReSharper disable InconsistentNaming

namespace FJ.Client.Core.UIElements.Filters.FilterItems
{
    public class FJFilterTimeRange : FJTimeRangePicker, IStyleable
    {
        static FJFilterTimeRange()
        {
            FocusableProperty.OverrideDefaultValue<FJFilterTimeRange>(true);
            SelectedTimeRangeProperty.Changed.AddClassHandler<FJFilterTimeRange>(
                (x, e) => x.OnSelectedTimeChanged(e));
            FilterModelProperty.Changed.AddClassHandler<FJFilterTimeRange>(
                (x, e) => x.OnFilterModelChanged(e));
        }

        public Type StyleKey => typeof(FJTimeRangePicker);
        
        #region DependencyProperties

        private RegisterFilterModelBase m_filterModel;
        
        /// <summary>
        /// Filter model to be used with
        /// <see cref="FJ.Client.Core.UIElements.Filters.FilterItems.FJFilterTimeRange"/>.
        /// </summary>
        public RegisterFilterModelBase FilterModel
        {
            get => m_filterModel;
            set => SetAndRaise(FilterModelProperty, ref m_filterModel, value);
        }
        public static readonly DirectProperty<FJFilterTimeRange, RegisterFilterModelBase> FilterModelProperty =
            AvaloniaProperty.RegisterDirect<FJFilterTimeRange, RegisterFilterModelBase>(
                nameof(FilterModel),
                o => o.FilterModel,
                (o, v) => o.FilterModel = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true);
        
        #endregion

        public FJFilterTimeRange()
        {
            if (m_filterModel != null)
            {
                m_filterModel.FilterChanged -= OnFilterChanged;
            }
        }
        
        private void OnSelectedTimeChanged(AvaloniaPropertyChangedEventArgs e)
        {
            m_filterModel.BaseValue = e.NewValue;
        }
        
        private void OnFilterModelChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.OldValue is RegisterFilterModelBase oldFilter)
            {
                oldFilter.FilterChanged -= OnFilterChanged;
            }
            
            m_filterModel.FilterChanged += OnFilterChanged;
            OnFilterChanged();
        }
        
        private void OnFilterChanged()
        {
            SetTimeRangeSelection(m_filterModel.BaseValue as TimeRange);
        }
    }
}
