using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.Services.CoreServices;

namespace FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices
{
    public class SimpleDataFetcherCacheDecorator : IDataFetchingService
    {
        private readonly IDataFetchingService m_dataFetchingService;
        private readonly ICacheProvider m_cacheProvider;

        public SimpleDataFetcherCacheDecorator(IDataFetchingService dataFetchingService, ICacheProvider cacheProvider)
        {
            m_dataFetchingService = dataFetchingService;
            m_cacheProvider = cacheProvider;
        }
        
        public async Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(FilterCollection filters)
        {
            if (m_cacheProvider.TryGetSerializingKey(filters, out FinlandiaHiihtoResultsCollection cachedRes))
            {
                return cachedRes;
            }

            var res = await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
            m_cacheProvider.SetSerializingKey(filters, res);

            return res;
        }
    }
}
