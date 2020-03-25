using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FJ.FinlandiaHiihtoAPI
{
    internal interface IFinlandiaHiihtoScraper
    {
        Task<Dictionary<string, string>> GetRequestBaseData();

        Task<IEnumerable<FinlandiaHiihtoAPISearchResultRow>> FetchData(Dictionary<string, string> form, IEnumerable<string> constraints);
    }
}
