using System;
using System.Collections.Generic;
using System.Linq;

namespace FJ.DomainObjects.FinlandiaHiihto
{
    public class FinlandiaHiihtoResultsCollection
    {
        public int SearchesExecuted { get; set; }
        public IEnumerable<FinlandiaHiihtoSingleResult> Results { get; set; }
        public int ResultsCount => Results?.Count() ?? 0;

        public bool HasAnyResults => Results?.Any() == true;

        public FinlandiaHiihtoResultsCollection()
            : this(null)
        {
        }
        
        public FinlandiaHiihtoResultsCollection(IEnumerable<FinlandiaHiihtoSingleResult> results)
        {
            Results = results ?? new FinlandiaHiihtoSingleResult[] { };
        }
    }
}
