using System;
using Avalonia;
using Avalonia.Data;
using Avalonia.Styling;
// ReSharper disable InconsistentNaming

namespace FJ.Client.Core.UIElements.Filters.FilterItems
{
    public class FJFilterMultiTextBox : FJPopupTextBox, IStyleable
    {
        static FJFilterMultiTextBox()
        {
            FocusableProperty.OverrideDefaultValue<FJFilterMultiTextBox>(true);
            SelectedItemsProperty.Changed.AddClassHandler<FJFilterMultiTextBox>(
                (x, e) => x.OnSelectedItemsChanged(e));
        }
        
        public Type StyleKey => typeof(FJPopupTextBox);
        
        #region DependencyProperties

        private RegisterFilterModelBase m_filterModel;
        
        /// <summary>
        /// Filter model to be used with
        /// <see cref="FJ.Client.Core.UIElements.Filters.FilterItems.FJFilterMultiTextBox"/>.
        /// </summary>
        public RegisterFilterModelBase FilterModel
        {
            get => m_filterModel;
            set => SetAndRaise(FilterModelProperty, ref m_filterModel, value);
        }
        public static readonly DirectProperty<FJFilterMultiTextBox, RegisterFilterModelBase> FilterModelProperty =
            AvaloniaProperty.RegisterDirect<FJFilterMultiTextBox, RegisterFilterModelBase>(
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
