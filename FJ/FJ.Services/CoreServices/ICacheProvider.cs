using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FJ.Services.CoreServices
{
    public interface ICacheProvider
    {
        T Get<T>(object key, out T value);
        T GetSerializingKey<T>(object key, out T value);
        
        bool TryGet<T>(object key, out T value);
        bool TryGetSerializingKey<T>(object key, out T value);
        
        void Set<T>(object key, T value, DateTimeOffset absoluteExpiry);
        void SetSerializingKey<T>(object key, T value, DateTimeOffset absoluteExpiry);
        
        void Set<T>(object key, T value, uint minutesToExpiration = 60);
        void SetSerializingKey<T>(object key, T value, uint minutesToExpiration = 60);

        void DoInvalidateAll();
    }
}
