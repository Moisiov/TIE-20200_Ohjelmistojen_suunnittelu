using System;
using Avalonia;
using Avalonia.Data;
using Avalonia.Styling;
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
        
        private void OnSelectedTimeChanged(AvaloniaPropertyChangedEventArgs e)
        {
            m_filterModel.BaseValue = e.NewValue;
        }
    }
}
