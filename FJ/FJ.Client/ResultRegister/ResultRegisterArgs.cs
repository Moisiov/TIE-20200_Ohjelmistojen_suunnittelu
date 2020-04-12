using System;
using System.Collections.Generic;
using FJ.Client.Core;
using FJ.DomainObjects;

namespace FJ.Client.ResultRegister
{
    public class ResultRegisterArgs : NavigationArgsBase<ResultRegisterArgs>
    {
        public IEnumerable<int> CompetitionYears { get; set; } = new List<int>();
        public IEnumerable<string> HomeCities { get; set; } = new List<string>();
    }
}
