using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.ServiceInterfaces.FinlandiaHiihto
{
    public interface ICompetitionOccasionDataService
    {
        Task<FinlandiaHiihtoResultsCollection> GetCompetitionOccasionResultsAsync(int year);
        Task<IEnumerable<FinlandiaHiihtoResultsCollection>> GetOrderedCompetitionListsAsync(int year);
        Task<IEnumerable<FinlandiaHiihtoResultsCollection>> GetOrderedCompetitionTeamListAsync(int year);
    }
}
