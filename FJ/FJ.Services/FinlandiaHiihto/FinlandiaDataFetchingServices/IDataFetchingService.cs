using System;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices
{
    public interface IDataFetchingService
    {
        Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(FinlandiaHiihtoSearchArgs args);
        Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(FilterCollection filters);
    }
}
