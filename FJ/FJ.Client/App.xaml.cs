using Avalonia;
using Avalonia.Markup.Xaml;
using FJ.Client.UIUtils;
using FJ.Client.Views;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using Prism.Unity.Ioc;
using Unity;

namespace FJ.Client
{
    public class App : PrismApplication
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();

            // Initial navigations
            var regionManager = Container.Resolve<Prism.Regions.IRegionManager>();
            regionManager.NavigateControlPanelTo(nameof(ControlPanelView));
            regionManager.NavigateTopBarTo(nameof(TopBarView));
            regionManager.NavigateContentTo(nameof(FrontPageView));
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
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            CustomUIRegistrations.AutoRegisterViewsforNavigation(containerRegistry);
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(CustomResolvers.ViewToViewModelResolver);
        }
    }
}
