using System;
using FJ.DomainObjects.Filters.Core;

namespace FJ.DomainObjects.Filters.CommonFilters
{
    public class TimeFilter : TimeFilterBase<TimeFilter>
    {
        public TimeFilter(TimeSpan min, TimeSpan max)
            : base(min, max)
        {
        }
    }
}
