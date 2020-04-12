using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinlandiaHiihtoAPI;
using FJ.DomainObjects;
using FJ.DomainObjects.Enums;
using FJ.DomainObjects.Filters;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.Services.CoreServices;
using FJ.Utils;

namespace FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices
{
    public class FinlandiaAPIDataFetchingService : IDataFetchingService
    {
        private const int c_searchCountLimit = 1000;
        private readonly IFinlandiaHiihtoAPI m_api;
        private readonly IFilterImplementationProvider m_filterImplementationProvider;

        public FinlandiaAPIDataFetchingService(IFinlandiaHiihtoAPI api, IFilterImplementationProvider filterImplementationProvider)
        {
            m_api = api;
            m_filterImplementationProvider = filterImplementationProvider;
        }

        public async Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(FinlandiaHiihtoSearchArgs args)
        {
            // TODO proof of concept, ei huomioi argseja vielä kuin ensimmäisen vuoden osalta
            var raw = await m_api.GetData(new FinlandiaHiihtoAPISearchArgs
            {
                Year = args.CompetitionYears.First()
            });
            
            return new FinlandiaHiihtoResultsCollection(ParseRawResult(raw));
        }

        public async Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(FilterCollection filters)
        {
            var searchTasks = new FilterSearchComposer<FinlandiaHiihtoAPISearchArgs>()
                .ApplyFilters(filters, m_filterImplementationProvider)
                .Searches
                .Select(x => m_api.GetData(x))
                .ToArray();

            if (searchTasks.Length > c_searchCountLimit)
            {
                throw new Exception("Too wide args"); // TODO dunno if good
            }

            var searchResultRows = await Task.WhenAll(searchTasks);
            return new FinlandiaHiihtoResultsCollection(ParseRawResult(
                searchResultRows.SelectMany(x => x)
                    .ApplyFilters(filters, m_filterImplementationProvider)));
        }

        private static IEnumerable<FinlandiaHiihtoSingleResult> ParseRawResult(IEnumerable<FinlandiaHiihtoAPISearchResultRow> rawData)
        {
            var res = new List<FinlandiaHiihtoSingleResult>();
            foreach (var result in rawData)
            {
                var styleDistString = result.StyleAndDistance;
                var style = styleDistString[0] == 'P' ? FinlandiaSkiingStyle.Classic : FinlandiaSkiingStyle.Skate;
                var dist = (FinlandiaSkiingDistance)int.Parse(styleDistString.Substring(1));
                var gender = result.Gender switch
                {
                    "M" => Gender.Man,
                    "N" => Gender.Woman,
                    _ => Gender.Unknown
                };
                var competitionClass = FinlandiaHiihtoCompetitionClass.Create(dist, style);

                res.Add(new FinlandiaHiihtoSingleResult
                {
                    CompetitionInfo = new Competition
                    {
                        Year = result.Year,
                        Name = (int)competitionClass.Distance + "km " + 
                               competitionClass.Style.GetDescription()
                    },
                    CompetitionClass = FinlandiaHiihtoCompetitionClass.Create(dist, style),
                    Result = result.Result,
                    PositionGeneral = result.Position,
                    PositionMen = result.PositionMen,
                    PositionWomen = result.PositionWomen,
                    Athlete = new Person
                    {
                        FirstName = string.Join(" ", result.FullName.Split().Skip(1).ToArray()),
                        LastName = result.FullName.Split()[0],
                        PersonGender = gender,
                        City = result.HomeTown,
                        Nationality = result.Nationality,
                        YearOfBirth = result.BornYear
                    },

                    Team = result.Team
                });
            }

            return res;
        }
    }
}
