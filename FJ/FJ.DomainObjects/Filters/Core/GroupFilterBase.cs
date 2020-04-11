using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FJ.DomainObjects.Filters.Core
{
    public abstract class GroupFilterBase<TItem, TFilter> : FilterBase<TFilter>
        where TFilter : GroupFilterBase<TItem, TFilter>
    {
        public List<TItem> Items { get; }

        protected virtual string Name => "Group filter";
        public override string ShortName => Name;
        public override string Description => $"{Name} ({Items.FirstOrDefault()?.ToString() ?? "-"})";

        protected GroupFilterBase(IEnumerable<TItem> items)
        {
            Items = items.ToList();
        }

        public override bool Equals(TFilter obj)
        {
            return obj != null && Items.All(i => obj.Items.Contains(i));
        }
    }
}
