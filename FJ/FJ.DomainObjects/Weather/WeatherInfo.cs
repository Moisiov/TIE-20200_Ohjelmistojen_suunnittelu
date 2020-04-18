using System;

namespace FJ.DomainObjects.Weather
{
    public class WeatherInfo
    {
        public DateRange ObservationPeriod { get; set; }
        public string Location { get; set; }
        public double? AirTemperature { get; set; }
        public double? WindSpeed { get; set; }
        public double? Precipitation { get; set; }
        public string MostSignificantWeatherDescription { get; set; }
    }
}
