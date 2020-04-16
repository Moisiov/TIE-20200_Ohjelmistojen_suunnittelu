using System;
using System.Collections.Generic;
using FinlandiaHiihtoAPI.Enums;
using FinlandiaHiihtoAPI.Utils;

namespace FinlandiaHiihtoAPI
{
    public class FinlandiaHiihtoAPISearchArgs
    {
        public int? Year { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public FinlandiaCompetitionType? CompetitionType { get; set; }
        public FinlandiaAgeGroup? AgeGroup { get; set; }
        public string CompetitorHomeTown { get; set; }
        public string Team { get; set; }
        public FinlandiaGender? Gender { get; set; }
        public FinlandiaNationality? Nationality { get; set; }
    }
    
    public class FinlandiaHiihtoAPISearchResultRow
    {
        public int Year { get; }
        public string StyleAndDistance { get; }
        public TimeSpan Result { get; }
        public int Position { get; }
        public int? PositionMen { get; }
        public int? PositionWomen { get; }
        public FinlandiaGender? Gender { get; }
        public string FullName { get; }
        public string HomeTown { get; }
        public FinlandiaNationality? Nationality { get; }
        public int? BornYear { get; }
        public string Team { get; }

        public FinlandiaHiihtoAPISearchResultRow(IReadOnlyList<string> resultRow)
        {
            Year = int.Parse(resultRow[0]);
            StyleAndDistance = resultRow[1].EmptyToNull();
            Result = TimeSpan.Parse(resultRow[2]);
            Position = int.Parse(resultRow[3]);
            PositionMen = resultRow[4].ToNullableInt();
            PositionWomen = resultRow[5].ToNullableInt();
            Gender = resultRow[6].EmptyToNull() switch
            {
                "M" => FinlandiaGender.Male,
                "N" => FinlandiaGender.Female,
                _ => null

            };
            FullName = resultRow[7];
            HomeTown = resultRow[8].EmptyToNull();
            Nationality = 
                Enum.TryParse(resultRow[9].EmptyToNull(), true, out FinlandiaNationality parsedNat)
                ? parsedNat : (FinlandiaNationality?)null;
            BornYear = resultRow[10].ToNullableInt();
            Team = resultRow[11].EmptyToNull();
        }

        public override string ToString()
        {
            return $"{Year} - {StyleAndDistance} - {FullName} - {Result}";
        }

        public bool Equals(FinlandiaHiihtoAPISearchResultRow other)
        {
            return Year == other.Year
                   && StyleAndDistance == other.StyleAndDistance
                   && Result.Equals(other.Result)
                   && Position == other.Position
                   && FullName == other.FullName
                   && BornYear == other.BornYear;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Year, StyleAndDistance, Result, Position, FullName, BornYear);
        }
    }
}
