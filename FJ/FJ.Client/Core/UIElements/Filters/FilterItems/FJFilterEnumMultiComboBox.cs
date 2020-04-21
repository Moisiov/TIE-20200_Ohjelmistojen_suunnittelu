using System;
using System.Collections;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Styling;
// ReSharper disable InconsistentNaming

namespace FJ.Client.Core.UIElements.Filters.FilterItems
{
    public class FJFilterEnumMultiComboBox : FJMultiSelectComboBox, IStyleable
    {
        static FJFilterEnumMultiComboBox()
        {
            FocusableProperty.OverrideDefaultValue<FJFilterEnumMultiComboBox>(true);
            SelectedItemsProperty.Changed.AddClassHandler<FJFilterEnumMultiComboBox>(
                (x, e) => x.OnSelectedItemsChanged(e));
            FilterModelProperty.Changed.AddClassHandler<FJFilterEnumMultiComboBox>(
                (x, e) => x.OnFilterModelChanged(e));
        }
        
        public Type StyleKey => typeof(FJMultiSelectComboBox);
        
        #region DependencyProperties

        private RegisterFilterModelBase m_filterModel;
        
        /// <summary>
        /// Filter model to be used with
        /// <see cref="FJ.Client.Core.UIElements.Filters.FilterItems.FJFilterEnumMultiComboBox"/>.
        /// </summary>
        public RegisterFilterModelBase FilterModel
        {
            get => m_filterModel;
            set => SetAndRaise(FilterModelProperty, ref m_filterModel, value);
        }
        public static readonly DirectProperty<FJFilterEnumMultiComboBox, RegisterFilterModelBase> FilterModelProperty =
            AvaloniaProperty.RegisterDirect<FJFilterEnumMultiComboBox, RegisterFilterModelBase>(
                nameof(FilterModel),
                o => o.FilterModel,
                (o, v) => o.FilterModel = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true);
        
        #endregion

        public FJFilterEnumMultiComboBox()
        {
            if (m_filterModel != null)
            {
                m_filterModel.FilterChanged -= OnFilterChanged;
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            
            UpdateItems();
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
            UpdateItems();
            OnFilterChanged();
        }

        private void UpdateItems()
        {
            var filterValueType = m_filterModel.ValueType;

            if (!filterValueType.IsEnum)
            {
                throw new InvalidOperationException($"{GetType().Name} needs enum as value type");
            }
            
            Items = Enum.GetValues(filterValueType);
        }
        
        private void OnFilterChanged()
        {
            SelectedItems = m_filterModel.BaseValue as IList;
            SetTextBoxText();
        }
    }
}
