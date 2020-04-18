using System;
using System.Threading.Tasks;
using FJ.DomainObjects.Weather;

namespace FJ.Services.Weather.WeatherDataFetchingServices
{
    public interface IWeatherDataFetchingService
    {
        Task<WeatherSearchResult> GetWeatherInfosAsync(WeatherSearchArgs args);
        Task<WeatherInfo> GetCurrentWeatherAsync(string location);
    }
}
