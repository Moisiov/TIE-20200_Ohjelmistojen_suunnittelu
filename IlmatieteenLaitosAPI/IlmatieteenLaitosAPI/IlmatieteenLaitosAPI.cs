using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IlmatieteenLaitosAPI
{
    public class IlmatieteenLaitosAPI : IIlmatieteenLaitosAPI
    {
        private readonly IlmatieteenLaitosDataFetcher m_dataFetcher = new IlmatieteenLaitosDataFetcher();

        public async Task<WeatherCollection> GetHourlyWeather(string location, DateTime start, DateTime end)
        {
            var result = await m_dataFetcher.FetchWeather(location, start, end);
            return new WeatherCollection(result);
        }

        public async Task<WeatherCollection> GetHourlyWeatherOfDay(string location, int year, int month, int date)
        {
            var start = new DateTime(year, month, date, 1, 0, 0, new CultureInfo("fi-FI").Calendar);
            var end = start.AddDays(1);

            var result = await m_dataFetcher.FetchWeather(location, start, end);
            return new WeatherCollection(result);
        }
        public async Task<WeatherCollection> GetHourlyWeatherOfDay(string location, DateTime dateTime)
        {
            var start = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 1, 0, 0, new CultureInfo("fi-FI").Calendar);
            var end = start.AddDays(1);

            var result = await m_dataFetcher.FetchWeather(location, start, end);
            return new WeatherCollection(result);
        }

        public async Task<WeatherModel> GetWeather(string location, DateTime start, DateTime end)
        {
            var hourlyWeather = await m_dataFetcher.FetchWeather(location, start, end);
            return HelperMethods.CalculateTimeSpanWeather(hourlyWeather);
        }

        public async Task<WeatherModel> GetWeatherOfDay(string location, int year, int month, int date)
        {
            var start = new DateTime(year, month, date, 1, 0, 0, new CultureInfo("fi-FI").Calendar);
            var end = start.AddDays(1);

            var hourlyWeather = await m_dataFetcher.FetchWeather(location, start, end);
            return HelperMethods.CalculateTimeSpanWeather(hourlyWeather);
        }

        public async Task<WeatherModel> GetWeatherOfDay(string location, DateTime dateTime)
        {
            var start = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 1, 0, 0, new CultureInfo("fi-FI").Calendar);
            var end = start.AddDays(1);

            var hourlyWeather = await m_dataFetcher.FetchWeather(location, start, end);
            return HelperMethods.CalculateTimeSpanWeather(hourlyWeather);
        }

        public async Task<WeatherModel> GetWeatherNow(string location)
        {
            var result = await m_dataFetcher.FetchWeather(location, DateTime.Now.AddMinutes(-10), DateTime.Now, false);
            return result.Last();
        }
    }
}
