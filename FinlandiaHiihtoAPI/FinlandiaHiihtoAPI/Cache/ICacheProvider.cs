using System;
using System.Collections.Generic;

namespace FinlandiaHiihtoAPI.Cache
{
    public interface ICacheProvider
    {
        bool TryGetSerializingArgs(FinlandiaHiihtoAPISearchArgs key,
            out IEnumerable<FinlandiaHiihtoAPISearchResultRow> value);
        
        void SetSerializingKey(FinlandiaHiihtoAPISearchArgs key, IEnumerable<FinlandiaHiihtoAPISearchResultRow>  value, 
            DateTimeOffset absoluteExpiry); 
        void SetSerializingKey(FinlandiaHiihtoAPISearchArgs key, IEnumerable<FinlandiaHiihtoAPISearchResultRow>  value, 
            uint minutesToExpiration = 60);

        void DoInvalidateAll();
    }
}