using System;
using System.Linq.Expressions;
using FJ.DomainObjects.Filters.Core;

namespace FJ.DomainObjects.Filters.ImplementationCore
{
    public enum FilterPurpose
    {
        ExpandSearchArgs,
        ReduceSearchResults,
        DomainObject
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FilterPurposeAttribute : Attribute
    {
        public FilterPurpose Purpose { get; set; }

        public FilterPurposeAttribute(FilterPurpose purpose)
        {
            Purpose = purpose;
        }
    }
    
    public abstract class Expression<TFilter> : IFilterImplementation<TFilter>
        where TFilter : IFilter
    {
        protected abstract LambdaExpression GetExpression(TFilter filter, Type parameterType);

        object IFilterImplementation.GetImplementation(IFilter filter, Type type, object filterSpecificData)
        {
            return GetExpression((TFilter)filter, type);
        }
    }

    [FilterPurpose(FilterPurpose.DomainObject)]
    public abstract class DomainExpression<TFilter> : Expression<TFilter>
        where TFilter : IFilter
    {
    }

    [FilterPurpose(FilterPurpose.ReduceSearchResults)]
    public abstract class ReduceSearchResults<TFilter> : Expression<TFilter>
        where TFilter : IFilter
    {
    }

    [FilterPurpose(FilterPurpose.ExpandSearchArgs)]
    public abstract class ExpandSearchArgsExpression<TFilter> : Expression<TFilter>
        where TFilter : IFilter
    {
    }
}
