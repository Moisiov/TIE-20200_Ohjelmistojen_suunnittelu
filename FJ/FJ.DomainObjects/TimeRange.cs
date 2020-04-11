using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FJ.DomainObjects
{
    public class TimeRange : ICloneable
    {
        public TimeSpan? Start { get; set; }
        public TimeSpan? End { get; set; }

        public bool NotEmpty => Start.HasValue || End.HasValue;
        public bool HasStartAndEnd => Start.HasValue && End.HasValue;

        public TimeRange()
        {
        }

        public TimeRange(TimeSpan? start, TimeSpan? end)
        {
            Start = start;
            End = end;
        }

        public object Clone()
        {
            return new TimeRange
            {
                Start = Start,
                End = End
            };
        }

        public bool ContainsTime(TimeSpan time)
        {
            return time >= (Start ?? TimeSpan.MinValue) && time <= (End ?? TimeSpan.MaxValue);
        }

        public override string ToString()
        {
            var timeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            return $"{Start?.ToString(timeFormat)}–{End?.ToString(timeFormat)}";
        }
    }
}
