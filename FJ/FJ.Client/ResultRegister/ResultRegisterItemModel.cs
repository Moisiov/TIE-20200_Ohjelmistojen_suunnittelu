using System;
using FJ.Client.Athlete;
using FJ.Client.Core.Register;
using FJ.Client.Core.UIElements.Filters;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.Utils.FinlandiaUtils;

namespace FJ.Client.ResultRegister
{
    public class ResultRegisterItemModel : RegisterItemModelBase<AthleteCardArgs>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Position { get; set; }
        public FinlandiaHiihtoCompetitionClass CompetitionClass { get; set; }
        public int Year { get; set; }
        
        public TimeSpan Result { get; set; }
        public string ResultTime { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string StyleAndDistanceString => FinlandiaHelpers.GetDistanceAndStyleShortString(
            CompetitionClass.Distance, CompetitionClass.Style);

        public override AthleteCardArgs GetNavigationArgs()
        {
            return new AthleteCardArgs
            {
                AthleteFirstName = FirstName ?? "Rocky",
                AthleteLastName = LastName ?? "Balboa"
            };
        }

        public static explicit operator ResultRegisterItemModel(FinlandiaHiihtoSingleResult res)
        {
            return new ResultRegisterItemModel
            {
                FirstName = res.Athlete.FirstName,
                LastName = res.Athlete.LastName,
                Position = res.PositionGeneral,
                CompetitionClass = res.CompetitionClass,
                ResultTime = res.ResultString,
                Year = res.CompetitionInfo.Year,
                Result = res.Result,
            };
        }
    }
}
