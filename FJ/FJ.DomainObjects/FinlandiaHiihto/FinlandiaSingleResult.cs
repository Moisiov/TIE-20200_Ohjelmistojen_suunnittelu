using System;
using FJ.DomainObjects.FinlandiaHiihto.Enums;

namespace FJ.DomainObjects.FinlandiaHiihto
{
    public class FinlandiaSingleResult
    {
        public int Year { get; set; }
        public FinlandiaSkiingStyle Style { get; set; }
        public FinlandiaSkiingDistance Distance { get; set; }
        public TimeSpan Result { get; set; }
        public int PositionGeneral { get; set; }
        public int? PositionMen { get; set; }
        public int? PositionWomen { get; set; }
        public FinlandiaSkiingGender Gender { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string City { get; set; }
        public string Nationality { get; set; }
        public int? YearOfBirth { get; set; }
        public string Team { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public string StyleAndDistanceString => $"{(Style == FinlandiaSkiingStyle.Classic ? "P" : "V")}{(int)Distance}";  // MUAHAHAHAA TODO
        // public int? AgeOnCompetitionYear => ... TODO
    }
}
