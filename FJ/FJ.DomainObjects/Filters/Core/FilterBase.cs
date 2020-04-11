using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FJ.DomainObjects.Filters.ImplementationCore;

namespace FJ.DomainObjects.Filters.Core
{
    public interface IFilter
    {
        string ShortName { get; }
        string Description { get; }
    }
    
    public abstract class FilterBase : IFilter
    {
        public abstract string ShortName { get; }
        public abstract string Description { get; }

        public override int GetHashCode()
        {
            return ShortName?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return IsEqual(obj);
        }

        public override string ToString()
        {
            return $"({ShortName}) {Description}";
        }

        protected string GetDefaultDescription(object obj)
        {
            return $"{ShortName} ({ObjectToString(obj)})";
        }

        protected string GetDefaultDescription(IEnumerable<object> data)
        {
            return $"{ShortName} ({string.Join(", ", data.Select(ObjectToString).OrderBy(x => x))})";
        }
        
        protected abstract bool IsEqual(object obj);

        private static string ObjectToString(object obj)
        {
            if (obj == null)
            {
                return "-";
            }

            if (obj.GetType().IsEnum)
            {
                return obj.GetType()
                    .GetMember(obj.ToString())[0]
                    .GetCustomAttribute<DescriptionAttribute>()
                    .Description;
            }

            return obj.ToString();
        }
    }

    public abstract class FilterBase<TFilter> : FilterBase, IEquatable<TFilter>
        where TFilter : IFilter
    {
        public sealed override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public sealed override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected sealed override bool IsEqual(object obj)
        {
            return Equals((TFilter)obj);
        }

        public abstract bool Equals(TFilter obj);
        
        // Convenience classes
        public abstract class ExpressionImplementation : Expression<TFilter> { }
        public abstract class DomainExpressionImplementation : DomainExpression<TFilter> { }
        public abstract class ReduceSearchResultsExpressionImplementation : ReduceSearchResults<TFilter> { }
        public abstract class ExpandSearchArgsExpressionImplementation : ExpandSearchArgsExpression<TFilter> { }
    }

    public abstract class OrderFilterBase : FilterBase
    {
        public override string ShortName => GetType().Name;
        public override string Description => ShortName;

        public bool Ascending { get; set; }

        protected override bool IsEqual(object obj)
        {
            if (obj is OrderFilterBase o)
            {
                return Ascending == o.Ascending;
            }

            return false;
        }
    }

    public class OrderFilterBase<TFilter> : OrderFilterBase
    {
        // Convenience classes
        // TODO
    }

    public class OrderFilterBase<TFilter, TValue> : OrderFilterBase<TFilter>
    {
        public TValue Value { get; set; }

        public OrderFilterBase()
        {
        }

        public OrderFilterBase(TValue value)
        {
            Value = value;
        }

        protected override bool IsEqual(object obj)
        {
            return base.IsEqual(obj)
                   && EqualityComparer<TValue>.Default.Equals(((OrderFilterBase<TFilter, TValue>)obj).Value, Value);
        }
    }
}
