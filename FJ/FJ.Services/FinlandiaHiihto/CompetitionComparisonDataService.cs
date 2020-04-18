using System;
using System.Threading.Tasks;
using FinlandiaHiihtoAPI.Exceptions;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.CoreServices;
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
            try
            {
                return await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
            }
            // If competition has more than 10k results try to slice the search to smaller pieces.
            catch (TooMuchResultsExceptions)
            {
                FilterUtils.SliceSearchWithName(filters);
                return await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
            }
        }
    }
}
