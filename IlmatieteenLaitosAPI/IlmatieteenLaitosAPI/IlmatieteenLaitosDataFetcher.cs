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
                Request = "getFeature",
                StoredQueryId = "fmi::observations::weather::hourly::multipointcoverage"
            }
        };

        private static readonly HttpClient s_httpClient = new HttpClient();

        public async Task<IEnumerable<WeatherModel>> FetchWeather(string location, DateTime startTime, DateTime endTime)
        {
            var startTimeRounded = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, 0, 0, startTime.Kind);
            var endTimeRounded = new DateTime(endTime.Year, endTime.Month, endTime.Day, endTime.Hour, 0, 0, endTime.Kind);

            m_request.Parameters.SearchParameters = new Dictionary<string, string>
            {
                { "place",  location },
                { "starttime", startTimeRounded.ToUniversalTime().ToString(m_request.DateTimeFormat) },
                { "endtime", endTimeRounded.ToUniversalTime().ToString(m_request.DateTimeFormat) }
            };

            var response = await GetRequest();

            XNamespace ns = "http://www.opengis.net/gml/3.2";
            var dataBlock = response.Descendants(ns + "doubleOrNilReasonTupleList").FirstOrDefault()?.Value;

            var result = ParseDataBlock(dataBlock, startTimeRounded, location);

            return result;
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
                Console.WriteLine(e.Message);
#endif
            }

            return null;
        }

        private static IEnumerable<WeatherModel> ParseDataBlock(string dataBlock, DateTime startTime, string location)
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

                    weatherData.Add(new WeatherModel
                    {
                        ObservationStartTime = time.AddHours(-1),
                        ObservationEndTime = time,
                        Location = location,
                        AirTemperatureAvg = doubles[0],
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

                    time = time.AddHours(1);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.Message);
#endif
            }

            return weatherData;
        }
    }
}
