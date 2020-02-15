using System;
using System.Collections.Generic;
using System.Linq;

namespace FJ.DomainObjects.FinlandiaHiihto
{
    public class FinlandiaHiihtoResultsCollection
    {
        public FinlandiaHiihtoSearchArgs AppliedArgs { get; set; }
        public IEnumerable<FinlandiaHiihtoSingleResult> Results { get; set; }

        public bool HasAnyResults => Results?.Any() == true;

        public FinlandiaHiihtoResultsCollection(FinlandiaHiihtoSearchArgs args, IEnumerable<FinlandiaHiihtoSingleResult> results)
        {
            AppliedArgs = args;
            Results = results ?? new FinlandiaHiihtoSingleResult[] { };
        }
    }
}
