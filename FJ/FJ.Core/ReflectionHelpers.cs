using System;
using System.Collections.Generic;
using System.Linq;

namespace FJ.Core
{
    public static class ReflectionHelpers
    {
        /// <summary>
        /// Get types which implement T
        /// </summary>
        /// <typeparam name="T">Type of which targets have to be assigned from</typeparam>
        /// <param name="predicate">Predicate for extra filtering</param>
        /// <returns>IEnumerable of matching types</returns>
        public static IEnumerable<Type> GetClasses<T>(Func<Type, bool> predicate = null)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && typeof(T).IsAssignableFrom(p) && (predicate?.Invoke(p)).IsIn(null, true));
        }
    }
}
