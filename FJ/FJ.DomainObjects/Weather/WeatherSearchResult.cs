using System;
using System.Collections.Generic;
using System.Linq;

namespace FJ.DomainObjects.Weather
{
    public class WeatherSearchResult
    {
        public IEnumerable<WeatherInfo> WeatherInfos { get; set; }
        
        public int ResultsCount => WeatherInfos?.Count() ?? 0;
        public bool HasAnyResults => WeatherInfos?.Any() == true;

        public WeatherSearchResult()
            : this(null)
        {
        }

        public WeatherSearchResult(IEnumerable<WeatherInfo> weatherInfos)
        {
            WeatherInfos = weatherInfos ?? new WeatherInfo[] { };
        }
    }
}
