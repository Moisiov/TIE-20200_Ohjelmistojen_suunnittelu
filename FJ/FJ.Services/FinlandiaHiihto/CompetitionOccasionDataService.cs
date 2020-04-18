using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinlandiaHiihtoAPI.Exceptions;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Filters;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.CoreServices;
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
            if (m_year == year && m_resultsCollection != null)
            {
                return m_resultsCollection;
            }
            
            m_year = year;
            
            // Set year as search filter to fetch all the data about the competition occasion.
            var yearFilter = new FinlandiaCompetitionYearsFilter(m_year.ToMany());
            var filters = new FilterCollection(yearFilter);
            
            try
            {
                m_resultsCollection = await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
            }
            // If year has more than 10k results try to slice the search to smaller pieces.
            catch (TooMuchResultsExceptions)
            {
                FilterUtils.SliceSearchWithName(filters);
                m_resultsCollection = await m_dataFetchingService.GetFinlandiaHiihtoResultsAsync(filters);
            }
            
            return m_resultsCollection;
        }
        
        public async Task<IEnumerable<FinlandiaHiihtoResultsCollection>> GetOrderedCompetitionListsAsync(int year)
        {
            if (m_resultsCollection == null || year != m_year)
            {
                await GetCompetitionOccasionResultsAsync(year);
                m_year = year;
            }

            // Create individual result collection for each competition and order collections by athletes position.
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

            // Create result collection for each competition's team results.
            var res = m_orderedCompetitionLists
                .Select(competition 
                    => new FinlandiaHiihtoResultsCollection(competition.Results
                .GroupBy(x => x.Team)
                .Where(x => x.Key != null && x.Count() >= 4)
                .SelectMany(x =>
                {
                    var teamAthletesOrdered = x.OrderBy(y => y.Result).ToList();
                    var competitionClass = x.First().CompetitionClass;
                    var competitionInfo = x.First().CompetitionInfo;
                    var teamsList = new List<FinlandiaHiihtoSingleResult>();
                    
                    // If team has more than 4 athletes create multiple teams by taking sets of 4 athletes.
                    // Top 4 athletes form team number 1, next 4 form team number 2 and so on.
                    for (var i = 0; i < teamAthletesOrdered.Count; i += 4)
                    {
                        var teamMembers = teamAthletesOrdered.Skip(i).Take(4).ToList();
                        
                        // If less than 4 athletes left, no more teams can be formed.
                        if (teamMembers.Count < 4)
                        {
                            continue;
                        }
                        
                        // Create new result row by calculating athletes' result times together. 
                        teamsList.Add(new FinlandiaHiihtoSingleResult
                        {
                            Team = $"{x.Key} {((i / 4) + 1).ToString()}",
                            Result = new TimeSpan(teamMembers.Sum(r => r.Result.Ticks)),
                            CompetitionClass = competitionClass,
                            CompetitionInfo = competitionInfo
                        });
                    }
                    
                    return teamsList;
                })
                .ToList()
                .OrderBy(x => x.Result)));
            
            return await Task.FromResult(res);
        }
    }
}
