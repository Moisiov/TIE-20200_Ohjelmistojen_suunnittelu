using System;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.Utils.FinlandiaUtils;

namespace FJ.Client.ResultRegister
{
    public class ResultRegisterItemModel
    {
        public bool IsSelected { get; set; }

        public string Name { get; set; }
        public int Position { get; set; }
        public string StyleAndDistance { get; set; }
        public int Year { get; set; }
        public string ResultTime { get; set; }

        // TODO Not binded yet
        public string Gender { get; set; }
        public string City { get; set; }
        public string Nationality { get; set; }
        public string YearOfBirthString { get; set; }
        public string Team { get; set; }
        public string PositionMenString { get; set; }
        public string PositionWomenString { get; set; }

        // For passing accurate name info from ResultRegister to AthleteCard
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static explicit operator ResultRegisterItemModel(FinlandiaHiihtoSingleResult res)
        {
            return new ResultRegisterItemModel
            {
                Name = res.Athlete.FullName,
                Position = res.PositionGeneral,
                StyleAndDistance = FinlandiaHelpers.GetDistanceAndStyleShortString(
                    res.CompetitionClass.Distance, res.CompetitionClass.Style),
                ResultTime = res.Result.ToString(@"hh\:mm\:ss\.ff"),
                FirstName = res.Athlete.FirstName,
                LastName = res.Athlete.LastName,
                Year = res.CompetitionInfo.Year
            };
        }
    }
}
