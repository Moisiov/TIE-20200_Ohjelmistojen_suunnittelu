using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace FJ.DomainObjects.Filters
{
    public class FilterSearchComposer<TSearch>
        where TSearch : new()
    {
        public List<TSearch> Searches { get; protected set; }
        public int SearchesCount => Searches.Count;

        public FilterSearchComposer()
        {
            Searches = new[] { new TSearch() }.ToList();
        }
        
        public FilterSearchComposer<TSearch> EscalateBy<T>(string propertyName, IEnumerable<T> values)
        {
            var escalatedCollection = new List<TSearch>();
            if (!TryGetPropertyInfo(propertyName, out var propInfo))
            {
                throw new ArgumentException($"{propInfo.Name} not exists in class {typeof(TSearch).Name}");
            }

            var valuesArr = values as T[] ?? values.ToArray();
            foreach (var existingSearch in Searches)
            {
                if (propInfo.GetValue(existingSearch, null) != null)
                {
                    escalatedCollection.Add(existingSearch);
                }
                
                foreach (var value in valuesArr)
                {
                    var escalation = Clone(existingSearch);
                    propInfo.SetValue(escalation, value);
                    escalatedCollection.Add(escalation);
                }
            }
            
            Searches = new List<TSearch>(escalatedCollection);
            return this;
        }

        public FilterSearchComposer<TSearch> AggregateBy<T>(string propertyName, T value, bool throwIfValueExists)
        {
            if (!TryGetPropertyInfo(propertyName, out var propInfo))
            {
                throw new ArgumentException($"{propInfo.Name} not exists in class {typeof(TSearch).Name}");
            }

            foreach (var search in Searches)
            {
                if (propInfo.GetValue(search, null) != null)
                {
                    if (throwIfValueExists)
                    {
                        throw new ArgumentException($"{propInfo.Name} already exists");
                    }
                    
                    continue;
                }
                
                propInfo.SetValue(search, value);
            }

            return this;
        }

        private static bool TryGetPropertyInfo(string propertyName, out PropertyInfo propInfo)
        {
            propInfo = typeof(TSearch).GetProperty(propertyName);
            return propInfo != null;
        }

        private static TSearch Clone(TSearch obj)
        {
            var inst = typeof(TSearch).GetMethod(
                "MemberwiseClone",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return (TSearch)inst?.Invoke(obj, null);
        }
    }
}
