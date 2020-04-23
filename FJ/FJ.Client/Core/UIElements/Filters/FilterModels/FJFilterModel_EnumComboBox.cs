using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.Utils;

namespace FJ.Client.Core.UIElements.Filters.FilterModels
{
    public abstract class FJFilterModel_EnumComboBox<TEnum, TFilter> : RegisterEnumFilterModelBase<TEnum>
        where TEnum : struct, IConvertible
        where  TFilter : EnumFilterBase<TEnum, TFilter>
    {
        private readonly Func<TEnum, TFilter> m_createFilterFunc;

        protected FJFilterModel_EnumComboBox(Func<TEnum, TFilter> createFilterFunc)
        {
            m_createFilterFunc = createFilterFunc;
        }

        protected override FilterBase DoGetActiveFilter()
        {
            // TODO Should this throw?
            return Value.HasValue ? m_createFilterFunc(Value.Value) : null;
        }
    }
    
    public abstract class FJFilterModel_NullableEnumComboBox<TEnum, TFilter> : RegisterEnumFilterModelBase<TEnum>
        where TEnum : struct, IConvertible
        where  TFilter : EnumFilterBase<TEnum, TFilter>
    {
        private readonly Func<TEnum?, TFilter> m_createFilterFunc;
        
        public override void ClearFilters()
        {
            Value = null;
        }

        protected FJFilterModel_NullableEnumComboBox(Func<TEnum?, TFilter> createFilterFunc)
        {
            m_createFilterFunc = createFilterFunc;
        }

        protected override FilterBase DoGetActiveFilter()
        {
            return Value.HasValue ? m_createFilterFunc(Value) : null;
        }
    }
    
    // Derive from reference type base class as IEnumerable is not a value type
    public abstract class FJFilterModel_MultiEnumComboBox<TEnum, TFilter> : RegisterIEnumerableFilterBase<TEnum>
        where TEnum : struct, IConvertible
        where  TFilter : EnumFilterBase<TEnum, TFilter>
    {
        private readonly Func<IEnumerable<TEnum>, TFilter> m_createFilterFunc;
        
        public override void ClearFilters()
        {
            Value = new List<TEnum>();
        }

        protected FJFilterModel_MultiEnumComboBox(Func<IEnumerable<TEnum>, TFilter> createFilterFunc)
        {
            m_createFilterFunc = createFilterFunc;
        }

        protected override FilterBase DoGetActiveFilter()
        {
            return Value?.Any() ?? false ? m_createFilterFunc(Value) : null;
        }
    }
}
