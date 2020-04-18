using System;
using System.Collections.Generic;

namespace IlmatieteenLaitosAPI
{
    public class WeatherModel
    {
        public DateTime ObservationStartTime { get; set; }
        public DateTime ObservationEndTime { get; set; }
        public string Location { get; set; }
        public double? AirTemperature { get; set; }
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
        /// Average wind direction (deg).
        /// </summary>
        public double? WindDirection { get; set; }

        /// <summary>
        /// Rain amount accumulated (mm).
        /// </summary>
        public double? Precipitation { get; set; }

        /// <summary>
        /// Rain amount intensity maximum (mm/h).
        /// </summary>
        public double? PrecipitationIntensityMaximum { get; set; }

        /// <summary>
        /// Average air pressure (hPa).
        /// </summary>
        public double? AirPressureAvg { get; set; }

        /// <summary>
        /// Weather code is used to describe weather.
        /// </summary>
        /// <see href="https://www.ilmatieteenlaitos.fi/latauspalvelun-pikaohje"/>
        public string MostSignificantWeatherCode { get; set; }
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
