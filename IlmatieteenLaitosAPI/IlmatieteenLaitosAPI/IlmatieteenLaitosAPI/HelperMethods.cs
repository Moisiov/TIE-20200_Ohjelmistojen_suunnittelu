using System.Collections.Generic;
using System.Linq;

namespace IlmatieteenLaitosAPI
{
    public static class HelperMethods
    {
        public static WeatherModel CalculateTimeSpanWeather(IEnumerable<WeatherModel> data)
        {
            WeatherModel averageWeather = new WeatherModel()
            {
                ObservationStartTime = data.Min(w => w.ObservationStartTime),
                ObservationEndTime = data.Max(w => w.ObservationEndTime),
                Location = data.Select(w => w.Location).FirstOrDefault(),
                AirTemperatureAvg = data.Average(w => w.AirTemperatureAvg),
                AirTemperaturMax = data.Max(w => w.AirTemperaturMax),
                AirTemperatureMin = data.Min(w => w.AirTemperatureMin),
                AirHumidityAvg = data.Average(w => w.AirHumidityAvg),
                WindSpeedAvg = data.Average(w => w.WindSpeedAvg),
                WindSpeedMin = data.Min(w => w.WindSpeedMin),
                WindSpeedMax = data.Max(w => w.WindSpeedMax),
                WindGustSpeedMax = data.Max(w => w.WindGustSpeedMax),
                RainAccumulated = data.Sum(w => w.RainAccumulated),
                RainIntensityMaximum = data.Max(w => w.RainIntensityMaximum),
                AirPressure = data.Average(w => w.AirPressure),
                MostSignificantWeatherCode = data.GroupBy(w => w.MostSignificantWeatherCode).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).FirstOrDefault()
            };

            return averageWeather;
        }
    }
}
