using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;

namespace FJ.DomainObjects.FinlandiaHiihto.Filters
{
    public class FinlandiaPositionRangeGeneralFilter : RangeFilterBase<int, FinlandiaPositionRangeGeneralFilter>
    {
        public override string ShortName => "General position";
        public override string Description => "General position in competition result";

        public FinlandiaPositionRangeGeneralFilter(int? min, int? max)
            : base(min ?? 0, max ?? int.MaxValue)
        {
        }
    }
    
    public class FinlandiaPositionRangeMenFilter : RangeFilterBase<int, FinlandiaPositionRangeMenFilter>
    {
        public override string ShortName => "General men's position";
        public override string Description => "General men's position in competition result";

        public FinlandiaPositionRangeMenFilter(int? min, int? max)
            : base(min ?? 0, max ?? int.MaxValue)
        {
        }
    }
    
    public class FinlandiaPositionRangeWomenFilter : RangeFilterBase<int, FinlandiaPositionRangeWomenFilter>
    {
        public override string ShortName => "General women's position";
        public override string Description => "General women's position in competition result";

        public FinlandiaPositionRangeWomenFilter(int? min, int? max)
            : base(min ?? 0, max ?? int.MaxValue)
        {
        }
    }
}
