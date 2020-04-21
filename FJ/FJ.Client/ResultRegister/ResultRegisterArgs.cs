using System;
using System.Collections.Generic;
using System.Linq;
using FJ.Client.Core;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.Client.ResultRegister
{
    // TODO Not SOLID but implementation via reflection would need more testing than what we got time for
    public class ResultRegisterArgs : NavigationArgsBase<ResultRegisterArgs>
    {
        public HashSet<int> CompetitionYears { get; set; }
        public HashSet<FinlandiaHiihtoCompetitionClass> CompetitionClasses { get; set; }
        public HashSet<string> FirstNames { get; set; }
        public HashSet<string> LastNames { get; set; }

        public bool Empty => !CompetitionYears.Any() && !CompetitionClasses.Any() && !FirstNames.Any() && !LastNames.Any();

        public ResultRegisterArgs()
        {
            CompetitionYears = new HashSet<int>();
            CompetitionClasses = new HashSet<FinlandiaHiihtoCompetitionClass>();
            FirstNames = new HashSet<string>();
            LastNames = new HashSet<string>();
        }

        public static ResultRegisterArgs CreateFromSingleResults(IEnumerable<FinlandiaHiihtoSingleResult> results)
        {
            var args = new ResultRegisterArgs();
            
            var finlandiaHiihtoSingleResults = results as FinlandiaHiihtoSingleResult[] ?? results.ToArray();
            if (finlandiaHiihtoSingleResults.Any() != true)
            {
                return args;
            }
            
            foreach (var result in finlandiaHiihtoSingleResults)
            {
                args.CompetitionYears.Add(result.CompetitionInfo.Year);
                args.CompetitionClasses.Add(result.CompetitionClass);
                args.FirstNames.Add(result.Athlete.FirstName);
                args.LastNames.Add(result.Athlete.LastName);
            }

            return args;
        }
    }
}
