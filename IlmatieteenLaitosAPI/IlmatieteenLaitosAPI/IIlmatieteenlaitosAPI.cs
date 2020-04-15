using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IlmatieteenLaitosAPI
{
    public interface IIlmatieteenLaitosAPI
    {
        /// <summary>
        /// Get hourly weather observations.
        /// </summary>
        /// <param name="location">Observation location</param>
        /// <param name="start">Timespan start</param>
        /// <param name="end">Timespan end</param>
        /// <returns>Collection of WeatherModels</returns>
        Task<WeatherCollection> GetHourlyWeather(string location, DateTime start, DateTime end);

        /// <summary>
        /// Get hourly weather of day.
        /// </summary>
        /// <param name="location">Observation location</param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="date"></param>
        /// <returns>Collection of WeatherModels</returns>
        Task<WeatherCollection> GetHourlyWeatherOfDay(string location, int year, int month, int date);

        /// <summary>
        /// Get hourly weather of day.
        /// </summary>
        /// /// <param name="location">Observation location</param>
        /// <param name="dateTime">Only date matters. Time is not taken into account.</param>
        /// <returns>Collection of WeatherModels</returns>
        Task<WeatherCollection> GetHourlyWeatherOfDay(string location, DateTime dateTime);

        /// <summary>
        /// Get weather of given timespan.
        /// </summary>
        /// <param name="location">Observation location</param>
        /// <param name="start">Timespan start</param>
        /// <param name="end">Timespan end</param>
        Task<WeatherModel> GetWeather(string location, DateTime start, DateTime end);

        /// <summary>
        /// Get weather of given day.
        /// </summary>
        /// <param name="location">Observation location</param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="date"></param>
        Task<WeatherModel> GetWeatherOfDay(string location, int year, int month, int date);

        /// <summary>
        /// Get weather of given day.
        /// </summary>
        /// <param name="location">Observation location</param>
        /// <param name="dateTime">Only date matters. Time is not taken into account.</param>
        Task<WeatherModel> GetWeatherOfDay(string location, DateTime dateTime);
    }
}