using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FJ.DomainObjects.Filters.Core
{
    public abstract class EnumFilterBase<TEnum, TFilter> : FilterBase<TFilter>
        where TEnum : struct, IConvertible
        where TFilter : EnumFilterBase<TEnum, TFilter>
    {
        public HashSet<int> IntValues { get; }
        public HashSet<TEnum> EnumValues { get; }

        protected virtual string Name => GetType().Name;
        public override string ShortName => Name;
        public override string Description => GetDefaultDescription(EnumValues);

        protected EnumFilterBase(IEnumerable<TEnum> values)
        {
            EnumValues = values?.ToHashSet() ?? new HashSet<TEnum>();
            IntValues = EnumValues.Select(x => x.ToInt32(CultureInfo.CurrentCulture)).ToHashSet();
        }

        public sealed override bool Equals(TFilter obj)
        {
            return obj != null && EnumValues.SequenceEqual(obj.EnumValues);
        }

        protected void Add(TEnum value)
        {
            EnumValues.Add(value);
            IntValues.Add(value.ToInt32(CultureInfo.CurrentCulture));
        }
    }
}
