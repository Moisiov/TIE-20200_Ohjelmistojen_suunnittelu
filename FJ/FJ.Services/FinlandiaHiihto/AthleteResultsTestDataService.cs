using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;
using FJ.Utils;

namespace FJ.Services.FinlandiaHiihto
{
    public class AthleteResultsTestDataService : IAthleteResultsService
    {
        private readonly IDataFetchingService m_dataFetchingService;
        
        public AthleteResultsTestDataService(IDataFetchingService dataFetchingService)
        {
            m_dataFetchingService = dataFetchingService;
        }
        
        public Task<FinlandiaHiihtoResultsCollection> GetAthleteResultsAsync(string athleteFirstName, string athleteLastName)
        {
            // TODO FilterCollectionin luonti ja dataFetchingServicen avulla näiden muuttujien tiedon haku APIlta.
            var resultRows = new FinlandiaHiihtoResultsCollection(PopulateResultRows());
            var athletePersonalInfo = new Person
            {
                FirstName = athleteFirstName,
                LastName = athleteLastName,
                City = "Hämeenlinna",
                Nationality = "FI",
                YearOfBirth = 1978
            };
            resultRows.Results.First().Athlete = athletePersonalInfo;
            
            return Task.FromResult(resultRows);
        }
        
        private static IEnumerable<FinlandiaHiihtoSingleResult> PopulateResultRows()
        {
            return new[]
            {
                new FinlandiaHiihtoSingleResult
                {    
                    Result = new TimeSpan(2, 34, 56),
                    PositionGeneral = 356,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass 
                    {
                        Distance = FinlandiaSkiingDistance.Thirty,
                        Style = FinlandiaSkiingStyle.Skate
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Thirty + "km " + 
                               FinlandiaSkiingStyle.Skate.GetDescription(),
                        Year = 2005
                    }
                },
                new FinlandiaHiihtoSingleResult
                {    
                    Result = new TimeSpan(3, 55, 5),
                    PositionGeneral = 225,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass 
                    {
                        Distance = FinlandiaSkiingDistance.Fifty,
                        Style = FinlandiaSkiingStyle.Classic
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Fifty + "km " + 
                               FinlandiaSkiingStyle.Classic.GetDescription(),
                        Year = 2005
                    }
                },
                new FinlandiaHiihtoSingleResult
                {    
                    Result = new TimeSpan(2, 21, 4),
                    PositionGeneral = 238,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass 
                    {
                        Distance = FinlandiaSkiingDistance.Thirty,
                        Style = FinlandiaSkiingStyle.Skate
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Thirty + "km " + 
                               FinlandiaSkiingStyle.Skate.GetDescription(),
                        Year = 2006
                    }
                },
                new FinlandiaHiihtoSingleResult
                {    
                    Result = new TimeSpan(3, 23, 22),
                    PositionGeneral = 200,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass 
                    {
                        Distance = FinlandiaSkiingDistance.Fifty,
                        Style = FinlandiaSkiingStyle.Classic
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Fifty + "km " + 
                               FinlandiaSkiingStyle.Classic.GetDescription(),
                        Year = 2006
                    }
                },
                new FinlandiaHiihtoSingleResult
                {    
                    Result = new TimeSpan(1, 59, 49),
                    PositionGeneral = 150,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass 
                    {
                        Distance = FinlandiaSkiingDistance.Thirty,
                        Style = FinlandiaSkiingStyle.Skate
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Thirty + "km " + 
                               FinlandiaSkiingStyle.Skate.GetDescription(),
                        Year = 2007
                    }
                },
                new FinlandiaHiihtoSingleResult
                {    
                    Result = new TimeSpan(1, 47, 46),
                    PositionGeneral = 99,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass 
                    {
                        Distance = FinlandiaSkiingDistance.Thirty,
                        Style = FinlandiaSkiingStyle.Skate
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Thirty + "km " + 
                               FinlandiaSkiingStyle.Skate.GetDescription(),
                        Year = 2008
                    }
                },
                new FinlandiaHiihtoSingleResult
                {    
                    Result = new TimeSpan(3, 19, 23),
                    PositionGeneral = 198,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass 
                    {
                        Distance = FinlandiaSkiingDistance.Fifty,
                        Style = FinlandiaSkiingStyle.Classic
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Fifty + "km " + 
                               FinlandiaSkiingStyle.Classic.GetDescription(),
                        Year = 2008
                    }
                },
                new FinlandiaHiihtoSingleResult
                {    
                    Result = new TimeSpan(7, 32, 33),
                    PositionGeneral = 245,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass 
                    {
                        Distance = FinlandiaSkiingDistance.Hundred,
                        Style = FinlandiaSkiingStyle.Classic
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Hundred + "km " + 
                               FinlandiaSkiingStyle.Classic.GetDescription(),
                        Year = 2009
                    }
                },
            };
        }
    }
}
