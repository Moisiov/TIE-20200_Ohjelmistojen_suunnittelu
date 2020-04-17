using System.Collections.Generic;
using System.Linq;
using System;

namespace IlmatieteenLaitosAPI
{
    public static class HelperMethods
    {
        public static WeatherModel CalculateTimeSpanWeather(IEnumerable<WeatherModel> data)
        {
            try
            {
                var weatherModels = data as WeatherModel[] ?? data.ToArray();

                var averageWeather = new WeatherModel
                {
                    ObservationStartTime = weatherModels.Min(w => w.ObservationStartTime),
                    ObservationEndTime = weatherModels.Max(w => w.ObservationEndTime),
                    Location = weatherModels.Select(w => w.Location).FirstOrDefault(),
                    AirTemperatureAvg = weatherModels.Average(w => w.AirTemperatureAvg),
                    AirTemperatureMax = weatherModels.Max(w => w.AirTemperatureMax),
                    AirTemperatureMin = weatherModels.Min(w => w.AirTemperatureMin),
                    AirHumidityAvg = weatherModels.Average(w => w.AirHumidityAvg),
                    WindSpeedAvg = weatherModels.Average(w => w.WindSpeedAvg),
                    WindSpeedMin = weatherModels.Min(w => w.WindSpeedMin),
                    WindSpeedMax = weatherModels.Max(w => w.WindSpeedMax),
                    WindDirection = weatherModels.Average(w => w.WindDirection),
                    Precipitation = weatherModels.Sum(w => w.Precipitation),
                    PrecipitationIntensityMaximum = weatherModels.Max(w => w.PrecipitationIntensityMaximum),
                    AirPressureAvg = weatherModels.Average(w => w.AirPressureAvg),
                    MostSignificantWeatherCode = weatherModels
                        .GroupBy(w => w.MostSignificantWeatherCode)
                        .OrderByDescending(grp => grp.Count())
                        .Select(grp => grp.Key)
                        .FirstOrDefault()
                };

                return averageWeather;
            }
            catch (Exception e)
            {
                Console.WriteLine("IlmatieteenLaitosAPI: " + e.Message);
            }

            return null;
        }
    }
}
