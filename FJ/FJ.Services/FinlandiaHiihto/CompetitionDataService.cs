using System;
using System.Linq;
using System.Threading.Tasks;
using FinlandiaHiihtoAPI.Exceptions;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.CoreServices;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;

namespace FJ.Services.FinlandiaHiihto
{
    public class CompetitionDataService : ICompetitionDataService
    {
        private readonly IDataFetchingService m_dataFetchingService;

        public CompetitionDataService(IDataFetchingService dataFetchingService)
        {
            m_dataFetchingService = dataFetchingService;
        }

        public async Task<FinlandiaHiihtoResultsCollection> GetCompetitionData(FilterCollection filters)
        {
            try
            {
                return await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
            }
            // If competition has more than 10k results try to slice the search to smaller pieces.
            catch (TooMuchResultsExceptions e)
            {
                if (filters.Count > 2)
                {
                    throw e;
                }
                FilterUtils.SliceSearchWithName(filters);
                var res = await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
                res.Results = res.Results.Distinct();
                return res;
            }
        }
    }
}
