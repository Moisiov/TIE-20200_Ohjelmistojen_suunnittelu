using System;
using System.Collections.Generic;
using FinlandiaHiihtoAPI.Utils;

namespace FinlandiaHiihtoAPI
{
    public class FinlandiaHiihtoAPISearchArgs
    {
        public int? Year { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompetitionType { get; set; }
        public string AgeGroup { get; set; }
        public string CompetitorHomeTown { get; set; }
        public string Team { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
    }
    
    public class FinlandiaHiihtoAPISearchResultRow
    {
        public int Year { get; }
        public string StyleAndDistance { get; }
        public TimeSpan Result { get; }
        public int Position { get; }
        public int? PositionMen { get; }
        public int? PositionWomen { get; }
        public string Gender { get; }
        public string FullName { get; }
        public string HomeTown { get; }
        public string Nationality { get; }
        public int? BornYear { get; }
        public string Team { get; }

        public FinlandiaHiihtoAPISearchResultRow(IReadOnlyList<string> resultRow)
        {
            Year = int.Parse(resultRow[0]);
            StyleAndDistance = resultRow[1];
            Result = TimeSpan.Parse(resultRow[2]);
            Position = int.Parse(resultRow[3]);
            PositionMen = resultRow[4].ToNullableInt();
            PositionWomen = resultRow[5].ToNullableInt();
            Gender = resultRow[6].EmptyToNull();
            FullName = resultRow[7];
            HomeTown = resultRow[8].EmptyToNull();
            Nationality = resultRow[9].EmptyToNull();
            BornYear = resultRow[10].ToNullableInt();
            Team = resultRow[11].EmptyToNull();
        }

        public override string ToString()
        {
            return $"{Year} - {StyleAndDistance} - {FullName} - {Result}";
        }
    }
}
