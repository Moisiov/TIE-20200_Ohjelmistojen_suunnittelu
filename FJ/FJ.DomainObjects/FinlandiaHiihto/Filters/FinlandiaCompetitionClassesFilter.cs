using System;
using System.Collections.Generic;
using System.Linq;
using FJ.DomainObjects.Filters.Core;

namespace FJ.DomainObjects.FinlandiaHiihto.Filters
{
    public class FinlandiaCompetitionClassesFilter
        : GroupFilterBase<FinlandiaHiihtoCompetitionClass, FinlandiaCompetitionClassesFilter>
    {
        public override string ShortName => "Finlandia competition class";
        public override string Description => "Style and distance of the result";

        public FinlandiaCompetitionClassesFilter(IEnumerable<FinlandiaHiihtoCompetitionClass> competitionClasses)
            : base(competitionClasses)
        {
        }
    }
}
