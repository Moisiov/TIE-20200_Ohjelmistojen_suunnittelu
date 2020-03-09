using System;
using System.Collections.Generic;
using FJ.DomainObjects.FinlandiaHiihto.Enums;

namespace FJ.DomainObjects.FinlandiaHiihto
{
    public class FinlandiaHiihtoSearchArgs
    {
        public IEnumerable<int> CompetitionYears { get; set; }
        public IEnumerable<FinlandiaSkiingStyle> Styles { get; set; }
        public IEnumerable<FinlandiaSkiingDistance> Distances { get; set; }

        public TimeSpan MinResult { get; set; }
        public TimeSpan MaxResult { get; set; }

        public int? MinPositionGeneral { get; set; }
        public int? MaxPositionGeneral { get; set; }

        public int? MinPositionMen { get; set; }
        public int? MaxPositionMen { get; set; }

        public int? MinPositionWomen { get; set; }
        public int? MaxPositionWomen { get; set; }

        public IEnumerable<FinlandiaSkiingGender> Genders { get; set; }
        public IEnumerable<FinlandiaSkiingAgeGroup> AgeGroups { get; set; }

        public IEnumerable<string> LastNames { get; set; }
        public IEnumerable<string> FirstNames { get; set; }
        public IEnumerable<string> Cities { get; set; }
        public IEnumerable<string> Nationalities { get; set; }
        public IEnumerable<int> YearsOfBirth { get; set; }
        public IEnumerable<string> Teams { get; set; }
    }
}
