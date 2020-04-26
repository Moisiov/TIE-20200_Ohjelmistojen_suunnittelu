using System;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices
{
    public abstract class DataFetcherDecoratorBase : IDataFetchingService
    {
        protected readonly IDataFetchingService DataFetchingService;

        protected DataFetcherDecoratorBase(IDataFetchingService dataFetchingService)
        {
            DataFetchingService = dataFetchingService;
        }
        
        public virtual Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(FilterCollection filters)
        {
            return DataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
        }
    }
}
