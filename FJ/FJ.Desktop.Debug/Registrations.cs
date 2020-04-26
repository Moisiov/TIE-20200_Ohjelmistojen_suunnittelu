using System;
using FinlandiaHiihtoAPI;
using FJ.Client.Core.Services;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.ServiceInterfaces.Weather;
using FJ.Services.CoreServices;
using FJ.Services.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;
using FJ.Services.Weather;
using FJ.Services.Weather.WeatherDataFetchingServices;
using FJ.Utils;
using IlmatieteenLaitosAPI;
using PlotterService;
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
            container.RegisterInstance<IPlotService>(new PlotService());
        }

        private static void ServicesInternalRegistrations(IUnityContainer container)
        {
            container.RegisterInstance<ICacheProvider>(new MemoryCacheProvider());
            
            container.RegisterType<IDataFetchingService, FinlandiaAPIDataFetchingService>();
            container.Decorate<IDataFetchingService, SimpleDataFetcherCacheDecorator>();
            container.Decorate<IDataFetchingService, SimpleDataFetcherDebugLoggerDecorator>();
            
            container.RegisterType<IWeatherDataFetchingService, IlmatieteenLaitosAPIDataFetchingService>();

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
            container.RegisterType<ICompetitionDataService, CompetitionDataService>();
            container.RegisterType<IWeatherService, WeatherService>();
        }
    }
}
