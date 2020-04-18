using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.Weather;
using IlmatieteenLaitosAPI;

namespace FJ.Services.Weather.WeatherDataFetchingServices
{
    public class IlmatieteenLaitosAPIDataFetchingService : IWeatherDataFetchingService
    {
        private readonly IIlmatieteenLaitosAPI m_api;

        public IlmatieteenLaitosAPIDataFetchingService(IIlmatieteenLaitosAPI api)
        {
            m_api = api;
        }
        
        public async Task<WeatherSearchResult> GetWeatherInfosAsync(WeatherSearchArgs args)
        {
            if (!args.SearchDateRange.NotEmpty || args.Locations?.Any() != true)
            {
                await Task.CompletedTask;
                
                // TODO could throw Exception as well
                return new WeatherSearchResult(null);
            }

            var searchDates = new List<DateTime>();
            if (!args.SearchDateRange.HasStartAndEnd)
            {
                // Never really throws but remove warnings
                searchDates.Add(args.SearchDateRange.Start
                                ?? (args.SearchDateRange.End
                                    ?? throw new ArgumentException(nameof(args))));
            }
            else
            {
                var startDate = args.SearchDateRange.Start ?? DateTime.MinValue;  // Never really null
                var endDate = args.SearchDateRange.End ?? DateTime.MaxValue;  // Never really null
                
                // https://stackoverflow.com/a/23787691
                searchDates.AddRange(Enumerable.Range(0, (startDate - endDate).Days + 1)
                    .Select(d => startDate.AddDays(d)));
            }

            var searchTasks = new List<Task<WeatherCollection>>();
            if (args.DailyTimeRange.NotEmpty)
            {
                var startTime = args.DailyTimeRange.Start ?? TimeSpan.Zero;
                var endTime = args.DailyTimeRange.End ?? new TimeSpan(23, 59, 59);

                foreach (var date in searchDates)
                {
                    var start = date.Date + startTime;
                    var end = date.Date + endTime;
                    
                    searchTasks.AddRange(args.Locations
                        .Select(l => m_api.GetHourlyWeather(l, start, end)));
                }
            }
            else
            {
                foreach (var location in args.Locations)
                {
                    searchTasks.AddRange(searchDates
                        .Select(d => m_api.GetHourlyWeatherOfDay(location, d.Date)));
                }
            }

            var results = new List<WeatherInfo>();
            while (searchTasks.Count > 0)
            {
                var searchTask = await Task.WhenAny(searchTasks);
                searchTasks.Remove(searchTask);
                var completedSearch = await searchTask;
                results.AddRange(completedSearch.WeatherData
                    .Select(wm => wm.ToWeatherInfo()));
            }
            
            return new WeatherSearchResult(results);
        }

        public async Task<WeatherInfo> GetWeatherAvgAsync(string location, DateTime startTime, DateTime endTime)
        {
            var res = await m_api.GetWeather(location, startTime, endTime);
            return res.ToWeatherInfo();
        }

        public async Task<WeatherInfo> GetCurrentWeatherAsync(string location)
        {
            var res = await m_api.GetWeatherNow(location);
            return res.ToWeatherInfo();
        }
    }

    internal static class IlmatieteenLaitosAPIExtensions
    {
        internal static WeatherInfo ToWeatherInfo(this WeatherModel wm)
        {
            return new WeatherInfo
            {
                ObservationPeriod = new DateRange(wm.ObservationStartTime, wm.ObservationEndTime),
                Location = wm.Location,
                AirTemperature = wm.AirTemperature,
                WindSpeed = wm.WindSpeedAvg,
                Precipitation = wm.Precipitation,
                MostSignificantWeatherDescription = wm.WeatherDescription
            };
        }
    }
}
