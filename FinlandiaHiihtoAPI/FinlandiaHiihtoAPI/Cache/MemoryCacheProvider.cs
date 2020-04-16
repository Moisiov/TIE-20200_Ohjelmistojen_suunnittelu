using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FinlandiaHiihtoAPI.Enums;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace FinlandiaHiihtoAPI.Cache
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private IMemoryCache m_cache;

        public MemoryCacheProvider()
        {
            m_cache = new MemoryCache(new MemoryCacheOptions());
        }

        private bool TryGet(string key, out IEnumerable<FinlandiaHiihtoAPISearchResultRow> value)
        {
            return m_cache.TryGetValue(key, out value);
        }

        public bool TryGetSerializingArgs(FinlandiaHiihtoAPISearchArgs key, 
            out IEnumerable<FinlandiaHiihtoAPISearchResultRow> value)
        {
            if (TryGet(JsonConvert.SerializeObject(key, Formatting.Indented), out value))
            {
                return true;
            }

            if (!TryGetSerializingArgsRecursively(key, out value))
            {
                return false;
            }
            value = FilterBroaderResultsWithArgs(key, value);
            return true;
        }
        
        private bool TryGetSerializingArgsRecursively(FinlandiaHiihtoAPISearchArgs key, 
            out IEnumerable<FinlandiaHiihtoAPISearchResultRow> value)
        {
            // Loop all key's properties.
            foreach (var prop in key.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanRead || !prop.CanWrite || prop.GetIndexParameters().Any())
                {
                    continue;
                }
                
                // Check if property's value is not null.
                var propValue = prop.GetValue(key, null);
                if (propValue == null)
                {
                    continue;
                }
                
                // Check recursively if key that is less restrictive exists by setting a property
                // temporarily to null.
                prop.SetValue(key, null);
                if (TryGet(JsonConvert.SerializeObject(key, Formatting.Indented), out value))
                {
                    prop.SetValue(key, propValue);
                    return true;
                }
                    
                if (TryGetSerializingArgsRecursively(key, out value))
                {
                    prop.SetValue(key, propValue);
                    return true;
                }
                prop.SetValue(key, propValue);
            }
            
            value = null;
            return false;
        }

        private static IEnumerable<FinlandiaHiihtoAPISearchResultRow> FilterBroaderResultsWithArgs(
            FinlandiaHiihtoAPISearchArgs args, 
            IEnumerable<FinlandiaHiihtoAPISearchResultRow> results)
        {
            return results.Where(x =>
            
                (args.Year == null || x.Year == args.Year) && 
                CheckNameFilter(x.FullName, args.FirstName, args.LastName)
                && (args.CompetitionType == null
                    || x.StyleAndDistance == Enum.GetName(typeof(FinlandiaCompetitionType), args.CompetitionType))
                && (args.AgeGroup == null || CheckAgeGroupFilter(x.BornYear, x.Year, args.AgeGroup))
                && (args.CompetitorHomeTown == null || x.HomeTown.Contains(args.CompetitorHomeTown))
                && (args.Team == null || x.Team.Contains(args.Team))
                && (args.Gender == null || x.Gender == args.Gender)
                && (args.Nationality == null || x.Nationality == args.Nationality));
        }
        
        private static bool CheckNameFilter(string fullName, string firstName, string lastName)
        {
            var firstFromFullName = string.Join(" ", fullName
                .Split()
                .Skip(1)
                .ToArray());
            var lastFromFullName = string.Join(" ", fullName
                .Split()
                .Reverse()
                .Skip(1)
                .Reverse()
                .ToArray());
            return (firstName == null || firstFromFullName.Contains(firstName)) && 
                   (lastName == null || lastFromFullName.Contains(lastName));
        }
        
        private static bool CheckAgeGroupFilter(int? bornYear, int competitionYear, FinlandiaAgeGroup? ageGroup)
        {
            var age = competitionYear - bornYear;
            return ageGroup switch
            {
                FinlandiaAgeGroup.LessThan35 => age < 35,
                FinlandiaAgeGroup.ThirtyFive => age >= 35 && age < 40,
                FinlandiaAgeGroup.Forty => age >= 40 && age < 45,
                FinlandiaAgeGroup.FortyFive => age >= 45 && age < 50,
                FinlandiaAgeGroup.Fifty => age >= 50 && age < 55,
                FinlandiaAgeGroup.FiftyFive => age >= 55 && age < 60,
                FinlandiaAgeGroup.Sixty => age >= 60 && age < 65,
                FinlandiaAgeGroup.SixtyFive => age >= 65 && age < 70,
                FinlandiaAgeGroup.Seventy => age >= 70 && age < 75,
                FinlandiaAgeGroup.SeventyFive => age >= 75 && age < 80,
                FinlandiaAgeGroup.OverEighty => age >= 80,
                _ => false
            };
        }

        private void Set(string key, IEnumerable<FinlandiaHiihtoAPISearchResultRow> value, DateTimeOffset absoluteExpiry)
        {
            m_cache.Set(key, value, absoluteExpiry);
        }

        public void SetSerializingKey(FinlandiaHiihtoAPISearchArgs key, IEnumerable<FinlandiaHiihtoAPISearchResultRow> value, 
            DateTimeOffset absoluteExpiry)
        {
            Set(JsonConvert.SerializeObject(key, Formatting.Indented), value, absoluteExpiry);
        }

        private void Set(string key, IEnumerable<FinlandiaHiihtoAPISearchResultRow> value, uint minutesToExpiration = 60)
        {
            m_cache.Set(key, value, TimeSpan.FromMinutes(minutesToExpiration));
        }

        public void SetSerializingKey(FinlandiaHiihtoAPISearchArgs key, IEnumerable<FinlandiaHiihtoAPISearchResultRow> value, 
            uint minutesToExpiration = 60)
        {
            Set(JsonConvert.SerializeObject(key, Formatting.Indented), value, minutesToExpiration);
        }

        public void DoInvalidateAll()
        {
            // TODO Not quite sure this is legit. It works though
            m_cache.Dispose();
            m_cache = new MemoryCache(new MemoryCacheOptions());
        }
    }
}
