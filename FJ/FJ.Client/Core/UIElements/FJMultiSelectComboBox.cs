using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace FJ.Client.Core.UIElements
{
    public class FJMultiSelectComboBox : TemplatedControl
    {
        static FJMultiSelectComboBox()
        {
            FocusableProperty.OverrideDefaultValue<FJMultiSelectComboBox>(true);
            IsDropDownOpenProperty.Changed.AddClassHandler<FJMultiSelectComboBox>(
                (x, e) => x.OnIsDropDownOpenChanged(e));

            PointerPressedEvent.AddClassHandler<FJMultiSelectComboBox>(
                (x, e) => x.OnPointerPressed(e), RoutingStrategies.Tunnel);
        }

        private const string c_textBoxName = "PART_MCBTextBox";
        private const string c_popupName = "PART_MCBPopup";
        private const string c_toggleButtonName = "PART_MCBDropDownToggle";
        private const string c_listBoxName = "PART_MCBListBox";
        private const string c_selectItemCheckBoxName = "PART_ItemCheckBox";

        private TextBox m_textBox;
        private Popup m_popup;
        private ToggleButton m_toggleButton;
        private ListBox m_listBox;

        private IDisposable m_subsOnOpen;
        private readonly List<IDisposable> m_checkBoxToggledSubs = new List<IDisposable>();

        private readonly HashSet<CheckBox> m_checkBoxes = new HashSet<CheckBox>();
        
        #region DependencyProperties
        
        private bool m_isDropDownOpen;
        
        /// <summary>
        /// Gets or sets a value indicating whether the dropdown is currently open.
        /// </summary>
        public bool IsDropDownOpen
        {
            get => m_isDropDownOpen;
            set => SetAndRaise(IsDropDownOpenProperty, ref m_isDropDownOpen, value);
        }
        public static readonly DirectProperty<FJMultiSelectComboBox, bool> IsDropDownOpenProperty =
            AvaloniaProperty.RegisterDirect<FJMultiSelectComboBox, bool>(
                nameof(IsDropDownOpen),
                o => o.IsDropDownOpen,
                (o, v) => o.IsDropDownOpen = v);
        
        /// <summary>
        /// Gets or sets the maximum height for the dropdown list.
        /// </summary>
        public double MaxDropDownHeight
        {
            get => GetValue(MaxDropDownHeightProperty);
            set => SetValue(MaxDropDownHeightProperty, value);
        }
        public static readonly StyledProperty<double> MaxDropDownHeightProperty =
            AvaloniaProperty.Register<FJMultiSelectComboBox, double>(nameof(MaxDropDownHeight), 200);
        
        /// <summary>
        /// Gets or set the watermark text that is displayed by the
        /// <see cref="T:FJ.Client.Core.UIElements.FJMultiSelectComboBox"/> when empty.
        /// </summary>
        public string Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
        public static readonly StyledProperty<string> WatermarkProperty =
            TextBox.WatermarkProperty.AddOwner<FJMultiSelectComboBox>();

        private IList m_selectedItems = new AvaloniaList<object>();
        
        /// <summary>
        /// Gets or sets the selected items
        /// </summary>
        public IList SelectedItems
        {
            get => m_selectedItems;
            set => m_selectedItems = value ?? new AvaloniaList<object>();
        }
        public static readonly DirectProperty<FJMultiSelectComboBox, IList> SelectedItemsProperty =
            AvaloniaProperty.RegisterDirect<FJMultiSelectComboBox, IList>(
                nameof(SelectedItems),
                o => o.SelectedItems,
                (o, v) => o.SelectedItems = v);
        
        private IEnumerable m_items = new AvaloniaList<object>();

        /// <summary>
        /// Gets or sets all selectable items
        /// </summary>
        public IEnumerable Items
        {
            get => m_items;
            set => SetAndRaise(ItemsProperty, ref m_items, value);
        }

        public static readonly DirectProperty<FJMultiSelectComboBox, IEnumerable> ItemsProperty =
            AvaloniaProperty.RegisterDirect<FJMultiSelectComboBox, IEnumerable>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v,
                defaultBindingMode: BindingMode.TwoWay);

        private IValueConverter m_valueConverter = new DefaultValueConverter();
        
        /// <summary>
        /// Gets or set the <see cref="Avalonia.Data.Converters.IValueConverter"/> used with drop down items
        /// </summary>
        public IValueConverter ValueConverter
        {
            get => m_valueConverter;
            set => m_valueConverter = value ?? m_valueConverter;
        }

        public static readonly DirectProperty<FJMultiSelectComboBox, IValueConverter> ValueConverterProperty =
            AvaloniaProperty.RegisterDirect<FJMultiSelectComboBox, IValueConverter>(
                nameof(ValueConverter),
                o => o.ValueConverter,
                (o, v) => o.ValueConverter = v);
        
        /// <summary>
        /// Gets or sets text box suffix in case of multiple selections on lost focus
        /// </summary>
        public string SuffixOnMany
        {
            get => GetValue(SuffixOnManyProperty);
            set => SetValue(SuffixOnManyProperty, value);
        }
        public static readonly StyledProperty<string> SuffixOnManyProperty =
            AvaloniaProperty.Register<FJMultiSelectComboBox, string>(nameof(SuffixOnMany), "kpl");

        #endregion
        
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            if (m_textBox != null)
            {
                m_textBox.GotFocus -= TextBox_GotFocus;
            }
            
            m_textBox = e.NameScope.Find<TextBox>(c_textBoxName);
            m_textBox.GotFocus += TextBox_GotFocus;

            if (m_popup != null)
            {
                m_popup.Opened -= PopupOpened;
                m_popup.Closed -= PopupClosed;
            }
            
            m_popup = e.NameScope.Find<Popup>(c_popupName);
            m_popup.Opened += PopupOpened;
            m_popup.Closed += PopupClosed;

            if (m_toggleButton != null)
            {
                m_toggleButton.Click -= ToggleButton_Clicked;
            }

            m_toggleButton = e.NameScope.Find<ToggleButton>(c_toggleButtonName);
            m_toggleButton.Click += ToggleButton_Clicked;

            if (m_listBox != null)
            {
                m_checkBoxToggledSubs.ForEach(x => x?.Dispose());
                m_checkBoxToggledSubs.Clear();
                
                m_listBox.Resources.Remove(ValueConverter);
                m_listBox.ItemContainerGenerator.Materialized -= OnItemContainerMaterialized;
            }
            
            m_listBox = e.NameScope.Find<ListBox>(c_listBoxName);
            m_listBox.Resources.Add(nameof(ValueConverter), ValueConverter);  // This is a sign of incompetency
            m_listBox.ItemContainerGenerator.Materialized += OnItemContainerMaterialized;

            SetTextBoxText();
            
            base.OnTemplateApplied(e);
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);
            if (!IsEnabled || m_textBox == null || e.NavigationMethod != NavigationMethod.Tab)
            {
                return;
            }
            
            m_textBox.Focus();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            IsDropDownOpen = false;
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (e.Handled
                || m_popup == null
                || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                return;
            }

            // TODO TextBox's IsPointerOver behaves weird sometimes, this is a workaround for that
            if (m_textBox.IsPointerOver && !m_listBox.IsPointerOver)
            {
                IsDropDownOpen = !IsDropDownOpen;
                e.Handled = true;
                return;
            }

            // Eat list box pointer press if not deleting
            if (!m_listBox.IsPointerOver)
            {
                return;
            }

            // Handle item selecting or deselecting manually
            var targetCheckBox = m_checkBoxes.FirstOrDefault(x => x.IsPointerOver);
            if (targetCheckBox != null)
            {
                targetCheckBox.IsChecked = targetCheckBox.IsChecked.HasValue
                    ? !targetCheckBox.IsChecked
                    : false;
            }
            
            e.Handled = true;
        }
        
        protected void SetTextBoxText()
        {
            if (m_textBox == null)
            {
                return;
            }
            
            if (m_selectedItems == null)
            {
                m_textBox.Text = string.Empty;
                return;
            }

            m_textBox.Text = m_selectedItems.Count switch
            {
                0 => string.Empty,
                1 => m_valueConverter
                    .Convert(m_selectedItems[0], typeof(string), null, CultureInfo.CurrentCulture)
                    .ToString(),
                _ => $"{SelectedItems.Count.ToString()} {SuffixOnMany}"
            };
        }

        private void CheckBoxToggledHandler(CheckBox checkBox, IContentControl item)
        {
            if (!IsDropDownOpen || !item.IsArrangeValid)
            {
                return;
            }

            if (checkBox.IsChecked == true && !SelectedItems.Contains(item.Content))
            {
                SelectedItems.Add(item.Content);
            }
            else
            {
                SelectedItems.Remove(item.Content);
            }
            
            RaisePropertyChanged(SelectedItemsProperty, m_selectedItems, m_selectedItems);
            SetTextBoxText();
        }
        
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
        }

        private void ToggleButton_Clicked(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
        }
        
        private void OnIsDropDownOpenChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var oldValue = (bool)e.OldValue;
            var value = (bool)e.NewValue;

            if (value == oldValue || m_popup == null)
            {
                return;
            }

            SetTextBoxText();
            m_popup.IsOpen = value;
        }
        
        private void PopupOpened(object sender, EventArgs e)
        {
            m_subsOnOpen?.Dispose();
            m_subsOnOpen = null;

            if (this.GetVisualRoot() is TopLevel toplevel)
            {
                m_subsOnOpen = toplevel.AddHandler(PointerWheelChangedEvent, (s, ev) =>
                {
                    // Eat wheel scroll event outside dropdown popup while it's open
                    if (IsDropDownOpen && Equals((ev.Source as IVisual).GetVisualRoot(), toplevel))
                    {
                        ev.Handled = true;
                    }
                }, RoutingStrategies.Tunnel);
            }
        }
        
        private void PopupClosed(object sender, EventArgs e)
        {
            m_subsOnOpen?.Dispose();
            m_subsOnOpen = null;
            IsDropDownOpen = false;

            if (Focusable && IsEffectivelyEnabled && IsVisible)
            {
                Focus();
            }
        }

        private void OnItemContainerMaterialized(object sender, ItemContainerEventArgs e)
        {
            if (!(e.Containers.LastOrDefault()?.ContainerControl is ListBoxItem latestItem))
            {
                return;
            }

            ((IContentPresenterHost)latestItem).LogicalChildren.PropertyChanged +=
                (s, ev) => FindAndSubscribeToLatestCheckBox(s, latestItem);
        }

        private void FindAndSubscribeToLatestCheckBox(object sender, ListBoxItem relatedListBoxItem)
        {
            if (!(sender is AvaloniaList<ILogical> l))
            {
                return;
            }
                
            var children = ((l.FirstOrDefault() as StackPanel)
                               ?.Children.FirstOrDefault() as DockPanel)
                           ?.Children
                           ?? new Controls();

            foreach (var child in children)
            {
                if (!(child is CheckBox checkBox) || checkBox.Name != c_selectItemCheckBoxName)
                {
                    continue;
                }

                if (SelectedItems.Contains(relatedListBoxItem.Content))
                {
                    checkBox.IsChecked = true;
                }

                var checkBoxCheckedSub = checkBox.GetObservable(ToggleButton.IsCheckedProperty)
                    .Subscribe(newValue => CheckBoxToggledHandler(checkBox, relatedListBoxItem));
                m_checkBoxToggledSubs.Add(checkBoxCheckedSub);
                m_checkBoxes.Add(checkBox);
            }
        }
    }
}
