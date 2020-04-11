using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.Filters.ImplementationCore;
using FJ.Utils;
using Unity;

namespace FJ.Services.CoreServices
{
    public interface IFilterImplementationProvider
    {
        IFilterImplementation GetImplementation(IFilter filter, Type filteredObjectType, FilterPurpose purpose);
        IFilterImplementation GetImplementation(Type filterType, Type filteredObjectType, FilterPurpose purpose);
    }

    public class FilterImplementationProvider : IFilterImplementationProvider
    {
        private class FilterImplementationType
        {
            public Type ImplementationType { get; set; }
            public FilterPurpose Purpose { get; set; }
        }
        
        // Filter type => Implementations
        private readonly Dictionary<Type, List<FilterImplementationType>> m_filterTypesImplementations =
            new Dictionary<Type, List<FilterImplementationType>>();
        
        public FilterImplementationProvider()
        {
            var filterImplTypes = ReflectionHelpers.GetClasses<IFilterImplementation>(
                c => !c.IsAbstract).ToHashSet();

            foreach (var filterImplType in filterImplTypes)
            {
                // Get implementation's target filter type
                var filterType = filterImplType
                    .GetInterfaces()
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IFilterImplementation<>))
                    .Select(x => x.GetGenericArguments()[0])
                    .Single();

                var filterAttr = filterImplType.GetCustomAttributes<FilterPurposeAttribute>();

                if (!m_filterTypesImplementations.ContainsKey(filterType))
                {
                    m_filterTypesImplementations.Add(filterType, new List<FilterImplementationType>());
                }
                
                m_filterTypesImplementations[filterType].Add(new FilterImplementationType
                {
                    ImplementationType = filterImplType,
                    Purpose = filterAttr.Select(x => x.Purpose).Single()
                });
            }
        }
        
        public IFilterImplementation GetImplementation(IFilter filter, Type filteredObjectType, FilterPurpose purpose)
        {
            return GetImplementation(filter.GetType(), filteredObjectType, purpose);
        }

        public IFilterImplementation GetImplementation(Type filterType, Type filteredObjectType, FilterPurpose purpose)
        {
            if (!m_filterTypesImplementations.ContainsKey(filterType))
            {
                return null;
            }

            var implType = m_filterTypesImplementations[filterType]
                .SingleOrDefault(x => x.Purpose == purpose)?.ImplementationType;

            return (IFilterImplementation)(implType == null ? null : Activator.CreateInstance(implType));
        }
    }
    
    public static class FilterUtils
    {
        public static FilterSearchComposer<TSearch> ApplyFilters<TSearch>(
            this FilterSearchComposer<TSearch> composer,
            FilterCollection filters,
            IFilterImplementationProvider filterImplementationProvider)
            where TSearch : new()
        {
            if (filters == null)
            {
                throw new ArgumentException(nameof(filters));
            }
            
            var filterExpressions = filters
                .Select(x => new
                {
                    impl = filterImplementationProvider.GetImplementation(
                        x.GetType(),
                        typeof(TSearch),
                        FilterPurpose.ExpandSearchArgs),
                    filter = x
                })
                .Select(x => x.impl?.GetImplementation(x.filter, typeof(TSearch), null) as LambdaExpression)
                .Where(x => x != null)
                .ToArray();

            foreach (var expression in filterExpressions)
            {
                ((Action<FilterSearchComposer<TSearch>>)expression.Compile()).Invoke(composer);
            }

            return composer;
        }

        public static IEnumerable<TObject> ApplyFilters<TObject>(
            this IEnumerable<TObject> query,
            FilterCollection filters,
            IFilterImplementationProvider filterImplementationProvider)
        {
            if (filters == null)
            {
                throw new ArgumentException(nameof(filters));
            }
            
            var filterExpressions = filters
                .Select(x => new
                {
                    impl = filterImplementationProvider.GetImplementation(
                        x.GetType(),
                        typeof(TObject),
                        FilterPurpose.ReduceSearchResults),
                    filter = x
                })
                .Select(x => x.impl?.GetImplementation(x.filter, typeof(TObject), null) as LambdaExpression)
                .Where(x => x != null)
                .ToArray();

            if (!filterExpressions.Any())
            {
                return query;
            }
            
            var e = filterExpressions.First();
            var targetExpr = Expression.Lambda<Func<TObject, bool>>(e.Body, e.Parameters);

            targetExpr = filterExpressions
                .Skip(1)
                .Select(filterExpr => Expression.Lambda<Func<TObject, bool>>(filterExpr.Body, filterExpr.Parameters))
                .Aggregate(targetExpr, AndAlso);

            return query.Where(targetExpr.Compile());
        }
        
        // Inspiration: https://stackoverflow.com/a/457328
        private static System.Linq.Expressions.Expression<Func<T, bool>> AndAlso<T>(
            this System.Linq.Expressions.Expression<Func<T, bool>> expr1,
            System.Linq.Expressions.Expression<Func<T, bool>> expr2)
        {
            // If both use the same parameter instance, this becomes trivial
            var param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
            {
                return Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(expr1.Body, expr2.Body), param);
            }
            
            // Keep expr1 and invoke expr2
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    expr1.Body,
                    Expression.Invoke(expr2, param)), param);
        }
    }
}
