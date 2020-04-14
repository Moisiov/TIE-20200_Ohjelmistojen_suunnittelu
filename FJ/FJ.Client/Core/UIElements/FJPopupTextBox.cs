using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace FJ.Client.Core.UIElements
{
    public class FJPopupTextBox : TemplatedControl
    {
        static FJPopupTextBox()
        {
            FocusableProperty.OverrideDefaultValue<FJPopupTextBox>(true);
            IsDropDownOpenProperty.Changed.AddClassHandler<FJPopupTextBox>(
                (x, e) => x.OnIsDropDownOpenChanged(e));

            PointerPressedEvent.AddClassHandler<FJPopupTextBox>(
                (x, e) => x.OnPointerPressed(e), RoutingStrategies.Tunnel);
        }

        private const string c_textBoxName = "PART_PTBTextBox";
        private const string c_popupName = "PART_PTBPopup";
        private const string c_listBoxName = "PART_PTBListBox";
        private const string c_deleteItemToggleName = "PART_DeleteItemToggle";

        private TextBox m_textBox;
        private Popup m_popup;
        private ListBox m_listBox;

        private IDisposable m_subsOnOpen;
        private IDisposable m_listBoxItemsChangedSub;

        private List<string> m_selectedItemsCopy;
        private readonly HashSet<ToggleButton> m_toggleButtons = new HashSet<ToggleButton>();
        
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
        public static readonly DirectProperty<FJPopupTextBox, bool> IsDropDownOpenProperty =
            AvaloniaProperty.RegisterDirect<FJPopupTextBox, bool>(
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
            AvaloniaProperty.Register<FJPopupTextBox, double>(nameof(MaxDropDownHeight), 200);
        
        /// <summary>
        /// Gets or set the watermark text that is displayed by the
        /// <see cref="T:FJ.Client.Core.UIElements.FJPopupTextBox"/> when empty.
        /// </summary>
        public string Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
        public static readonly StyledProperty<string> WatermarkProperty =
            TextBox.WatermarkProperty.AddOwner<FJPopupTextBox>();

        private IList m_selectedItems = new AvaloniaList<object>();
        
        /// <summary>
        /// Gets or sets the selected items
        /// </summary>
        public IList SelectedItems
        {
            get => m_selectedItems ??= new AvaloniaList<object>();
            set => m_selectedItems = value ?? new AvaloniaList<object>();
        }
        public static readonly DirectProperty<FJPopupTextBox, IList> SelectedItemsProperty =
            AvaloniaProperty.RegisterDirect<FJPopupTextBox, IList>(
                nameof(SelectedItems),
                o => o.SelectedItems,
                (o, v) => o.SelectedItems = v);

        /// <summary>
        /// Gets or sets text box suffix in case of multiple selections on lost focus
        /// </summary>
        public string SuffixOnMany
        {
            get => GetValue(SuffixOnManyProperty);
            set => SetValue(SuffixOnManyProperty, value);
        }
        public static readonly StyledProperty<string> SuffixOnManyProperty =
            AvaloniaProperty.Register<FJPopupTextBox, string>(nameof(SuffixOnMany), "kpl");

        #endregion
        
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            if (m_textBox != null)
            {
                m_textBox.KeyDown -= TextBox_KeyDown;
                m_textBox.GotFocus -= TextBox_GotFocus;
            }
            
            m_textBox = e.NameScope.Find<TextBox>(c_textBoxName);
            m_textBox.KeyDown += TextBox_KeyDown;
            m_textBox.GotFocus += TextBox_GotFocus;

            if (m_popup != null)
            {
                m_popup.Opened -= PopupOpened;
                m_popup.Closed -= PopupClosed;
            }
            
            m_popup = e.NameScope.Find<Popup>(c_popupName);
            m_popup.Opened += PopupOpened;
            m_popup.Closed += PopupClosed;

            if (m_listBox != null)
            {
                m_listBox.ItemContainerGenerator.Materialized -= OnItemContainerMaterialized;
                m_listBoxItemsChangedSub?.Dispose();
            }
            
            m_listBox = e.NameScope.Find<ListBox>(c_listBoxName);
            m_listBox.ItemContainerGenerator.Materialized += OnItemContainerMaterialized;
            m_listBoxItemsChangedSub = m_listBox.GetObservable(ItemsControl.ItemCountProperty)
                .Subscribe(item => ListBox_ItemsChanged());

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
            
            m_textBox.Text = string.Empty;
            m_textBox.Focus();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            
            if (e.Handled || !IsDropDownOpen)
            {
                return;
            }

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

            // Eat list box pointer press if not deleting
            if (!m_listBox.IsPointerOver)
            {
                return;
            }

            // Handle item deleting manually
            var targetToggle = m_toggleButtons.FirstOrDefault(x => x.IsPointerOver);
            if (targetToggle != null)
            {
                targetToggle.IsChecked = true;
            }
            
            e.Handled = true;
        }
        
        private void DeleteItem(object sender, IContentControl item)
        {
            if (!IsDropDownOpen 
                || !item.IsArrangeValid
                || !(sender is ToggleButton toggle))
            {
                return;
            }

            m_toggleButtons.Remove(toggle);
            SelectedItems.Remove(item.Content);
        }
        
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            
            e.Handled = ProcessTextBoxKey(e);
        }
        
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = true;
        }

        private bool ProcessTextBoxKey(KeyEventArgs e)
        {
            if (e.Key != Key.Enter || string.IsNullOrEmpty(m_textBox?.Text))
            {
                return false;
            }

            var text = m_textBox.Text;
            m_textBox.Text = string.Empty;
            if (m_selectedItemsCopy.Contains(m_textBox.Text.ToLower()))
            {
                return true;
            }

            SelectedItems.Add(text);

            return true;
        }
        
        private void OnIsDropDownOpenChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var oldValue = (bool)e.OldValue;
            var value = (bool)e.NewValue;

            if (m_popup?.Child == null || value == oldValue)
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
                (s, ev) => FindAndSubscribeToLatestToggle(s, latestItem);
        }

        private void FindAndSubscribeToLatestToggle(object sender, ListBoxItem relatedListBoxItem)
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
                if (!(child is ToggleButton toggle) || toggle.Name != c_deleteItemToggleName)
                {
                    continue;
                }

                toggle.Checked += (o, args) => DeleteItem(o, relatedListBoxItem);
                m_toggleButtons.Add(toggle);
            }
        }

        private void ListBox_ItemsChanged()
        {
            m_selectedItemsCopy = new List<string>();
            foreach (var item in m_listBox.Items)
            {
                if (!(item is string stringItem))
                {
                    continue;
                }
                
                m_selectedItemsCopy.Add(stringItem.ToLower());
            }
        }

        private void SetTextBoxText()
        {
            if (m_isDropDownOpen || m_selectedItems == null)
            {
                m_textBox.Text = string.Empty;
                return;
            }

            m_textBox.Text = m_selectedItems.Count switch
            {
                0 => string.Empty,
                1 => m_selectedItems[0].ToString(),
                _ => $"{SelectedItems.Count.ToString()} {SuffixOnMany}"
            };
        }
    }
}
