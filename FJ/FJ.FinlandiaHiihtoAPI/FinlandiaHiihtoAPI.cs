using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FJ.FinlandiaHiihtoAPI
{
    public class FinlandiaHiihtoAPI : IFinlandiaHiihtoAPI
    {
        public async Task<IEnumerable<Dictionary<string, string>>> GetData(
            int? year = null,
            string firstName = null,
            string lastName = null,
            string competitionType = null,
            string ageGroup = null,
            string competitorHomeTown = null,
            string team = null,
            string gender = null,
            string nationality = null)
        {
            IFinlandiaHiihtoScraper scraper = new FinlandiaHiihtoScraper();
            Dictionary<string, string> requestForm = await scraper.GetRequestBaseData();

            List<string> searchConstraints = new List<string>()
            {
                year.ToString(),
                firstName,
                lastName,
                competitionType,
                ageGroup,
                competitorHomeTown,
                team,
                gender,
                nationality
            };

            return await scraper.FetchData(requestForm, searchConstraints);
        }
    }
}
