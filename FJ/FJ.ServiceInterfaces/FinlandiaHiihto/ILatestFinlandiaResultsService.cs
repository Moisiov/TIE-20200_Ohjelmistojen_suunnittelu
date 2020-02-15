using System;
using System.Threading.Tasks;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.ServiceInterfaces.FinlandiaHiihto
{
    public interface ILatestFinlandiaResultsService
    {
        public Task<FinlandiaHiihtoResultsCollection> GetLatestFinlandiaResults();
    }
}
