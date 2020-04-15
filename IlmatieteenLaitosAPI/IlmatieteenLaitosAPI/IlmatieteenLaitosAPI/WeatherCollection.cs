using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IlmatieteenLaitosAPI
{
    public class WeatherCollection
    {
        public IEnumerable<WeatherModel> WeatherData { get; set; }

        public WeatherCollection(IEnumerable<WeatherModel> weatherData)
        {
            WeatherData = weatherData ?? new WeatherModel[] { };
        }
    }
}
