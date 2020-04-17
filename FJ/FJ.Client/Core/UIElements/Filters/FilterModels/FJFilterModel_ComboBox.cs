using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;

namespace FJ.Client.Core.UIElements.Filters.FilterModels
{
    public abstract class FJFilterModel_ComboBox<TItem, TFilter> : RegisterFilterModelBase<IEnumerable<TItem>>
        where  TFilter : GroupFilterBase<TItem, TFilter>
    {
        private readonly Func<IEnumerable<TItem>, TFilter> m_createFilterFunc;

        protected FJFilterModel_ComboBox(Func<IEnumerable<TItem>, TFilter> createFilterFunc)
        {
            m_createFilterFunc = createFilterFunc;
        }

        protected override FilterBase DoGetActiveFilter()
        {
            return Value != null ? m_createFilterFunc(Value) : null;
        }
    }
    
    public abstract class FJFilterModel_MultiComboBox<TItem, TFilter> : RegisterIEnumerableFilterBase<TItem>
        where  TFilter : GroupFilterBase<TItem, TFilter>
    {
        private readonly Func<IEnumerable<TItem>, TFilter> m_createFilterFunc;
        
        public override void ClearFilters()
        {
            Value = new TItem[] { };
        }

        protected FJFilterModel_MultiComboBox(Func<IEnumerable<TItem>, TFilter> createFilterFunc)
        {
            m_createFilterFunc = createFilterFunc;
        }

        protected override FilterBase DoGetActiveFilter()
        {
            return Value?.Any() ?? false ? m_createFilterFunc(Value) : null;
        }
    }
}
