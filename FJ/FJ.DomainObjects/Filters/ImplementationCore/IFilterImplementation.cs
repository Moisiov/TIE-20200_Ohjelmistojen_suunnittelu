using System;
using FJ.DomainObjects.Filters.Core;

namespace FJ.DomainObjects.Filters.ImplementationCore
{
    public interface IFilterImplementation
    {
        object GetImplementation(IFilter filter, Type type, object filterSpecificData);
    }

    public interface IFilterImplementation<TFilter> : IFilterImplementation
        where TFilter : IFilter
    {
    }
}
