using System;
using System.Threading.Tasks;
using FJ.DomainObjects.Weather;

namespace FJ.Services.Weather.WeatherDataFetchingServices
{
    public interface IWeatherDataFetchingService
    {
        Task<WeatherSearchResult> GetWeatherInfosAsync(WeatherSearchArgs args);
        Task<WeatherInfo> GetWeatherAvgAsync(string location, DateTime startTime, DateTime endTime);
        Task<WeatherInfo> GetCurrentWeatherAsync(string location);
    }
}
