using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;
using FJ.Utils;

namespace FJ.Services.FinlandiaHiihto
{
    public class CompetitionOccasionDataService : ICompetitionOccasionDataService
    {
        private readonly IDataFetchingService m_dataFetchingService;
        
        private int m_year;
        private FinlandiaHiihtoResultsCollection m_resultsCollection;
        private IEnumerable<FinlandiaHiihtoResultsCollection> m_orderedCompetitionLists;
        
        public CompetitionOccasionDataService(IDataFetchingService dataFetchingService)
        {
            m_dataFetchingService = dataFetchingService;
        }
        
        public async Task<FinlandiaHiihtoResultsCollection> GetCompetitionOccasionResultsAsync(int year)
        {
            m_year = year;
            
            // TODO Actual filter args.
            var filters = new FilterCollection();
            m_resultsCollection = await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
            
            return m_resultsCollection;
        }
        
        public async Task<IEnumerable<FinlandiaHiihtoResultsCollection>> GetOrderedCompetitionListsAsync(int year)
        {
            if (m_resultsCollection == null || year != m_year)
            {
                await GetCompetitionOccasionResultsAsync(year);
                m_year = year;
            }

            m_orderedCompetitionLists = m_resultsCollection.Results
                .GroupBy(x => x.CompetitionInfo.Name)
                .Select(x
                    => new FinlandiaHiihtoResultsCollection(x
                        .ToList()
                        .OrderBy(y => y.PositionGeneral)));
            
            return await Task.FromResult(m_orderedCompetitionLists);
        }
        
        public async Task<IEnumerable<FinlandiaHiihtoResultsCollection>> GetOrderedCompetitionTeamListAsync(int year)
        {
            if (m_orderedCompetitionLists == null || year != m_year)
            {
                await GetOrderedCompetitionListsAsync(year);
                m_year = year;
            }

            var res = m_orderedCompetitionLists
                .Select(competition 
                    => new FinlandiaHiihtoResultsCollection(competition.Results
                .GroupBy(x => x.Team)
                .Where(x => x.Key != null && x.Count() >= 4)
                .SelectMany(x =>
                {
                    var teamAthletesOrdered = x.OrderBy(y => y.Result);
                    var competitionClass = x.First().CompetitionClass;
                    var competitionInfo = x.First().CompetitionInfo;
                    var teamsList = new List<FinlandiaHiihtoSingleResult>();
                    for (var i = 0; i < teamAthletesOrdered.Count(); i += 4)
                    {
                        var teamMemebers = teamAthletesOrdered.Skip(i).Take(4);
                        if (teamMemebers.Count() < 4)
                        {
                            continue;
                        }
                        teamsList.Add(new FinlandiaHiihtoSingleResult
                        {
                            Team = x.Key + " " + ((i / 4) + 1),
                            Result = new TimeSpan(teamMemebers.Sum(r => r.Result.Ticks)),
                            CompetitionClass = competitionClass,
                            CompetitionInfo = competitionInfo
                        });
                    }
                    return teamsList;
                })
                .ToList()
                .OrderBy(x => x.PositionGeneral)));
            
            return await Task.FromResult(res);
        }
    }
}
