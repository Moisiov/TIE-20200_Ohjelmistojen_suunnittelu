using System;
using System.Threading.Tasks;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;
using FJ.Utils;

namespace FJ.Services.FinlandiaHiihto
{
    public class LatestFinlandiaResultsService : ILatestFinlandiaResultsService
    {
        private readonly IDataFetchingService m_dataFetchingService;

        public LatestFinlandiaResultsService(IDataFetchingService dataFetchingService)
        {
            m_dataFetchingService = dataFetchingService;
        }

        public async Task<FinlandiaHiihtoResultsCollection> GetLatestFinlandiaResultsAsync()
        {
            var currentYear = DateTime.Today.Year;
            var args = new FinlandiaHiihtoSearchArgs
            {
                CompetitionYears = currentYear.ToMany()
            };

            try
            {
                return await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(args);
            }
            catch (ArgumentException)
            {
                args = new FinlandiaHiihtoSearchArgs
                {
                    CompetitionYears = (currentYear - 1).ToMany()
                };

                return await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(args);
            }
            
        }
    }
}
