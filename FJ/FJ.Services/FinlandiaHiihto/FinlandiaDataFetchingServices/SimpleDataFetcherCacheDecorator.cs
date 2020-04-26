using System;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.Services.CoreServices;

namespace FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices
{
    public class SimpleDataFetcherCacheDecorator : DataFetcherDecoratorBase
    {
        private readonly ICacheProvider m_cacheProvider;

        public SimpleDataFetcherCacheDecorator(IDataFetchingService dataFetchingService, ICacheProvider cacheProvider)
            : base(dataFetchingService)
        {
            m_cacheProvider = cacheProvider;
        }
        
        public override async Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(FilterCollection filters)
        {
            if (m_cacheProvider.TryGetSerializingKey(filters, out FinlandiaHiihtoResultsCollection cachedRes))
            {
                return cachedRes;
            }

            var res = await DataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
            m_cacheProvider.SetSerializingKey(filters, res);

            return res;
        }
    }
}
