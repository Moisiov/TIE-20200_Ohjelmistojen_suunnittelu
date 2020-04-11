using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FJ.DomainObjects
{
    public class DateRange : ICloneable
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public bool NotEmpty => Start.HasValue || End.HasValue;
        public bool HasStartAndEnd => Start.HasValue && End.HasValue;

        public DateRange()
        {
        }

        public DateRange(DateTime? start, DateTime? end)
        {
            Start = start;
            End = end;
        }

        public object Clone()
        {
            return new DateRange
            {
                Start = Start,
                End = End
            };
        }

        public bool ContainsDate(DateTime date)
        {
            return date.Date >= (Start?.Date ?? DateTime.MinValue) && date.Date <= (End?.Date ?? DateTime.MaxValue);
        }

        public override string ToString()
        {
            return $"{Start?.ToShortDateString()}–{End?.ToShortDateString()}";
        }
    }
}
