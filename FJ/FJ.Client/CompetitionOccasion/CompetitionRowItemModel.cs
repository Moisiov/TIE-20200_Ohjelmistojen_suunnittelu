using System;
using System.Collections.Generic;
using FJ.DomainObjects;

namespace FJ.Client.CompetitionOccasion
{
    public class CompetitionRowItemModel
    {
        public Competition CompetitionInfo { get; set; }
        public int TotalParticipants { get; set; }
        public CompetitorSummaryItemModel FirstPlaceCompetitor { get; set; }
        public CompetitorSummaryItemModel LastPlaceCompetitor { get; set; }
        public List<Top10TeamItemModel> Top10Teams { get; set; }
    }

    public class CompetitorSummaryItemModel
    {
        public Person Competitor { get; set; }
        public TimeSpan Result { get; set; }
        public double AverageSpeed { get; set; }
    }
    
    public class Top10TeamItemModel
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public TimeSpan Result { get; set; }
    }
}
