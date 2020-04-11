using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;

namespace FJ.DomainObjects.FinlandiaHiihto.Filters
{
    public class FinlandiaCompetitionYearsFilter : GroupFilterBase<int, FinlandiaCompetitionYearsFilter>
    {
        public override string ShortName => "Competition year";
        public override string Description => "Years in which the result has to be accomplished";

        public FinlandiaCompetitionYearsFilter(IEnumerable<int> competitionYears)
            : base(competitionYears)
        {
        }
    }
}
