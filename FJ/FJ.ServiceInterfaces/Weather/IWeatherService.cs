using System;
using System.Threading.Tasks;
using FJ.DomainObjects.Weather;

namespace FJ.ServiceInterfaces.Weather
{
    public interface IWeatherService
    {
        Task<WeatherSearchResult> GetWeatherInfosAsync(WeatherSearchArgs args);
        Task<WeatherInfo> GetCurrentWeatherAsync(string location);
    }
}
