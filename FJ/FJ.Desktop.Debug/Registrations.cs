using System;
using FJ.Client.Services;
using Unity;

namespace FJ.Desktop.Debug
{
    public static class Registrations
    {
        public static void DoRegistrations(IUnityContainer container)
        {
            ClientRegistrations(container);
            ServicesRegistrations(container);
        }

        private static void ClientRegistrations(IUnityContainer container)
        {
            container.RegisterSingleton<IContentRegionNavigator, ContentRegionNavigator>();
        }

        private static void ServicesRegistrations(IUnityContainer container)
        {
            // TODO
        }
    }
}
