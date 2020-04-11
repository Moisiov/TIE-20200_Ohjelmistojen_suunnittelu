using System.Collections.Generic;
using FJ.DomainObjects.Enums;
using FJ.DomainObjects.Filters.Core;

namespace FJ.DomainObjects.Filters.CommonFilters
{
    public class GenderFilter : EnumFilterBase<Gender, GenderFilter>
    {
        public override string ShortName => "Person's gender";

        public GenderFilter(IEnumerable<Gender> genders)
            : base(genders)
        {
        }
    }

    public class MaleFilter : FixedFilterBase<MaleFilter>
    {
        public override string ShortName => "Male";
        public override string Description => "Person's gender is male";
    }
    
    public class FemaleFilter : FixedFilterBase<FemaleFilter>
    {
        public override string ShortName => "Female";
        public override string Description => "Person's gender is female";
    }
}
