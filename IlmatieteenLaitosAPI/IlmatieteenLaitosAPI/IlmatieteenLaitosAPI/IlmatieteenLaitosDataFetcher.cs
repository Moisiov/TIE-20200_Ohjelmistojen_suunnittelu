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
    class IlmatieteenLaitosDataFetcher
    {
        // Base information of API requests
        private APIRequestModel _request = new APIRequestModel()
        {
            BaseUrl = "http://opendata.fmi.fi/wfs/fin",
            DateTimeFormat = "yyyy-MM-ddTHH':'mm':'ssZ",
            Parameters = new APIRequestParameterModel()
            {
                Service = "WFS",
                Version = "2.0.0",
                Request = "getFeature",
                StoredqueryId = "fmi::observations::weather::hourly::multipointcoverage"
            }
        };

        private static HttpClient _httpClient = new HttpClient();

        public async Task<IEnumerable<WeatherModel>> FetchWeather(string location, DateTime startTime, DateTime endTime)
        {
            DateTime startTimeRounded = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, 0, 0, startTime.Kind);
            DateTime endTimeRounded = new DateTime(endTime.Year, endTime.Month, endTime.Day, endTime.Hour, 0, 0, endTime.Kind);

            _request.Parameters.SearchParameters = new Dictionary<string, string>()
            {
                { "place",  location },
                { "starttime", startTimeRounded.ToUniversalTime().ToString(_request.DateTimeFormat) },
                { "endtime", endTimeRounded.ToUniversalTime().ToString(_request.DateTimeFormat) }
            };

            var response = await GetRequest();

            XNamespace ns = "http://www.opengis.net/gml/3.2";
            var dataBlock = response.Descendants(ns + "doubleOrNilReasonTupleList").FirstOrDefault().Value;

            IEnumerable<WeatherModel> result = ParseDataBlock(dataBlock, startTimeRounded, location);

            return result;
        }

        private async Task<XDocument> GetRequest()
        {
            try
            {
                var query = new Dictionary<string, string>()
                {
                    { "service", _request.Parameters.Service },
                    { "version", _request.Parameters.Version },
                    { "request", _request.Parameters.Request },
                    { "storedquery_id", _request.Parameters.StoredqueryId }
                };

                foreach (KeyValuePair<string, string> param in _request.Parameters.SearchParameters)
                {
                    query.Add(param.Key, param.Value);
                }

                var url = QueryHelpers.AddQueryString(_request.BaseUrl, query);
                var response = await _httpClient.GetStringAsync(url);

                XDocument xml;
                xml = XDocument.Parse(response);
                return xml;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        private IEnumerable<WeatherModel> ParseDataBlock(string dataBlock, DateTime startTime, string location)
        {
            IList<WeatherModel> weatherData = new List<WeatherModel>();
            DateTime time = startTime;

            try
            {
                var rows = dataBlock.Split("\n");

                foreach (var row in rows)
                {
                    if (!string.IsNullOrWhiteSpace(row))
                    {
                        var values = row.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                        double?[] doubles = new double?[values.Length];

                        for (int i = 0; i < values.Length; ++i)
                        {
                            if (values[i] == "NaN")
                            {
                                doubles[i] = null;
                            }
                            else
                            {
                                doubles[i] = Convert.ToDouble(values[i], CultureInfo.InvariantCulture);
                            }
                        }

                        weatherData.Add(new WeatherModel()
                        {
                            ObservationStartTime = time.AddHours(-1),
                            ObservationEndTime = time,
                            Location = location,
                            AirTemperatureAvg = doubles[0],
                            AirTemperaturMax = doubles[1],
                            AirTemperatureMin = doubles[2],
                            AirHumidityAvg = doubles[3],
                            WindSpeedAvg = doubles[4],
                            WindSpeedMin = doubles[5],
                            WindSpeedMax = doubles[6],
                            WindGustSpeedMax = doubles[7],
                            RainAccumulated = doubles[8],
                            RainIntensityMaximum = doubles[9],
                            AirPressure = doubles[10],
                            MostSignificantWeatherCode = doubles[11]
                        });

                        time = time.AddHours(1);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return weatherData;
        }
    }
}
