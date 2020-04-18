using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.Weather;
using FJ.ServiceInterfaces.Weather;
using FJ.Services.Weather.WeatherDataFetchingServices;
using IlmatieteenLaitosAPI;

namespace FJ.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherDataFetchingService m_weatherDataFetchingService;
        
        public WeatherService(IWeatherDataFetchingService weatherDataFetchingService)
        {
            m_weatherDataFetchingService = weatherDataFetchingService;
        }
        
        public async Task<WeatherSearchResult> GetWeatherInfosAsync(WeatherSearchArgs args)
        {
            return await m_weatherDataFetchingService.GetWeatherInfosAsync(args);
        }

        public Task<WeatherInfo> GetCurrentWeatherAsync(string location)
        {
            throw new NotImplementedException();
        }
    }
}
