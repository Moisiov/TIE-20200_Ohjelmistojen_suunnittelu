using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.Utils;

namespace FJ.NUnitTests
{
    public class TestUtils
    {
        public static string GenerateRandomString(int length, int? seed = null)
        {
            var rand = seed.HasValue ? new Random(seed.Value) : new Random();
            
            // https://stackoverflow.com/a/1344242
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        public static TEnum GetRandomEnumValue<TEnum>(int? seed = null)
        {
            var rand = seed.HasValue ? new Random(seed.Value) : new Random();
            var values = EnumHelpers.GetEnumValues<TEnum>().ToArray();
            return values.ElementAt(rand.Next(values.Length - 1));
        }
    }
}
