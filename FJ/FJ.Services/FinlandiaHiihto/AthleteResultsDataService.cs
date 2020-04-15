using System;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Filters;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;
using FJ.Utils;

namespace FJ.Services.FinlandiaHiihto
{
    public class AthleteResultsDataService : IAthleteResultsService
    {
        private readonly IDataFetchingService m_dataFetchingService;
        
        public AthleteResultsDataService(IDataFetchingService dataFetchingService)
        {
            m_dataFetchingService = dataFetchingService;
        }
        
        public async Task<FinlandiaHiihtoResultsCollection> GetAthleteResultsAsync(
            string athleteFirstName,
            string athleteLastName)
        {
            var firstNameFilter = new FinlandiaFirstNamesFilter(athleteFirstName.ToMany());
            var lastNameFilter = new FinlandiaLastNamesFilter(athleteLastName.ToMany());
            var fullNameFilter = new FinlandiaFullNameFilter($"{athleteLastName} {athleteFirstName}");
            var filterCollection = new FilterCollection(firstNameFilter, lastNameFilter, fullNameFilter);
            
            var resultRows = await m_dataFetchingService
                .GetFinlandiaHiihtoResultsAsync(filterCollection);
            
            return resultRows;
        }
    }
}
