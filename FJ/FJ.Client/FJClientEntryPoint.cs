using System;
using Avalonia;
using Avalonia.Dialogs;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using Unity;

namespace FJ.Client
{
    public static class FJClientEntryPoint
    {
        [STAThread]
        public static void RunApplication(IUnityContainer container, string[] args)
        {
            var avaloniaAppBuilder = BuildAvaloniaApp();
            // TODO inject container
            avaloniaAppBuilder.StartWithClassicDesktopLifetime(args);
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
