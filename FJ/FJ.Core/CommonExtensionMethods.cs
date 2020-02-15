using System;
using System.Linq;

namespace FJ.Core
{
    public static class CommonExtensionMethods
    {
        /// <summary>
        /// Checks if given key value is some of the values given as parameter
        /// </summary>
        /// <typeparam name="T">Type of key value to be looked for</typeparam>
        /// <param name="key">Key value to be looked for</param>
        /// <param name="allowedValues">Values the key will be checked agains</param>
        /// <returns>Key equals with at least one of the given values</returns>
        public static bool IsIn<T>(this T key, params T[] allowedValues)
        {
            return allowedValues != null && allowedValues.Contains(key);
        }
    }
}
