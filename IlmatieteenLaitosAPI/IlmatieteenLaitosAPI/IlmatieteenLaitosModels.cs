using System;
using System.Collections.Generic;

namespace IlmatieteenLaitosAPI
{
    public class WeatherModel
    {
        public DateTime ObservationStartTime { get; set; }
        public DateTime ObservationEndTime { get; set; }
        public string Location { get; set; }
        public double? AirTemperatureAvg { get; set; }
        public double? AirTemperatureMax { get; set; }
        public double? AirTemperatureMin { get; set; }

        /// <summary>
        /// Air humidity percentage.
        /// </summary>
        public double? AirHumidityAvg { get; set; }

        /// <summary>
        /// Average wind speed (m/s).
        /// </summary>
        public double? WindSpeedAvg { get; set; }

        /// <summary>
        /// Wind speed minimum, 10 minute average (m/s).
        /// </summary>
        public double? WindSpeedMin { get; set; }

        /// <summary>
        /// Wind speed maximum, 10 minute average (m/s).
        /// </summary>
        public double? WindSpeedMax { get; set; }

        /// <summary>
        /// Wind gust maximum, 3 second average (m/s).
        /// </summary>
        public double? WindGustSpeedMax { get; set; }

        /// <summary>
        /// Rain accumulated (mm).
        /// </summary>
        public double? RainAccumulated { get; set; }

        /// <summary>
        /// Rain intensity maximum (mm).
        /// </summary>
        public double? RainIntensityMaximum { get; set; }

        /// <summary>
        /// Air pressure (hPa).
        /// </summary>
        public double? AirPressure { get; set; }

        /// <summary>
        /// Weather code is used to describe weather.
        /// </summary>
        /// <see href="https://www.ilmatieteenlaitos.fi/latauspalvelun-pikaohje"/>
        public double? MostSignificantWeatherCode { get; set; }
    }

    public class APIRequestModel
    {
        public string BaseUrl { get; set; }
        public string DateTimeFormat { get; set; }
        public APIRequestParameterModel Parameters { get; set; }
    }

    public class APIRequestParameterModel
    {
        public string Service { get; set; }
        public string Version { get; set; }
        public string Request { get; set; }
        public string StoredQueryId { get; set; }
        public Dictionary<string, string> SearchParameters { get; set; }
    }
}
