using System;
using System.Collections.Generic;
using System.Linq;

namespace FJ.Utils
{
    public static class EnumHelpers
    {
        public static IEnumerable<TEnum> GetEnumValues<TEnum>()
            where TEnum : struct, IConvertible
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
