using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FJ.FinlandiaHiihtoAPI.Utils;

namespace FJ.FinlandiaHiihtoAPI
{
    public class FinlandiaHiihtoAPI : IFinlandiaHiihtoAPI
    {
        private readonly IFinlandiaHiihtoScraper m_scraper = new FinlandiaHiihtoScraper();
        
        private Dictionary<string, string> m_requestBaseForm;
        private async Task<Dictionary<string, string>> RequestBaseForm()
        {
            if (m_requestBaseForm != null)
            {
                return m_requestBaseForm;
            }

            m_requestBaseForm = await m_scraper.GetRequestBaseData();
            return m_requestBaseForm;
        }
        
        public async Task<IEnumerable<FinlandiaHiihtoAPISearchResultRow>> GetData(FinlandiaHiihtoAPISearchArgs args)
        {
            var searchConstraints = new List<string>
            {
                args.Year.ToString().EmptyToNull(),
                args.FirstName,
                args.LastName,
                args.CompetitionType,
                args.AgeGroup,
                args.CompetitorHomeTown,
                args.Team,
                args.Gender,
                args.Nationality
            };
            
            return await m_scraper.FetchData(await RequestBaseForm(), searchConstraints);
        }
    }
}
