using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Styling;
// ReSharper disable InconsistentNaming

namespace FJ.Client.Core.UIElements.Filters.FilterItems
{
    public class FJFilterTextBox : TextBox, IStyleable
    {
        static FJFilterTextBox()
        {
            FocusableProperty.OverrideDefaultValue<FJFilterTextBox>(true);
            TextProperty.Changed.AddClassHandler<FJFilterTextBox>(
                (x, e) => x.OnTextChanged(e));
        }
        
        public Type StyleKey => typeof(TextBox);
        
        #region DependencyProperties

        private RegisterFilterModelBase m_filterModel;
        
        /// <summary>
        /// Filter model to be used with
        /// <see cref="FJ.Client.Core.UIElements.Filters.FilterItems.FJFilterTextBox"/>.
        /// </summary>
        public RegisterFilterModelBase FilterModel
        {
            get => m_filterModel;
            set => SetAndRaise(FilterModelProperty, ref m_filterModel, value);
        }
        public static readonly DirectProperty<FJFilterTextBox, RegisterFilterModelBase> FilterModelProperty =
            AvaloniaProperty.RegisterDirect<FJFilterTextBox, RegisterFilterModelBase>(
                nameof(FilterModel),
                o => o.FilterModel,
                (o, v) => o.FilterModel = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true);
        
        #endregion
        private void OnTextChanged(AvaloniaPropertyChangedEventArgs e)
        {
            m_filterModel.BaseValue = e.NewValue;
        }
    }
}
