using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.WebUtilities;

namespace IlmatieteenLaitosAPI
{
    internal class IlmatieteenLaitosDataFetcher
    {
        // Base information of API requests
        private readonly APIRequestModel m_request = new APIRequestModel
        {
            BaseUrl = "http://opendata.fmi.fi/wfs/fin",
            DateTimeFormat = "yyyy-MM-ddTHH':'mm':'ssZ",
            Parameters = new APIRequestParameterModel
            {
                Service = "WFS",
                Version = "2.0.0",
                Request = "getFeature"
            }
        };

        private static readonly HttpClient s_httpClient = new HttpClient();

        public async Task<IEnumerable<WeatherModel>> FetchWeather(string location, DateTime start, DateTime end, bool hourly = true)
        {
            try
            {
                var startTime = hourly ? new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0, start.Kind) : start;
                var endTime = hourly ? new DateTime(end.Year, end.Month, end.Day, end.Hour, 0, 0, end.Kind) : end;

                m_request.Parameters.StoredQueryId = hourly ? "fmi::observations::weather::hourly::multipointcoverage" : "fmi::observations::weather::multipointcoverage";

                if (hourly)
                {
                    m_request.Parameters.SearchParameters = new Dictionary<string, string>
                    {
                        { "place",  location },
                        { "starttime", startTime.ToUniversalTime().ToString(m_request.DateTimeFormat) },
                        { "endtime", endTime.ToUniversalTime().ToString(m_request.DateTimeFormat) }
                    };
                }
                else
                {
                    m_request.Parameters.SearchParameters = new Dictionary<string, string>
                    {
                        { "place",  location },
                        { "endtime", endTime.ToUniversalTime().ToString(m_request.DateTimeFormat) }
                    };
                }

                var response = await GetRequest();

                XNamespace ns = "http://www.opengis.net/gml/3.2";
                var dataBlock = response.Descendants(ns + "doubleOrNilReasonTupleList").FirstOrDefault()?.Value;

                var result = ParseDataBlock(dataBlock, startTime, location, hourly);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("IlmatieteenLaitosAPI: " + e.Message);
            }

            return Enumerable.Empty<WeatherModel>();
        }

        private async Task<XDocument> GetRequest()
        {
            try
            {
                var query = new Dictionary<string, string>
                {
                    { "service", m_request.Parameters.Service },
                    { "version", m_request.Parameters.Version },
                    { "request", m_request.Parameters.Request },
                    { "storedquery_id", m_request.Parameters.StoredQueryId }
                };

                foreach (var (key, value) in m_request.Parameters.SearchParameters)
                {
                    query.Add(key, value);
                }

                var url = QueryHelpers.AddQueryString(m_request.BaseUrl, query);
                var response = await s_httpClient.GetStringAsync(url);

                return XDocument.Parse(response);
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("IlmatieteenLaitosAPI: " + e.Message);
#endif
            }

            return null;
        }

        private static IEnumerable<WeatherModel> ParseDataBlock(string dataBlock, DateTime startTime, string location, bool hourly)
        {
            IList<WeatherModel> weatherData = new List<WeatherModel>();
            var time = startTime;

            try
            {
                var rows = dataBlock.Split("\n");

                foreach (var row in rows)
                {
                    if (string.IsNullOrWhiteSpace(row))
                    {
                        continue;
                    }

                    var values = row.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    var doubles = new double?[values.Length];

                    for (var i = 0; i < values.Length; ++i)
                    {
                        doubles[i] = values[i] == "NaN"
                            ? (double?)null
                            : Convert.ToDouble(values[i], CultureInfo.InvariantCulture);
                    }

                    if (hourly)
                    {
                        weatherData.Add(new WeatherModel
                        {
                            ObservationStartTime = time.AddHours(-1),
                            ObservationEndTime = time,
                            Location = location,
                            AirTemperature = doubles[0],
                            AirTemperatureMax = doubles[1],
                            AirTemperatureMin = doubles[2],
                            AirHumidityAvg = doubles[3],
                            WindSpeedAvg = doubles[4],
                            WindSpeedMax = doubles[5],
                            WindSpeedMin = doubles[6],
                            WindDirection = doubles[7],
                            Precipitation = doubles[8],
                            PrecipitationIntensityMaximum = doubles[9],
                            AirPressureAvg = doubles[10],
                            MostSignificantWeatherCode = doubles[11]
                        });
                    }
                    else
                    {
                        weatherData.Add(new WeatherModel
                        {
                            ObservationStartTime = time,
                            ObservationEndTime = time,
                            Location = location,
                            AirTemperature = doubles[0],
                            AirTemperatureMax = null,
                            AirTemperatureMin = null,
                            AirHumidityAvg = doubles[4],
                            WindSpeedAvg = doubles[1],
                            WindSpeedMax = null,
                            WindSpeedMin = null,
                            WindDirection = doubles[3],
                            Precipitation = null,
                            PrecipitationIntensityMaximum = null,
                            AirPressureAvg = null,
                            MostSignificantWeatherCode = doubles[12]
                        });
                    }

                    time = time.AddHours(1);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("IlmatieteenLaitosAPI: " + e.Message);
#endif
            }

            return weatherData;
        }
    }
}
