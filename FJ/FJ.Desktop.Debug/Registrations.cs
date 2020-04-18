using System;
using FinlandiaHiihtoAPI;
using FJ.Client.Core.Services;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.CoreServices;
using FJ.Services.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;
using FJ.Utils;
using IlmatieteenLaitosAPI;
using PlotterService;
using Unity;
using Unity.Lifetime;

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
            container.RegisterInstance<IPlotService>(new PlotService());
        }

        private static void ServicesInternalRegistrations(IUnityContainer container)
        {
            // TODO not sure if this should be registered as instance, even though it efficient
            container.RegisterInstance<ICacheProvider>(new MemoryCacheProvider());
            
            container.RegisterSingleton<IDataFetchingService, FinlandiaAPIDataFetchingService>();
            container.DecorateSingleton<IDataFetchingService, SimpleDataFetcherCacheDecorator>();
            
            /*
            Leaving this here for now as this alternative might be a safer choice. -Olli
            https://stackoverflow.com/a/36101994
            
            container.RegisterSingleton<IDataFetchingService, FinlandiaAPISimpleCacheDecorator>(
                new InjectionConstructor(
                    new ResolvedParameter<FinlandiaAPIDataFetchingService>(),
                    new ResolvedParameter<ICacheProvider>()));
                    
            TODO Remove if extension is proved to be worthy.
            */

            container.RegisterInstance<IFilterImplementationProvider>(new FilterImplementationProvider());
        }

        private static void ServicesExternalRegistrations(IUnityContainer container)
        {
            // APIs are treated as externals
            container.RegisterInstance<IFinlandiaHiihtoAPI>(new FinlandiaHiihtoAPI.FinlandiaHiihtoAPI());
            container.RegisterInstance<IIlmatieteenLaitosAPI>(new IlmatieteenLaitosAPI.IlmatieteenLaitosAPI());
        }

        private static void ServiceInterfacesRegistrations(IUnityContainer container)
        {
            container.RegisterType<IFinlandiaResultsService, FinlandiaResultsService>();
            container.RegisterType<IAthleteResultsService, AthleteResultsDataService>();
            container.RegisterType<ICompetitionOccasionDataService, CompetitionOccasionDataService>();
            container.RegisterType<ICompetitionComparisonDataService, CompetitionComparisonDataService>();
        }
    }
}
