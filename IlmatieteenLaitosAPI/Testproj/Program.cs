﻿using System;
using IlmatieteenLaitosAPI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Testproj
{
    internal static class Program
    {
        internal static async Task Main(string[] args)
        {
            IIlmatieteenLaitosAPI api = new IlmatieteenLaitosAPI.IlmatieteenLaitosAPI();

            var start = DateTime.Now.AddYears(-1);
            var end = start.AddHours(2);
            var loc = "Lahti";
            var result = await api.GetHourlyWeather(loc, start, end);
            var result2 = await api.GetHourlyWeatherOfDay(loc, start.Year, start.Month, start.Day);
            var result3 = await api.GetHourlyWeatherOfDay(loc, start);
            var singleresult = await api.GetWeather(loc, start, end);
            var singleresult2 = await api.GetWeatherOfDay(loc, start.Year, start.Month, start.Day);
            var singleresult3 = await api.GetWeatherOfDay(loc, start);
            var singleresultnow = await api.GetWeatherNow(loc);
            Console.ReadKey();
        }
    }
}
