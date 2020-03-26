using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinlandiaHiihtoAPI;
using FJ.DomainObjects;
using FJ.DomainObjects.Enums;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;

namespace FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices
{
    public class FinlandiaAPIDataFetchingService : IDataFetchingService
    {
        private readonly IFinlandiaHiihtoAPI m_api;

        public FinlandiaAPIDataFetchingService(IFinlandiaHiihtoAPI api)
        {
            m_api = api;
        }

        public async Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(FinlandiaHiihtoSearchArgs args)
        {
            // TODO proof of concept, ei huomioi argseja vielä kuin ensimmäisen vuoden osalta
            var raw = await m_api.GetData(new FinlandiaHiihtoAPISearchArgs
            {
                Year = args.CompetitionYears.First()
            });
            
            return new FinlandiaHiihtoResultsCollection(args, ParseRawResult(raw));
        }

        /*
        private static IEnumerable<FinlandiaHiihtoAPISearchArgs> ParseArguments(FinlandiaHiihtoSearchArgs args)
        {
            TODO
        }
        */

        private static IEnumerable<FinlandiaHiihtoSingleResult> ParseRawResult(IEnumerable<FinlandiaHiihtoAPISearchResultRow> rawData)
        {
            // TODO apilta jotain järkevämpää ulos
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

                res.Add(new FinlandiaHiihtoSingleResult
                {
                    CompetitionInfo = new Competition
                    {
                        Year = result.Year
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
                        YearOfBirth = result.BornYear,
                    },

                    Team = result.Team
                });
            }

            return res;
        }
    }
}
