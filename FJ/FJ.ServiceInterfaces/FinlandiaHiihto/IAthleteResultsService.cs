using System;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.ServiceInterfaces.FinlandiaHiihto
{
    public interface IAthleteResultsService
    {
        Task<FinlandiaHiihtoResultsCollection> GetAthleteResultsAsync(string athleteFirstName, string athleteLastName);
    }
}
