using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Styling;
// ReSharper disable InconsistentNaming

namespace FJ.Client.Core.UIElements.Filters.FilterItems
{
    public class FJFilterEnumComboBox : ComboBox, IStyleable
    {
        static FJFilterEnumComboBox()
        {
            FocusableProperty.OverrideDefaultValue<FJFilterEnumComboBox>(true);
            SelectedItemProperty.Changed.AddClassHandler<FJFilterEnumComboBox>(
                (x, e) => x.OnSelectedItemChanged(e));
            FilterModelProperty.Changed.AddClassHandler<FJFilterEnumComboBox>(
                (x, e) => x.OnFilterModelChanged(e));
        }
        
        public Type StyleKey => typeof(ComboBox);
        
        #region DependencyProperties

        private RegisterFilterModelBase m_filterModel;
        
        /// <summary>
        /// Filter model to be used with
        /// <see cref="FJ.Client.Core.UIElements.Filters.FilterItems.FJFilterEnumComboBox"/>.
        /// </summary>
        public RegisterFilterModelBase FilterModel
        {
            get => m_filterModel;
            set => SetAndRaise(FilterModelProperty, ref m_filterModel, value);
        }
        public static readonly DirectProperty<FJFilterEnumComboBox, RegisterFilterModelBase> FilterModelProperty =
            AvaloniaProperty.RegisterDirect<FJFilterEnumComboBox, RegisterFilterModelBase>(
                nameof(FilterModel),
                o => o.FilterModel,
                (o, v) => o.FilterModel = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true);
        
        #endregion

        public FJFilterEnumComboBox()
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

        private void OnSelectedItemChanged(AvaloniaPropertyChangedEventArgs e)
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
            SelectedItem = m_filterModel.BaseValue;
        }
    }
}
