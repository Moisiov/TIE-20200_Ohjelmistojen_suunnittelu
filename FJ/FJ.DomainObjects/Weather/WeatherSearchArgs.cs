using System;
using System.Collections.Generic;

namespace FJ.DomainObjects.Weather
{
    public class WeatherSearchArgs
    {
        public DateRange SearchDateRange { get; set; }
        public TimeRange DailyTimeRange { get; set; }
        public IEnumerable<string> Locations { get; set; }
    }
}
