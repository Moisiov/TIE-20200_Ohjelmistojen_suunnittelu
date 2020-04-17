using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Styling;
// ReSharper disable InconsistentNaming

namespace FJ.Client.Core.UIElements.Filters.FilterItems
{
    public class FJFilterComboBox : ComboBox, IStyleable
    {
        static FJFilterComboBox()
        {
            FocusableProperty.OverrideDefaultValue<FJFilterComboBox>(true);
            SelectedItemProperty.Changed.AddClassHandler<FJFilterComboBox>(
                (x, e) => x.OnSelectedItemChanged(e));
        }
        
        public Type StyleKey => typeof(ComboBox);
        
        #region DependencyProperties

        private RegisterFilterModelBase m_filterModel;
        
        /// <summary>
        /// Filter model to be used with
        /// <see cref="FJ.Client.Core.UIElements.Filters.FilterItems.FJFilterComboBox"/>.
        /// </summary>
        public RegisterFilterModelBase FilterModel
        {
            get => m_filterModel;
            set => SetAndRaise(FilterModelProperty, ref m_filterModel, value);
        }
        public static readonly DirectProperty<FJFilterComboBox, RegisterFilterModelBase> FilterModelProperty =
            AvaloniaProperty.RegisterDirect<FJFilterComboBox, RegisterFilterModelBase>(
                nameof(FilterModel),
                o => o.FilterModel,
                (o, v) => o.FilterModel = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true);
        
        #endregion
        private void OnSelectedItemChanged(AvaloniaPropertyChangedEventArgs e)
        {
            m_filterModel.BaseValue = e.NewValue;
        }
    }
}
