using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.Filters.Core;

namespace FJ.Client.Core.UIElements.Filters.FilterModels
{
    public abstract class FJFilterModel_TimeRange<TFilter> : RegisterFilterModelBase<TimeRange>
        where  TFilter : TimeFilterBase<TFilter>
    {
        private readonly Func<TimeRange, TFilter> m_createFilterFunc;
        
        public override void ClearFilters()
        {
            Value = new TimeRange();
        }

        protected FJFilterModel_TimeRange(Func<TimeRange, TFilter> createFilterFunc)
        {
            m_createFilterFunc = createFilterFunc;
        }

        protected override FilterBase DoGetActiveFilter()
        {
            return Value?.NotEmpty ?? false ? m_createFilterFunc(Value) : null;
        }
    }
}
