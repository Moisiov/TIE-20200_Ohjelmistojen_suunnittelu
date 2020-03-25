using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.Utils.FinlandiaUtils;

namespace FJ.NUnitTests.TestData
{
    public class FinlandiaHiihtoSingleResultDummyDataProvider
    {
        public static FinlandiaHiihtoSingleResult Create(int? seed = null)
        {
            var rand = seed.HasValue ? new Random(seed.Value) : new Random();
            var compClasses = FinlandiaHiihtoCompetitionClasses.FinlandiaCompetitionClasses.ToArray();
            
            return new FinlandiaHiihtoSingleResult
            {
                Result = DateTime.Now.TimeOfDay,
                PositionGeneral = rand.Next(1000),
                CompetitionClass = compClasses.ElementAt(rand.Next(compClasses.Length - 1)),
                CompetitionInfo = new Competition
                {
                    Year = rand.Next(FinlandiaConstants.C_FirstFinlandiaSkiingYear, DateTime.Today.Year)
                },
                Athlete = PersonDummyDataProvider.Create(seed),
                Team = TestUtils.GenerateRandomString(12)
            };
        }
    }
}
