using System;
using System.Collections.Generic;
using FJ.DomainObjects.FinlandiaHiihto.Enums;

namespace FJ.DomainObjects.FinlandiaHiihto
{
    public class FinlandiaHiihtoCompetitionClass
    {
        public FinlandiaSkiingDistance Distance { get; set; }
        public FinlandiaSkiingStyle Style { get; set; }
        public string AdditionalDescription { get; set; }

        public static FinlandiaHiihtoCompetitionClass Create(FinlandiaSkiingDistance dist, FinlandiaSkiingStyle style, string description = null)
        {
            return new FinlandiaHiihtoCompetitionClass
            {
                Distance = dist,
                Style = style,
                AdditionalDescription = description
            };
        }
    }

    public static class FinlandiaHiihtoCompetitionClasses
    {
        public static IEnumerable<FinlandiaHiihtoCompetitionClass> FinlandiaCompetitionClasses { get; }

        static FinlandiaHiihtoCompetitionClasses()
        {
            FinlandiaCompetitionClasses = new[]
            {
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)50, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)50, FinlandiaSkiingStyle.Skate),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)100, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)32, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)20, FinlandiaSkiingStyle.Skate),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)32, FinlandiaSkiingStyle.Skate),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)20, FinlandiaSkiingStyle.Skate, ", juniorit alle 16v"),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)42, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)42, FinlandiaSkiingStyle.Skate),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)32, FinlandiaSkiingStyle.Skate, " (2014)"),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)20, FinlandiaSkiingStyle.Classic, " (2014)"),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)30, FinlandiaSkiingStyle.Classic, " (2002–2005)"),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)44, FinlandiaSkiingStyle.Classic, " (2002)"),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)60, FinlandiaSkiingStyle.Classic, " (2003–2005)"),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)62, FinlandiaSkiingStyle.Classic, " (2006)"),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)25, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)32, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)35, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)45, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)52, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)53, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)75, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)30, FinlandiaSkiingStyle.Skate),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)45, FinlandiaSkiingStyle.Skate),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)53, FinlandiaSkiingStyle.Skate),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)75, FinlandiaSkiingStyle.Skate),
            };
        }
    }
}
