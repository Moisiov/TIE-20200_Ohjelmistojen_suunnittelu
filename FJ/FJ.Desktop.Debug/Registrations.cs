using System;
using FJ.Client.UIServices;
using FJ.FinlandiaHiihtoAPI;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;
using Unity;

namespace FJ.Desktop.Debug
{
    public static class Registrations
    {
        public static void DoRegistrations(IUnityContainer container)
        {
            ClientInternalRegistrations(container);
            ServicesInternalRegistrations(container);
            ServicesExternalRegistratios(container);
            ServiceInterfacesRegistrations(container);
        }

        private static void ClientInternalRegistrations(IUnityContainer container)
        {
            container.RegisterSingleton<IContentRegionNavigator, ContentRegionNavigator>();
        }

        private static void ServicesInternalRegistrations(IUnityContainer container)
        {
            container.RegisterSingleton<IDataFetchingService, FinlandiaAPIDataFetchingService>();
        }

        private static void ServicesExternalRegistratios(IUnityContainer container)
        {
            // APIs are treated as externals
            container.RegisterInstance<IFinlandiaHiihtoAPI>(new FinlandiaHiihtoAPI.FinlandiaHiihtoAPI());
        }

        private static void ServiceInterfacesRegistrations(IUnityContainer container)
        {
            container.RegisterType<ILatestFinlandiaResultsService, LatestFinlandiaResultsService>();
        }
    }
}
