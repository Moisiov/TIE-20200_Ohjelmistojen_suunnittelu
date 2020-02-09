using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FJ.FinlandiaHiihtoAPI
{
    public interface IFinlandiaHiihtoAPI
    {
        Task<IEnumerable<Dictionary<string, string>>> GetData(
            int? year = null,
            string firstName = null,
            string lastName = null,
            string competitionType = null,
            string ageGroup = null,
            string competitorHomeTown = null,
            string team = null,
            string gender = null,
            string nationality = null);
    }
}
