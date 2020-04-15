using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;

namespace FJ.Client.Athlete
{
    public class AthleteCardModel
    {
        private readonly IAthleteResultsService m_athleteResultsService;
        
        public FinlandiaHiihtoResultsCollection AthletesResultRows { get; private set; }
        public Person Athlete => AthletesResultRows.Results.FirstOrDefault()?.Athlete;
        
        public AthleteCardModel(IAthleteResultsService athleteResultsService)
        {
            m_athleteResultsService = athleteResultsService;
        }
        
        public async Task GetAthleteData(string athleteFirstName, string athleteLastName)
        {
            AthletesResultRows = await m_athleteResultsService.GetAthleteResultsAsync(athleteFirstName, athleteLastName);
        }
    }
}
