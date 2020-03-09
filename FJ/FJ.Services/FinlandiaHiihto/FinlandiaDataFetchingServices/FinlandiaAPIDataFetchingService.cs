using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.FinlandiaHiihtoAPI;

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
            var raw = await m_api.GetData(year: args.CompetitionYears.First());
            return new FinlandiaHiihtoResultsCollection(args, ParseRawResult(raw));
        }

        protected IEnumerable<FinlandiaHiihtoSingleResult> ParseRawResult(IEnumerable<Dictionary<string, string>> rawData)
        {
            // TODO apilta jotain järkevämpää ulos
            var res = new List<FinlandiaHiihtoSingleResult>();
            foreach (var d in rawData)
            {
                var styleDistString = d["Matka"];
                var style = styleDistString[0] == 'P' ? FinlandiaSkiingStyle.Classic : FinlandiaSkiingStyle.Skate;
                var dist = (FinlandiaSkiingDistance)int.Parse(styleDistString.Substring(1));
                var gender = d["Sukupuoli"] == "M"
                    ? FinlandiaSkiingGender.Man
                    : d["Sukupuoli"] == "N"
                        ? FinlandiaSkiingGender.Woman
                        : FinlandiaSkiingGender.Unknown;

                res.Add(new FinlandiaHiihtoSingleResult
                {
                    Year = int.Parse(d["Vuosi"]),
                    Style = style,
                    Distance = dist,
                    Result = TimeSpan.Parse(d["Tulos"]),
                    PositionGeneral = int.Parse(d["Sija"]),
                    PositionMen = !string.IsNullOrWhiteSpace(d["Sija/Miehet"]) ? (int?)int.Parse(d["Sija/Miehet"]) : null,
                    PositionWomen = !string.IsNullOrWhiteSpace(d["Sija/Naiset"]) ? (int?)int.Parse(d["Sija/Naiset"]) : null,
                    Gender = gender,
                    LastName = d["Sukunimi Etunimi"].Split()[0],
                    FirstName = string.Join(" ", d["Sukunimi Etunimi"].Split().Skip(1).ToArray()),
                    City = d["Paikkakunta"],
                    Nationality = d["Kansallisuus"],
                    YearOfBirth = !string.IsNullOrWhiteSpace(d["Syntymävuosi"]) ? (int?)int.Parse(d["Syntymävuosi"]) : null,
                    Team = d["Joukkue"]
                });
            }

            return res;
        }
    }
}
