using System;
using System.Collections;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.VisualTree;
using FJ.Utils;
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace FJ.Client.Core.UIElements
{
    /// <summary>
    /// Time picker with hard coded time format of hh:mm
    /// </summary>
    public class FJTimePicker : TemplatedControl
    {
        static FJTimePicker()
        {
            FocusableProperty.OverrideDefaultValue<FJTimePicker>(true);
            IsDropDownOpenProperty.Changed.AddClassHandler<FJTimePicker>(
                (x, e) => x.OnIsDropDownOpenChanged(e));
            
            KeyDownEvent.AddClassHandler<FJTimePicker>(
                (x, e) => x.OnKeyDown(e), RoutingStrategies.Tunnel);
            
            TextInputEvent.AddClassHandler<FJTimePicker>(
                (x, e) => x.OnTextInput(e), RoutingStrategies.Tunnel);
        }
        
        private const string c_textBoxName = "PART_TimePickerTextBox";
        private const string c_contentPopupName = "PART_TimePickerContentPopup";

        private TextBox m_textBox;
        private Popup m_contentPopup;

        private IDisposable m_textBoxChangedSub;
        private IDisposable m_subsOnOpen;
        
        private bool m_suspendItemFocusedHandler;
        private bool m_suspendTextBoxTextChangedHandler;
        private bool m_textBoxValueParsedAlready;

        private int? m_currentHourSelection;
        private int? m_currentMinuteSelection;

        #region DependencyProperties
        protected IEnumerable HourItems { get; } = new AvaloniaList<FJTimePickerItem>(
            CommonConstants.S_Hours.Select(x =>
                new FJTimePickerItem
                {
                    Content = x.ToString("00"),
                    TimeSelectionType = FJTimePickerItem.SelectionType.Hour
                }));
        public static readonly DirectProperty<FJTimePicker, IEnumerable> HourItemsProperty =
            AvaloniaProperty.RegisterDirect<FJTimePicker, IEnumerable>(
                nameof(HourItems),
                o => o.HourItems);
        
        protected IEnumerable MinuteItems { get; } = new AvaloniaList<FJTimePickerItem>(
            CommonConstants.S_Minutes.Select(x =>
                new FJTimePickerItem
                {
                    Content = x.ToString("00"),
                    TimeSelectionType = FJTimePickerItem.SelectionType.Minute
                }));

        public static readonly DirectProperty<FJTimePicker, IEnumerable> MinuteItemsProperty =
            AvaloniaProperty.RegisterDirect<FJTimePicker, IEnumerable>(
                nameof(MinuteItems),
                o => o.MinuteItems);
        
        private bool m_isDropDownOpen;
        
        /// <summary>
        /// Gets or sets a value indicating whether the dropdown is currently open.
        /// </summary>
        public bool IsDropDownOpen
        {
            get => m_isDropDownOpen;
            set => SetAndRaise(IsDropDownOpenProperty, ref m_isDropDownOpen, value);
        }
        public static readonly DirectProperty<FJTimePicker, bool> IsDropDownOpenProperty =
            AvaloniaProperty.RegisterDirect<FJTimePicker, bool>(
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
            AvaloniaProperty.Register<FJTimePicker, double>(nameof(MaxDropDownHeight), 200);
        
        /// <summary>
        /// Gets or set the watermark text that is displayed by the
        /// <see cref="T:FJ.Client.Core.UIElements.FJTimePicker"/> when empty.
        /// </summary>
        public string Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
        public static readonly StyledProperty<string> WatermarkProperty =
            TextBox.WatermarkProperty.AddOwner<FJTimePicker>();

        private TimeSpan? m_selectedTime;
        
        /// <summary>
        /// Gets or sets the selected time that is displayed by the
        /// <see cref="T:FJ.Client.Core.UIElements.FJTimePicker"/>.
        /// </summary>
        public TimeSpan? SelectedTime
        {
            get => m_selectedTime;
            set => SetAndRaise(SelectedTimeProperty, ref m_selectedTime, value);
        }

        public static readonly DirectProperty<FJTimePicker, TimeSpan?> SelectedTimeProperty =
            AvaloniaProperty.RegisterDirect<FJTimePicker, TimeSpan?>(
                nameof(SelectedTime),
                o => o.SelectedTime,
                (o, v) => o.SelectedTime = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true);
        #endregion

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            if (m_contentPopup != null)
            {
                m_contentPopup.Opened -= PopupOpened;
                m_contentPopup.Closed -= PopupClosed;
            }
            
            m_contentPopup = e.NameScope.Find<Popup>(c_contentPopupName);
            m_contentPopup.Opened += PopupOpened;
            m_contentPopup.Closed += PopupClosed;
            
            if (m_textBox != null)
            {
                m_textBox.KeyDown -= TextBox_KeyDown;
                m_textBox.GotFocus -= TextBox_GotFocus;
                m_textBoxChangedSub?.Dispose();
            }
            
            m_textBox = e.NameScope.Find<TextBox>(c_textBoxName);
            SetWaterMarkText();
            
            m_textBox.KeyDown += TextBox_KeyDown;
            m_textBox.GotFocus += TextBox_GotFocus;
            m_textBoxChangedSub = m_textBox.GetObservable(TextBox.TextProperty)
                .Subscribe(txt => TextBox_TextChanged());

            if (SelectedTime.HasValue)
            {
                m_currentHourSelection = SelectedTime.Value.Hours;
                m_currentMinuteSelection = SelectedTime.Value.Minutes;
                m_textBox.Text = TimeSpanToString(SelectedTime.Value);
            }

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
            
            var text = m_textBox.Text;
            if (text.IsNullOrEmpty())
            {
                return;
            }

            m_textBox.SelectionStart = 0;
            m_textBox.SelectionEnd = text.Length;
        }
        
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            SetSelectedTime();
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            if (e.Handled || m_textBox == null)
            {
                return;
            }

            var caret = m_textBox.CaretIndex;
            var prevText = m_textBox.Text ?? string.Empty;
            var inputString = prevText.Insert(caret, e.Text);
            var parsed = ParseInputString(inputString);
            m_textBoxValueParsedAlready = true;

            // Try to infer where caret should be
            int caretTargetIndex;
            switch (caret)
            {
                case 0 when m_currentMinuteSelection != null:
                case 1 when m_currentMinuteSelection != null:
                {
                    caretTargetIndex = parsed.IndexOf(':');
                    break;
                }
                
                default:
                {
                    caretTargetIndex = m_currentHourSelection == null ? 0 : parsed.Length;
                    break;
                }
            }

            m_textBox.Text = parsed;
            m_textBox.CaretIndex = caretTargetIndex;
            
            e.Handled = true;
        }

        internal void ItemFocused(FJTimePickerItem dropDownItem)
        {
            if (m_suspendItemFocusedHandler
                || !IsDropDownOpen
                || !dropDownItem.IsSelected
                || !dropDownItem.IsArrangeValid)
            {
                return;
            }

            switch (dropDownItem.TimeSelectionType)
            {
                case FJTimePickerItem.SelectionType.Hour:
                    m_currentHourSelection = int.Parse((string)dropDownItem.Content);
                    break;
                case FJTimePickerItem.SelectionType.Minute:
                    m_currentMinuteSelection = int.Parse((string)dropDownItem.Content);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // Set other selection as well if needed
            var selectionNeeded = false;
            if (!m_currentHourSelection.HasValue)
            {
                m_currentHourSelection = 0;
                selectionNeeded = true;
            }

            if (!m_currentMinuteSelection.HasValue)
            {
                m_currentMinuteSelection = 0;
                selectionNeeded = true;
            }
            
            SetSelectedTime();

            if (!selectionNeeded)
            {
                return;
            }

            m_suspendItemFocusedHandler = true;
            TryFocusSelectedValues();
            m_suspendItemFocusedHandler = false;
        }
        
        private void OnIsDropDownOpenChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var oldValue = (bool)e.OldValue;
            var value = (bool)e.NewValue;

            if (m_contentPopup?.Child == null || value == oldValue)
            {
                return;
            }

            m_contentPopup.IsOpen = value;
        }
        
        private void SetSelectedTime()
        {
            if (!m_currentHourSelection.HasValue || !m_currentMinuteSelection.HasValue)
            {
                if (m_textBox != null && !m_textBox.Text.IsNullOrEmpty())
                {
                    // TODO Loc
                    DataValidationErrors.SetErrors(this, "Virheellinen aika".ToMany());
                }
                else
                {
                    DataValidationErrors.ClearErrors(this);
                }
                
                if (SelectedTime != null)
                {
                    SelectedTime = null;
                }
                return;
            }
            
            var newT = new TimeSpan(m_currentHourSelection.Value, m_currentMinuteSelection.Value, 0);
            var newTimeSpanString = TimeSpanToString(newT);
            
            if (m_textBox != null && m_textBox.Text != newTimeSpanString)
            {
                m_suspendTextBoxTextChangedHandler = true;
                m_textBox.Text = newTimeSpanString;
                m_suspendTextBoxTextChangedHandler = false;

                m_textBox.CaretIndex = m_textBox.Text.Length;
            }
            
            if (SelectedTime.HasValue && SelectedTime.Value == newT)
            {
                return;
            }

            SelectedTime = newT;
            DataValidationErrors.ClearErrors(this);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = true;
        }
        
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            
            e.Handled = ProcessTimePickerKey(e);
        }
        
        private void TextBox_TextChanged()
        {
            var doParse = !m_textBoxValueParsedAlready;
            m_textBoxValueParsedAlready = false;
            
            if (m_textBox == null || m_suspendTextBoxTextChangedHandler)
            {
                return;
            }

            if (doParse && m_textBox.Text != null)
            {
                ParseInputString(m_textBox.Text);
            }

            if (m_textBox.Text.IsNullOrEmpty())
            {
                SetWaterMarkText();
            }

            m_suspendItemFocusedHandler = true;
            TryFocusSelectedValues();
            m_suspendItemFocusedHandler = false;
        }
        
        private bool ProcessTimePickerKey(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetSelectedTime();
            }
            else if (e.Key != Key.Down || (e.KeyModifiers & KeyModifiers.Control) != KeyModifiers.Control)
            {
                return false;
            }

            HandlePopup();
            return true;
        }
        
        private void HandlePopup()
        {
            if (IsDropDownOpen)
            {
                Focus();
                IsDropDownOpen = false;
                return;
            }
            
            SetSelectedTime();
            IsDropDownOpen = true;
            m_contentPopup?.Focus();
        }

        private void PopupOpened(object sender, EventArgs e)
        {
            TryFocusSelectedValues();

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

        private void TryFocusSelectedValues()
        {
            m_suspendItemFocusedHandler = true;
            
            foreach (FJTimePickerItem item in HourItems)
            {
                item.IsSelected = int.Parse((string)item.Content) == m_currentHourSelection;
            }
            foreach (FJTimePickerItem item in MinuteItems)
            {
                item.IsSelected = int.Parse((string)item.Content) == m_currentMinuteSelection;
            }

            m_suspendItemFocusedHandler = false;
        }

        private void SetWaterMarkText()
        {
            if (m_textBox == null)
            {
                return;
            }

            if (Watermark.IsNullOrEmpty())
            {
                m_textBox.Watermark = Watermark.IsNullOrEmpty() ? "hh:mm" : Watermark;
            }
            else
            {
                m_textBox.ClearValue(TextBox.WatermarkProperty);
            }
        }

        private string ParseInputString(string input)
        {
            var doAddSeparator = false;
            string hourFormat = null;
            string minuteFormat = null;

            var t = input.WithoutWhitespaces();
            if (t.IsNullOrEmpty())
            {
                m_currentHourSelection = null;
                m_currentMinuteSelection = null;
                return string.Empty;
            }

            var newParts = t.Split(':');
            var hours = newParts.First();

            if (hours.IsNullOrEmpty() || hours.Any(c => c != ':' && !c.IsDigit()))
            {
                m_currentHourSelection = null;
            }
            else
            {
                var hoursVal = (int?)int.Parse(hours);
                if (hoursVal > 23)
                {
                    m_currentHourSelection = null;
                }
                else
                {
                    m_currentHourSelection = hoursVal;
                    if (hoursVal > 2 || (hours.StartsWith('0') && hours.Length > 1))
                    {
                        doAddSeparator = true;
                        hourFormat = "00";
                    }
                }
            }

            if (newParts.Length > 1)
            {
                var minutes = newParts[1];
                if (minutes.IsNullOrEmpty() || minutes.Any(c => c != ':' && !c.IsDigit()))
                {
                    m_currentMinuteSelection = null;
                }
                else
                {
                    var minutesVal = (int?)int.Parse(minutes);
                    if (minutesVal > 59)
                    {
                        m_currentMinuteSelection = null;
                    }
                    else
                    {
                        m_currentMinuteSelection = minutesVal;
                        if (minutesVal > 5 || (minutes.StartsWith('0') && minutes.Length > 1))
                        {
                            minuteFormat = "00";
                        }
                    }

                    doAddSeparator |= m_currentMinuteSelection.HasValue;
                }
            }
            else
            {
                m_currentMinuteSelection = null;
            }

            return new StringBuilder()
                .Append(m_currentHourSelection?.ToString(hourFormat) ?? string.Empty)
                .Append(doAddSeparator ? ":" : string.Empty)
                .Append(m_currentMinuteSelection?.ToString(minuteFormat) ?? string.Empty)
                .ToString();
        }

        private static string TimeSpanToString(TimeSpan value)
        {
            return value.ToString(@"hh\:mm");
        }
    }

    public class FJTimePickerItem : ListBoxItem, IStyleable
    {
        public enum SelectionType
        {
            Hour,
            Minute
        }
        
        public Type StyleKey => typeof(ListBoxItem);
        public SelectionType TimeSelectionType { get; set; }

        public FJTimePickerItem()
        {
            this.GetObservable(IsSelectedProperty).Where(selected => selected)
                .Subscribe(_ => (TemplatedParent as FJTimePicker)?.ItemFocused(this));
        }
    }
}
