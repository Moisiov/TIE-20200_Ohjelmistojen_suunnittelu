using System;
using System.Globalization;

namespace FJ.DomainObjects.Filters.Core
{
    public abstract class DateFilterBase<TFilter> : RangeFilterBase<DateTime, TFilter>
        where TFilter : DateFilterBase<TFilter>
    {
        public override string ShortName =>
            $"{Min.ToString(CultureInfo.CurrentCulture)}–{Max.ToString(CultureInfo.CurrentCulture)}";
        public override string Description => ShortName;

        public bool HasMin => Min != DateTime.MinValue;
        public bool HasMax => Max != DateTime.MaxValue;

        protected DateFilterBase(DateTime min, DateTime max)
            : base(min, max)
        {
        }

        public DateRange GetDateRange()
        {
            return new DateRange
            {
                Start = HasMin ? Min : (DateTime?)null,
                End = HasMax ? Max : (DateTime?)null
            };
        }
    }
}
