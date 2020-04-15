using Avalonia;
using Avalonia.Markup.Xaml;
using FJ.Client.ControlPanel;
using FJ.Client.Core.IoC;
using FJ.Client.Core.Services;
using FJ.Client.FrontPage;
using FJ.Client.MainWindow;
using FJ.Client.TopBar;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using Prism.Unity.Ioc;
using Unity;

namespace FJ.Client
{
    public class App : PrismApplication
    {
        private MainWindowViewModel m_mwViewModel;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();

            m_mwViewModel?.OnFrameworkInitializationCompleted();
            SetupStartupViews();
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            IUnityContainer containerFromStartup = StartupContainerHelper.StartupContainer;

            return containerFromStartup == null
                ? new UnityContainerExtension()
                : new UnityContainerExtension(containerFromStartup);
        }

        protected override IAvaloniaObject CreateShell()
        {
            var mw = Container.Resolve<MainWindow.MainWindow>();
            m_mwViewModel = mw.DataContext as MainWindowViewModel;
            
            return mw;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            CustomUIRegistrations.AutoRegisterViewsForNavigation(containerRegistry);
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(CustomResolvers.ViewToViewModelResolver);
        }

        private void SetupStartupViews()
        {
            var regionManager = Container.Resolve<Prism.Regions.IRegionManager>();

            regionManager.NavigateControlPanelTo(nameof(ControlPanelView));
            regionManager.NavigateTopBarTo(nameof(TopBarView));
            regionManager.NavigateContentTo(nameof(FrontPageView));
        }
    }
}
