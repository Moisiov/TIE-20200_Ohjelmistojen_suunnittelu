using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FJ.Client.CompetitionComparison
{
    public class CompetitionComparisonView : UserControl
    {
        public CompetitionComparisonView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
