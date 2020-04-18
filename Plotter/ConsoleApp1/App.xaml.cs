using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Prism.Ioc;
using Prism.Unity;

namespace ConsoleApp1
{
    public class App : PrismApplication
    {
        private MainWindowViewModel m_mwViewModel;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }
        
        protected override IAvaloniaObject CreateShell()
        {
            var mw = Container.Resolve<MainWindow>();
            m_mwViewModel = mw.DataContext as MainWindowViewModel;
            
            return mw;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
        }
        
        static void Main(string[] args)
        {
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .StartWithClassicDesktopLifetime(args);
        }

    }
}
