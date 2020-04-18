using System;
using FJ.Client.Core;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.Client.CompetitionGeneral
{
    public class CompetitionGeneralArgs : NavigationArgsBase<CompetitionGeneralArgs>
    {
        public int? CompetitionYear { get; set; }
        public FinlandiaHiihtoCompetitionClass CompetitionClass { get; set; }
    }
}
