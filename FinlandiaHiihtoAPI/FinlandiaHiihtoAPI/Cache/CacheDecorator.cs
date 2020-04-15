using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinlandiaHiihtoAPI.Cache
{
    public class CacheDecorator : IFinlandiaHiihtoAPI
    {
        private readonly IFinlandiaHiihtoAPI m_api;
        private readonly ICacheProvider m_cacheProvider;

        public CacheDecorator(IFinlandiaHiihtoAPI api, ICacheProvider cacheProvider)
        {
            m_api = api;
            m_cacheProvider = cacheProvider;
        }
        
        public async Task<IEnumerable<FinlandiaHiihtoAPISearchResultRow>> GetData(FinlandiaHiihtoAPISearchArgs args)
        {
            if (m_cacheProvider.TryGetSerializingArgs(args, out var cachedRes))
            {
                return cachedRes;
            }

            var res = await m_api.GetData(args);
            m_cacheProvider.SetSerializingKey(args, res);
            return res;
        }
    }
}