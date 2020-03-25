using System;
using FJ.Client.Core.Services;
using FJ.FinlandiaHiihtoAPI;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.CoreServices;
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
            ServicesExternalRegistrations(container);
            ServiceInterfacesRegistrations(container);
        }

        private static void ClientInternalRegistrations(IUnityContainer container)
        {
            container.RegisterSingleton<IContentRegionNavigator, ContentRegionNavigator>();
            container.RegisterSingleton<IControlPanelRegionController, ControlPanelRegionController>();
        }

        private static void ServicesInternalRegistrations(IUnityContainer container)
        {
            // TODO not sure if this should be registered as instance, even though it efficient
            container.RegisterInstance<ICacheProvider>(new MemoryCacheProvider());
            
            container.RegisterSingleton<IDataFetchingService, FinlandiaAPIDataFetchingService>();
        }

        private static void ServicesExternalRegistrations(IUnityContainer container)
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
