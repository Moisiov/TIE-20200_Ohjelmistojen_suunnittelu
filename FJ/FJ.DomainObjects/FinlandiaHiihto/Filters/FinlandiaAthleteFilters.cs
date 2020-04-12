using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;

namespace FJ.DomainObjects.FinlandiaHiihto.Filters
{
    public class FinlandiaFirstNamesFilter : StringListFilterBase<FinlandiaFirstNamesFilter>
    {
        public override string ShortName => "First name";
        public override string Description => "First name of the athlete in Finlandia";

        public FinlandiaFirstNamesFilter(IEnumerable<string> firstNames)
            : base(firstNames)
        {
        }
    }
    
    public class FinlandiaLastNamesFilter : StringListFilterBase<FinlandiaLastNamesFilter>
    {
        public override string ShortName => "Last name";
        public override string Description => "Last name of the athlete in Finlandia";

        public FinlandiaLastNamesFilter(IEnumerable<string> lastNames)
            : base(lastNames)
        {
        }
    }

    public class FinlandiaFullNameFilter : StringFilterBase<FinlandiaFullNameFilter>
    {
        public override string ShortName => "Full name";
        public override string Description => "Full name of the athlete in Finlandia";

        public FinlandiaFullNameFilter(string fullName)
            : base(fullName)
        {
        }
    }
    
    public class FinlandiaYearsOfBirthFilter : GroupFilterBase<int, FinlandiaYearsOfBirthFilter>
    {
        public override string ShortName => "Year of birth";
        public override string Description => "All years of birth for competitors";

        public FinlandiaYearsOfBirthFilter(IEnumerable<int> yearsOfBirth)
            : base(yearsOfBirth)
        {
        }
    }
    
    public class FinlandiaHomeCitiesFilter : StringListFilterBase<FinlandiaHomeCitiesFilter>
    {
        public override string ShortName => "Home city";
        public override string Description => "Finlandia competitor's city";

        public FinlandiaHomeCitiesFilter(IEnumerable<string> cityNames)
            : base(cityNames)
        {
        }
    }

    public class FinlandiaTeamsFilter : StringListFilterBase<FinlandiaTeamsFilter>
    {
        public override string ShortName => "Team";
        public override string Description => "Finlandia competitor's team";

        public FinlandiaTeamsFilter(IEnumerable<string> teams)
            : base(teams)
        {
        }
    }
}
