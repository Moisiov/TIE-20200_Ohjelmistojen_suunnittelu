using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.Utils;

namespace FJ.Client.Core.UIElements.Filters.FilterModels
{
    public abstract class FJFilterModel_TextBox<TFilter> : RegisterFilterModelBase<string>
        where TFilter : StringFilterBase<TFilter>
    {
        private readonly Func<string, TFilter> m_createFilterFunc;

        protected FJFilterModel_TextBox(Func<string, TFilter> createFilterFunc)
        {
            m_createFilterFunc = createFilterFunc;
        }

        protected override FilterBase DoGetActiveFilter()
        {
            return Value.IsNullOrEmpty() ? null : m_createFilterFunc(Value);
        }
    }
    
    public abstract class FJFilterModel_MultiTextBox<TFilter> : RegisterIEnumerableFilterBase<string>
        where TFilter : StringListFilterBase<TFilter>
    {
        private readonly Func<IEnumerable<string>, TFilter> m_createFilterFunc;
        
        public override void ClearFilters()
        {
            Value = new string[] { };
        }

        protected FJFilterModel_MultiTextBox(Func<IEnumerable<string>, TFilter> createFilterFunc)
        {
            m_createFilterFunc = createFilterFunc;
        }

        protected override FilterBase DoGetActiveFilter()
        {
            return Value?.Any() ?? false ? m_createFilterFunc(Value) : null;
        }
    }
}
