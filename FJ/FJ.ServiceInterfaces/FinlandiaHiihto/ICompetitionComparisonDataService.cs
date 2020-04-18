using System;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.ServiceInterfaces.FinlandiaHiihto
{
    public interface ICompetitionComparisonDataService
    {
        Task<FinlandiaHiihtoResultsCollection> GetCompetitionComparisonData(FilterCollection filters);
    }
}
