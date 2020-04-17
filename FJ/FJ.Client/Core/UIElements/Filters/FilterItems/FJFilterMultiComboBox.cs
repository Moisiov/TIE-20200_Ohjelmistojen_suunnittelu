using System;
using Avalonia;
using Avalonia.Data;
using Avalonia.Styling;
// ReSharper disable InconsistentNaming

namespace FJ.Client.Core.UIElements.Filters.FilterItems
{
    public class FJFilterMultiComboBox : FJMultiSelectComboBox, IStyleable
    {
        static FJFilterMultiComboBox()
        {
            FocusableProperty.OverrideDefaultValue<FJFilterMultiComboBox>(true);
            SelectedItemsProperty.Changed.AddClassHandler<FJFilterMultiComboBox>(
                (x, e) => x.OnSelectedItemsChanged(e));
        }
        
        public Type StyleKey => typeof(FJMultiSelectComboBox);
        
        #region DependencyProperties

        private RegisterFilterModelBase m_filterModel;
        
        /// <summary>
        /// Filter model to be used with
        /// <see cref="FJ.Client.Core.UIElements.Filters.FilterItems.FJFilterMultiComboBox"/>.
        /// </summary>
        public RegisterFilterModelBase FilterModel
        {
            get => m_filterModel;
            set => SetAndRaise(FilterModelProperty, ref m_filterModel, value);
        }
        public static readonly DirectProperty<FJFilterMultiComboBox, RegisterFilterModelBase> FilterModelProperty =
            AvaloniaProperty.RegisterDirect<FJFilterMultiComboBox, RegisterFilterModelBase>(
                nameof(FilterModel),
                o => o.FilterModel,
                (o, v) => o.FilterModel = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true);
        
        #endregion

        private void OnSelectedItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            m_filterModel.BaseValue = e.NewValue;
        }
    }
}
