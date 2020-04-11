using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto.Enums;

namespace FJ.DomainObjects.FinlandiaHiihto.Filters
{
    public class FinlandiaAgeGroupsFilter : EnumFilterBase<FinlandiaSkiingAgeGroup, FinlandiaAgeGroupsFilter>
    {
        public override string ShortName => "Age group";
        public override string Description => "Finlandia result's age group";

        public FinlandiaAgeGroupsFilter(IEnumerable<FinlandiaSkiingAgeGroup> ageGroups)
            : base(ageGroups)
        {
        }
    }
}
