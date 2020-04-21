using System;
using System.Collections;
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
            FilterModelProperty.Changed.AddClassHandler<FJFilterMultiTextBox>(
                (x, e) => x.OnFilterModelChanged(e));
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

        public FJFilterMultiTextBox()
        {
            if (m_filterModel != null)
            {
                m_filterModel.FilterChanged -= OnFilterChanged;
            }
        }
        
        private void OnSelectedItemsChanged(AvaloniaPropertyChangedEventArgs e)
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
            SelectedItems = m_filterModel.BaseValue as IList;
            SetTextBoxText();
        }
    }
}
