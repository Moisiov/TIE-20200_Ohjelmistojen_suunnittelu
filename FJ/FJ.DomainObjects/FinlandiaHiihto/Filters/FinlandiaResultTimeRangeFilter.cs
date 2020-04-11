using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters;
using FJ.DomainObjects.Filters.CommonFilters;
using FJ.DomainObjects.Filters.Core;

namespace FJ.DomainObjects.FinlandiaHiihto.Filters
{
    public class FinlandiaResultTimeRangeFilter : TimeFilterBase<FinlandiaResultTimeRangeFilter>
    {
        public override string ShortName => "Result time range";
        public override string Description => "Time range in which the result time has to fall";

        public FinlandiaResultTimeRangeFilter(TimeSpan? min, TimeSpan? max)
            : base(min ?? TimeSpan.MinValue, max ?? TimeSpan.MaxValue)
        {
        }
    }
}
