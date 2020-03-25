using System;
using FJ.Utils;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace FJ.Services.CoreServices
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private IMemoryCache m_cache;

        public MemoryCacheProvider()
        {
            m_cache = new MemoryCache(new MemoryCacheOptions());
        }

        public T Get<T>(object key, out T value)
        {
            return TryGet(key, out value) ? value : default;
        }

        public T GetSerializingKey<T>(object key, out T value)
        {
            return Get(JsonConvert.SerializeObject(key, Formatting.Indented), out value);
        }

        public bool TryGet<T>(object key, out T value)
        {
            return m_cache.TryGetValue(key, out value);
        }

        public bool TryGetSerializingKey<T>(object key, out T value)
        {
            return TryGet(JsonConvert.SerializeObject(key, Formatting.Indented), out value);
        }

        public void Set<T>(object key, T value, DateTimeOffset absoluteExpiry)
        {
            m_cache.Set(key, value, absoluteExpiry);
        }

        public void SetSerializingKey<T>(object key, T value, DateTimeOffset absoluteExpiry)
        {
            Set(JsonConvert.SerializeObject(key, Formatting.Indented), value, absoluteExpiry);
        }

        public void Set<T>(object key, T value, uint minutesToExpiration = 60)
        {
            m_cache.Set(key, value, TimeSpan.FromMinutes(minutesToExpiration));
        }

        public void SetSerializingKey<T>(object key, T value, uint minutesToExpiration = 60)
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
