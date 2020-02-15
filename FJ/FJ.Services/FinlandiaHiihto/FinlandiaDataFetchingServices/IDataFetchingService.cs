using System;
using System.Threading.Tasks;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices
{
    public interface IDataFetchingService
    {
        public Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResults(FinlandiaHiihtoSearchArgs args);
    }
}
