using System;
using FJ.DomainObjects.Filters.Core;

namespace FJ.DomainObjects.Filters.CommonFilters
{
    public class DateFilter : DateFilterBase<DateFilter>
    {
        public DateFilter(DateTime min, DateTime max)
            : base(min, max)
        {
        }
    }
}
