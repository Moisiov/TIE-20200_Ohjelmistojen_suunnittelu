using System;
using System.Collections.Generic;
using FJ.DomainObjects;

namespace FJ.Client.CompetitionOccasion
{
    public class CompetitionOccasionModel
    {
        public List<CompetitionRowItemModel> CompetitionList { get; private set; }
        public int TotalParticipants { get; private set; }
        public int TotalCompetitions { get; private set; }

        public CompetitionOccasionModel()
        {
        }

        public void GetOccasionData(int? year)
        {
            // TODO Hae dataa serviceltä ja mäppää ne propeteiksi.
            CompetitionList = PopulateCompetitions();
            TotalParticipants = 3600;
            TotalCompetitions = 4;
        }

        private List<CompetitionRowItemModel> PopulateCompetitions()
        {
            var comp1 = new Competition { Name = "V50" };
            var comp2 = new Competition { Name = "P50" };
            var comp3 = new Competition { Name = "V100" };
            var comp4 = new Competition { Name = "P100" };
            var first = new CompetitorSummaryItemModel
            {
                Competitor = new Person { FirstName = "Hessu", LastName = "Hopo" },
                Result = new TimeSpan(hours: 2, minutes: 6, seconds: 14),
                AverageSpeed = 18.2
            };
            var last = new CompetitorSummaryItemModel
            {
                Competitor = new Person { FirstName = "Mikki", LastName = "Hiiri" },
                Result = new TimeSpan(hours: 5, minutes: 46, seconds: 33),
                AverageSpeed = 7.5
            };
            var teams = new List<Top10TeamItemModel>
            {
                new Top10TeamItemModel
                {
                    Position = 1, Name = "T1", Result = new TimeSpan(5, 6, 27)
                },
                new Top10TeamItemModel
                {
                    Position = 2, Name = "T2", Result = new TimeSpan(5, 23, 00)
                },
                new Top10TeamItemModel
                {
                    Position = 3, Name = "T3", Result = new TimeSpan(5, 26, 19)
                },
                new Top10TeamItemModel
                {
                    Position = 4, Name = "T4", Result = new TimeSpan(5, 55, 37)
                },
                new Top10TeamItemModel
                {
                    Position = 5, Name = "T5", Result = new TimeSpan(6, 17, 6)
                },
                new Top10TeamItemModel
                {
                    Position = 6, Name = "T6", Result = new TimeSpan(6, 30, 11)
                },
                new Top10TeamItemModel
                {
                    Position = 7, Name = "T7", Result = new TimeSpan(6, 66, 7)
                },
                new Top10TeamItemModel
                {
                    Position = 8, Name = "T8", Result = new TimeSpan(7, 11, 9)
                },
                new Top10TeamItemModel
                {
                    Position = 9, Name = "T9", Result = new TimeSpan(7, 44, 44)
                },
                new Top10TeamItemModel
                {
                    Position = 10, Name = "T10", Result = new TimeSpan(7, 45, 1)
                },
            };
            return new List<CompetitionRowItemModel>
            {
                new CompetitionRowItemModel
                {
                    CompetitionInfo = comp1,
                    TotalParticipants = 900,
                    FirstPlaceCompetitor = first,
                    LastPlaceCompetitor = last,
                    Top10Teams = teams
                },
                new CompetitionRowItemModel
                {
                    CompetitionInfo = comp2,
                    TotalParticipants = 900,
                    FirstPlaceCompetitor = first,
                    LastPlaceCompetitor = last,
                    Top10Teams = teams
                },
                new CompetitionRowItemModel
                {
                    CompetitionInfo = comp3,
                    TotalParticipants = 900,
                    FirstPlaceCompetitor = first,
                    LastPlaceCompetitor = last,
                    Top10Teams = teams
                },
                new CompetitionRowItemModel
                {
                    CompetitionInfo = comp4,
                    TotalParticipants = 900,
                    FirstPlaceCompetitor = first,
                    LastPlaceCompetitor = last,
                    Top10Teams = teams
                },
            };
        }
    }
}
