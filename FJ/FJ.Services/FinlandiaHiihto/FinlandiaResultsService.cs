using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;

namespace FJ.Services.FinlandiaHiihto
{
    public class FinlandiaResultsService : IFinlandiaResultsService
    {
        private readonly IDataFetchingService m_dataFetchingService;

        public FinlandiaResultsService(IDataFetchingService dataFetchingService)
        {
            m_dataFetchingService = dataFetchingService;
        }
        
        public async Task<FinlandiaHiihtoResultsCollection> GetFinlandiaResultsAsync(FilterCollection filters)
        {
            return await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
        }
    }
}
