using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;

namespace FJ.Client.CompetitionOccasion
{
    public class CompetitionOccasionModel
    {
        private readonly ICompetitionOccasionDataService m_competitionOccasionDataService;
        
        public List<CompetitionRowItemModel> CompetitionList { get; private set; }
        public int TotalParticipants { get; private set; }
        public int TotalCompetitions { get; private set; }
        
        public IEnumerable<(string Nationality, int TotalCount)> NationalityDistribution { get; private set; }

        public CompetitionOccasionModel(ICompetitionOccasionDataService competitionOccasionDataService)
        {
            m_competitionOccasionDataService = competitionOccasionDataService;
        }

        public async Task GetOccasionData(int year)
        {
            // Get all occasions result rows and calculate unique athletes.
            var allResultRows = 
                await m_competitionOccasionDataService.GetCompetitionOccasionResultsAsync(year);
            TotalParticipants = GetUniqueParticipantsAmount(allResultRows.Results);
            
            // Get nationality distribution data.
            NationalityDistribution = 
                await m_competitionOccasionDataService.GetCompetitionOccasionNationalityDistributionAsync(year);

            // Get result collections for each individual competition that were held during the occasion.
            var resultRowsByCompetition = 
                (await m_competitionOccasionDataService.GetOrderedCompetitionListsAsync(year)).ToList();
            TotalCompetitions = resultRowsByCompetition.Count;
            CompetitionList = null;
            
            // Get team results for each individual competition that were held during the occasion.
            var teamResultRowsByCompetition = 
                await m_competitionOccasionDataService.GetOrderedCompetitionTeamListAsync(year);
            
            // Compile CompetitionRowItemModel from the fetched data.
            CompetitionList = resultRowsByCompetition
                .Select(competition => new CompetitionRowItemModel
                {
                    CompetitionInfo = competition.Results.First().CompetitionInfo,
                    TotalParticipants = GetUniqueParticipantsAmount(competition.Results),
                    FirstPlaceCompetitor = new CompetitorSummaryItemModel
                    {
                        Competitor = competition.Results.First().Athlete,
                        Result = competition.Results.First().Result.ToString(@"hh\:mm\:ss\.ff"),
                        AverageSpeed = ((int)competition.Results.First().CompetitionClass.Distance
                                        / competition.Results.First().Result.TotalHours).ToString("0.00")
                    },
                    LastPlaceCompetitor = new CompetitorSummaryItemModel
                    {
                        Competitor = competition.Results.Last().Athlete,
                        Result = competition.Results.Last().Result.ToString(@"hh\:mm\:ss\.ff"),
                        AverageSpeed = ((int)competition.Results.Last().CompetitionClass.Distance
                                        / competition.Results.Last().Result.TotalHours).ToString("0.00")
                    },
                    Top10Teams = teamResultRowsByCompetition
                        .FirstOrDefault(x
                            => x.Results.FirstOrDefault()?.CompetitionInfo.Name ==
                               competition.Results.First().CompetitionInfo.Name)
                        ?.Results
                        .Take(10)
                        .Select((resultRow, i) => new Top10TeamItemModel
                        {
                            Position = i + 1,
                            Name = resultRow.Team,
                            Result = resultRow.Result.ToString(@"hh\:mm\:ss\.ff")
                        })
                }).ToList();
        }

        private static int GetUniqueParticipantsAmount(IEnumerable<FinlandiaHiihtoSingleResult> res)
        {
            return res.GroupBy(x => x.Athlete.FullName).ToList().Count;
        }
    }
}
