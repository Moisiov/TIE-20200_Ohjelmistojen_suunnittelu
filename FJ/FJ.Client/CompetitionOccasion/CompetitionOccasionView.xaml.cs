using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FJ.Client.CompetitionOccasion
{
    public class CompetitionOccasionView : UserControl
    {
        public CompetitionOccasionView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
