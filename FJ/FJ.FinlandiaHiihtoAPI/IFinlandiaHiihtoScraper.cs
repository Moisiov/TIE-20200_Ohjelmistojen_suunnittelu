using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FJ.FinlandiaHiihtoAPI
{
    internal interface IFinlandiaHiihtoScraper
    {
        Task<Dictionary<string, string>> GetRequestBaseData();

        Task<IEnumerable<Dictionary<string, string>>> FetchData(
            Dictionary<string, string> form,
            IEnumerable<string> constraints);
    }
}
