using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinlandiaHiihtoAPI
{
    public interface IFinlandiaHiihtoAPI
    {
        Task<IEnumerable<FinlandiaHiihtoAPISearchResultRow>> GetData(FinlandiaHiihtoAPISearchArgs args);
    }
}
