using System.Collections.Generic;
using System.Linq;
using System;

namespace IlmatieteenLaitosAPI
{
    public static class HelperMethods
    {
        private static Dictionary<int, string> m_weatherCodeDescriptions = new Dictionary<int, string>()
        {
            { 00, "Ei merkittäviä sääilmiöitä." },
            { 04, "Auerta, savua tai ilmassa leijuvaa pölyä ja näkyvyys vähintään 1 km." },
            { 05, "Auerta, savua tai ilmassa leijuvaa pölyä ja näkyvyys alle 1 km." },
            { 10, "Utua." },
            { 20, "Sumua." },
            { 21, "Sadetta (olomuoto on määrittelemätön)." },
            { 22, "Tihkusadetta (ei jäätävää) tai lumijyväsiä." },
            { 23, "Vesisadetta (ei jäätävää)." },
            { 24, "Lumisadetta." },
            { 25, "Jäätävää vesisadetta tai jäätävää tihkua." },
            { 30, "Sumua." },
            { 31, "Sumua tai jääsumua erillisinä hattaroina." },
            { 32, "Sumua tai jääsumua, joka on ohentunut edellisen tunnin aikana." },
            { 33, "Sumua tai jääsumua, jonka tiheydessä ei ole tapahtunut merkittäviä muutoksia edellisen tunnin aikana." },
            { 34, "Sumua tai jääsumua, joka on muodostunut tai tullut sakeammaksi edellisen tunnin aikana." },
            { 40, "Sadetta." },
            { 41, "Heikkoa tai kohtalaista sadetta." },
            { 42, "Kovaa sadetta." },
            { 50, "Tihkusadetta." },
            { 51, "Heikkoa tihkua, joka ei ole jäätävää." },
            { 52, "Kohtalaista tihkua, joka ei ole jäätävää." },
            { 53, "Kovaa tihkua, joka ei ole jäätävää." },
            { 54, "Jäätävää heikkoa tihkua." },
            { 55, "Jäätävää kohtalaista tihkua." },
            { 56, "Jäätävää kovaa tihkua." },
            { 60, "Vesisadetta." },
            { 61, "Heikkoa vesisadetta, joka ei ole jäätävää." },
            { 62, "Kohtalaista vesisadetta, joka ei ole jäätävää." },
            { 63, "Kovaa vesisadetta, joka ei ole jäätävää." },
            { 64, "Jäätävää heikkoa vesisadetta." },
            { 65, "Jäätävää kohtalaista vesisadetta." },
            { 66, "Jäätävää kovaa vesisadetta." },
            { 67, "Heikkoa lumensekaista vesisadetta tai tihkua (räntää)." },
            { 68, "Kohtalaista tai kovaa lumensekaista vesisadetta tai tihkua (räntää)." },
            { 70, "Lumisadetta." },
            { 71, "Heikkoa lumisadetta." },
            { 72, "Kohtalaista lumisadetta." },
            { 73, "Tiheää lumisadetta." },
            { 74, "Heikkoa jääjyvässadetta." },
            { 75, "75 Kohtalaista jääjyväsadetta." },
            { 76, "Kovaa jääjyväsadetta." },
            { 77, "Lumijyväsiä." },
            { 78, "Jääkiteitä." },
            { 80, "Kuuroja tai ajoittaista sadetta (heikkoja)." },
            { 81, "Heikkoja vesikuuroja." },
            { 82, "Kohtalaisia vesikuuroja." },
            { 83, "Kovia vesikuuroja." },
            { 84, "Ankaria vesikuuroja (>32 mm/h)." },
            { 85, "Heikkoja lumikuuroja." },
            { 86, "Kohtalaisia lumikuuroja." },
            { 87, "Kovia lumikuuroja." },
            { 89, "Raekuuroja mahdollisesti yhdessä vesi- tai räntäsateen kanssa." }
        };

        public static string GetWeatherDescription(double? weatherCode)
        {
            var description = string.Empty;

            if (weatherCode != null)
            {
                description = m_weatherCodeDescriptions.ContainsKey(Convert.ToInt32(weatherCode)) ? m_weatherCodeDescriptions[Convert.ToInt32(weatherCode)] : string.Empty;
            }

            return description;
        }

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
                    AirTemperature = weatherModels.Average(w => w.AirTemperature),
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
