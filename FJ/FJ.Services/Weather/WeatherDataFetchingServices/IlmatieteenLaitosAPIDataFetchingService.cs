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
        private IIlmatieteenLaitosAPI m_api;

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
                MostSignificantWeatherDescription = wm.MostSignificantWeatherCode.ToWeatherDescription()
            };
        }

        private static string ToWeatherDescription(this double? d)
        {
            // TODO
            return d?.ToString() ?? string.Empty;
        }
    }
}

/*
00 Ei merkittäviä sääilmiöitä (minkään alla olevan WaWa-koodin ehdot eivät täyty)
04 Auerta, savua tai ilmassa leijuvaa pölyä ja näkyvyys vähintään 1 km
05 Auerta, savua tai ilmassa leijuvaa pölyä ja näkyvyys alle 1 km
10 Utua

Koodeja 20-25 käytetään, kun on ollut sadetta tai sumua edellisen tunnin aikana mutta ei enää havaintohetkellä.

20 Sumua
21 Sadetta (olomuoto on määrittelemätön)
22 Tihkusadetta (ei jäätävää) tai lumijyväsiä
23 Vesisadetta (ei jäätävää)
24 Lumisadetta
25 Jäätävää vesisadetta tai jäätävää tihkua

Seuraavia koodeja käytetään, kun sadetta tai sumua on havaittu havaintohetkellä.

30 – SUMUA
31 Sumua tai jääsumua erillisinä hattaroina
32 Sumua tai jääsumua, joka on ohentunut edellisen tunnin aikana
33 Sumua tai jääsumua, jonka tiheydessä ei ole tapahtunut merkittäviä muutoksia edellisen tunnin aikana
34 Sumua tai jääsumua, joka on muodostunut tai tullut sakeammaksi edellisen tunnin aikana
40 SADETTA (olomuoto on määrittelemätön)
41 Heikkoa tai kohtalaista sadetta (olomuoto on määrittelemätön)
42 Kovaa sadetta (olomuoto on määrittelemätön)
50 TIHKUSADETTA (heikkoa, ei jäätävää)
51 Heikkoa tihkua, joka ei ole jäätävää
52 Kohtalaista tihkua, joka ei ole jäätävää
53 Kovaa tihkua, joka ei ole jäätävää
54 Jäätävää heikkoa tihkua
55 Jäätävää kohtalaista tihkua
56 Jäätävää kovaa tihkua
60 VESISADETTA (heikkoa, ei jäätävää)
61 Heikkoa vesisadetta, joka ei ole jäätävää
62 Kohtalaista vesisadetta, joka ei ole jäätävää
63 Kovaa vesisadetta, joka ei ole jäätävää
64 Jäätävää heikkoa vesisadetta
65 Jäätävää kohtalaista vesisadetta
66 Jäätävää kovaa vesisadetta
67 Heikkoa lumensekaista vesisadetta tai tihkua (räntää)
68 Kohtalaista tai kovaa lumensekaista vesisadetta tai tihkua (räntää)
70 LUMISADETTA
71 Heikkoa lumisadetta
72 Kohtalaista lumisadetta
73 Tiheää lumisadetta
74 Heikkoa jääjyvässadetta
75 Kohtalaista jääjyväsadetta
76 Kovaa jääjyväsadetta
77 Lumijyväsiä
78 Jääkiteitä
80 KUUROJA TAI AJOITTAISTA SADETTA (heikkoja)
81 Heikkoja vesikuuroja
82 Kohtalaisia vesikuuroja
83 Kovia vesikuuroja
84 Ankaria vesikuuroja (>32 mm/h)
85 Heikkoja lumikuuroja
86 Kohtalaisia lumikuuroja
87 Kovia lumikuuroja
89 Raekuuroja mahdollisesti yhdessä vesi- tai räntäsateen kanssa
*/
