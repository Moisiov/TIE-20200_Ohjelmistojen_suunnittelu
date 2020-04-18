using System;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;

namespace FJ.Services.FinlandiaHiihto
{
    public class CompetitionComparisonDataService : ICompetitionComparisonDataService
    {
        private readonly IDataFetchingService m_dataFetchingService;

        public CompetitionComparisonDataService(IDataFetchingService dataFetchingService)
        {
            m_dataFetchingService = dataFetchingService;
        }

        public async Task<FinlandiaHiihtoResultsCollection> GetCompetitionComparisonData(FilterCollection filters)
        {
            return await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
        }
    }
}
