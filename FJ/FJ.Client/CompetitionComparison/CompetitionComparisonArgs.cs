using System;
using FJ.Client.Core;
using FJ.DomainObjects;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.Client.CompetitionComparison
{
    public class CompetitionComparisonArgs : NavigationArgsBase<CompetitionComparisonArgs>
    {
        public int? Competition1Year { get; set; }
        public FinlandiaHiihtoCompetitionClass Competition1Class { get; set; }
        public int? Competition2Year { get; set; }
        public FinlandiaHiihtoCompetitionClass Competition2Class { get; set; }
    }
}
