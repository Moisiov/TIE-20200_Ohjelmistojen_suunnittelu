using System;
using System.Collections.Generic;
using FinlandiaHiihtoAPI;

namespace FJ.NUnitTests.TestData
{
    public static class ReduceSearchResultsDummyData
    {
        public static IEnumerable<FinlandiaHiihtoAPISearchResultRow> GetData()
        {
            var results = new List<FinlandiaHiihtoAPISearchResultRow>
            {
                new FinlandiaHiihtoAPISearchResultRow(new[]
                {
                    "2019", "", "1:58:51.76", "3", "2", "", "M", "Mäkinen Jani", "", "", "1975", ""
                }),
                new FinlandiaHiihtoAPISearchResultRow(new[]
                {
                    "2019", "", "1:22:51.76", "99", "", "", "M", "Laakso Saku", "", "", "", ""
                }),
                new FinlandiaHiihtoAPISearchResultRow(new[]
                {
                    "2016", "", "2:58:51.76", "6", "5", "", "M", "Mäkinen Jani", "", "", "1996", ""
                }),
                new FinlandiaHiihtoAPISearchResultRow(new[]
                {
                    "2017", "", "1:31:00.00", "8", "", "1", "M", "Mäkinen Joona", "", "", "1995", ""
                }),
                new FinlandiaHiihtoAPISearchResultRow(new[]
                {
                    "2018", "", "2:58:51.76", "43", "", "6", "M", "Virtanen Joni", "", "", "1990", ""
                }),
                new FinlandiaHiihtoAPISearchResultRow(new[]
                {
                    "2017", "", "2:58:51.00", "9", "8", "", "M", "Mäkinen Joona", "", "", "", "1995"
                }),
                new FinlandiaHiihtoAPISearchResultRow(new[]
                {
                    "2019", "", "2:58:51.76", "13", "", "", "M", "", "", "", "1988", ""
                })
            };
            
            return results;
        }
    }
}
