using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FJ.Client.Views
{
    public class ControlPanelView : UserControl
    {
        public ControlPanelView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
