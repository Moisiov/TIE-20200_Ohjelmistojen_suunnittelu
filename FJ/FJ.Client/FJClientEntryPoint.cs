using System;
using Avalonia;
using Avalonia.Dialogs;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using FJ.Client.UIUtils;
using Unity;

namespace FJ.Client
{
    public static class FJClientEntryPoint
    {
        [STAThread]
        public static void RunApplication(IUnityContainer container, string[] args)
        {
            StartupContainerHelper.SetStartupContainer(container);

            var avaloniaAppBuilder = BuildAvaloniaApp();

            try
            {
                avaloniaAppBuilder.StartWithClassicDesktopLifetime(args);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        private static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI()
                .UseManagedSystemDialogs();
    }
}
