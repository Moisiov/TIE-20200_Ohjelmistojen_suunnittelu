using System;
using System.Collections.Generic;

namespace IlmatieteenLaitosAPI
{
    public class WeatherCollection
    {
        public IEnumerable<WeatherModel> WeatherData { get; set; }

        public WeatherCollection()
            : this(null)
        {
        }

        public WeatherCollection(IEnumerable<WeatherModel> weatherData)
        {
            WeatherData = weatherData ?? new WeatherModel[] { };
        }
    }
}
