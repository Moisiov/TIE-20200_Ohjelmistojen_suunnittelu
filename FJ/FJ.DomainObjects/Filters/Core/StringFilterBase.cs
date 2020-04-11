using System;
using System.Collections.Generic;
using System.Linq;

namespace FJ.DomainObjects.Filters.Core
{
    public abstract class StringFilterBase<TFilter> : FilterBase<TFilter>
        where TFilter : StringFilterBase<TFilter>
    {
        public string Value { get; set; }

        protected virtual string Name => GetType().Name;
        public override string ShortName => Name;
        public override string Description => Value;

        protected StringFilterBase(string value)
        {
            Value = value;
        }

        public sealed override bool Equals(TFilter obj)
        {
            return obj != null && obj.Value == Value;
        }
    }

    public abstract class StringListFilterBase<TFilter> : FilterBase<TFilter>
        where TFilter : StringListFilterBase<TFilter>
    {
        public List<string> SearchStringsList { get; }

        protected virtual string Name => "String filter";
        public override string ShortName => Name;
        public override string Description => $"{Name} ({SearchStringsList.FirstOrDefault() ?? "-"})";

        protected StringListFilterBase(IEnumerable<string> searchStrings)
        {
            SearchStringsList = new List<string>(searchStrings);
        }

        public override bool Equals(TFilter obj)
        {
            return obj != null && SearchStringsList.All(s => obj.SearchStringsList.Contains(s));
        }
    }
}
