namespace FJ.DomainObjects.Filters.Core
{
    public abstract class RangeFilterBase<TItem, TFilter> : FilterBase<TFilter>
        where TFilter : RangeFilterBase<TItem, TFilter>
    {
        public TItem Min { get; }
        public TItem Max { get; }

        protected RangeFilterBase(TItem min, TItem max)
        {
            Min = min;
            Max = max;
        }

        public override bool Equals(TFilter obj)
        {
            return obj == null
                ? Min == null && Max == null
                : Equals(Min, obj.Min) && Equals(Max, obj.Max);
        }
    }
}
