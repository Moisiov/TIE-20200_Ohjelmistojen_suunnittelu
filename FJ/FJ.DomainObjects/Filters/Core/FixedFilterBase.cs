using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FJ.DomainObjects.Filters.Core
{
    public abstract class FixedFilterBase<TFilter> : FilterBase<TFilter>
        where TFilter : IFilter
    {
        public override string Description => ShortName;

        public sealed override bool Equals(TFilter obj)
        {
            return true;
        }
    }
}
