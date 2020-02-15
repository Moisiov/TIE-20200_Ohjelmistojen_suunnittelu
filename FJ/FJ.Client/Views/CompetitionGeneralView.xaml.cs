using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FJ.Client.Views
{
    public class CompetitionGeneralView : UserControl
    {
        public CompetitionGeneralView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
