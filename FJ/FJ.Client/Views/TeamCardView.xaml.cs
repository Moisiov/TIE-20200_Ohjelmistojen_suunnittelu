using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FJ.Client.Views
{
    public class TeamCardView : UserControl
    {
        public TeamCardView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
