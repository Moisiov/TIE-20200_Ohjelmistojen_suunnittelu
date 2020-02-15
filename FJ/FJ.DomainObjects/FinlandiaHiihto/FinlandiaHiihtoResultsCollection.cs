using System;
using System.Collections.Generic;

namespace FJ.DomainObjects.FinlandiaHiihto
{
    public class FinlandiaHiihtoResultsCollection
    {
        public FinlandiaHiihtoResultArgs AppliedArgs { get; set; }
        public IEnumerable<FinlandiaSingleResult> Results { get; set; }

        public FinlandiaHiihtoResultsCollection(FinlandiaHiihtoResultArgs args, IEnumerable<FinlandiaSingleResult> results)
        {
            AppliedArgs = args;
            Results = results;
        }
    }
}
