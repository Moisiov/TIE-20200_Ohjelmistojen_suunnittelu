using System;

namespace FJ.DomainObjects.Filters.Core
{
    public abstract class TimeFilterBase<TFilter> : RangeFilterBase<TimeSpan, TFilter>
        where TFilter : TimeFilterBase<TFilter>
    {
        private readonly string m_timeFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        public override string ShortName => $"{Min.ToString(m_timeFormat)}–{Max.ToString(m_timeFormat)}";
        public override string Description => ShortName;

        public bool HasMin => Min != TimeSpan.MinValue;
        public bool HasMax => Max != TimeSpan.MaxValue;

        protected TimeFilterBase(TimeSpan min, TimeSpan max)
            : base(min, max)
        {
        }

        public TimeRange GetTimeRange()
        {
            return new TimeRange
            {
                Start = HasMin ? Min : (TimeSpan?)null,
                End = HasMax ? Max : (TimeSpan?)null
            };
        }
    }
}
