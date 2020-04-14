using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.Utils;

namespace FJ.NUnitTests.TestData
{
    public static class CompetitionOccasionDummyDataProvider
    {
        public static FinlandiaHiihtoResultsCollection PopulateResultRows(int year)
        {
            List<FinlandiaHiihtoSingleResult> results = new List<FinlandiaHiihtoSingleResult>();
             
            // Competition #1
            results.Add(new FinlandiaHiihtoSingleResult
            {
                 Result = new TimeSpan(2, 30, 00),
                 PositionGeneral = 1,
                 CompetitionClass = new FinlandiaHiihtoCompetitionClass
                 {
                     Distance = FinlandiaSkiingDistance.Fifty,
                     Style = FinlandiaSkiingStyle.Skate
                 },
                 CompetitionInfo = new Competition
                 {
                     Name = (int)FinlandiaSkiingDistance.Fifty + "km " +
                            FinlandiaSkiingStyle.Skate.GetDescription(),
                     Year = year
                 },
                 Athlete = new Person { FirstName = "Hessu", LastName = "Hopo" },
             });

            for (var i = 2; i < 43; i++)
            {
                results.Add(new FinlandiaHiihtoSingleResult
                {
                    Result = new TimeSpan(3, 30, i),
                    PositionGeneral = i,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass
                    {
                        Distance = FinlandiaSkiingDistance.Fifty,
                        Style = FinlandiaSkiingStyle.Skate
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Fifty + "km " +
                               FinlandiaSkiingStyle.Skate.GetDescription(),
                        Year = year
                    },
                    Athlete = new Person { FirstName = "Aku", LastName = "Ankka" },
                    Team = "Team" + (i % 5),
                });
            }

            results.Add(new FinlandiaHiihtoSingleResult
            {
                Result = new TimeSpan(5, 00, 00),
                PositionGeneral = 43,
                CompetitionClass = new FinlandiaHiihtoCompetitionClass
                {
                    Distance = FinlandiaSkiingDistance.Fifty,
                    Style = FinlandiaSkiingStyle.Skate
                },
                CompetitionInfo = new Competition
                {
                    Name = (int)FinlandiaSkiingDistance.Fifty + "km " +
                           FinlandiaSkiingStyle.Skate.GetDescription(),
                    Year = year
                },
                Athlete = new Person { FirstName = "Mikki", LastName = "Hiiri" },
            });
             
            // Competition #2
            for (var i = 2; i < 43; i++)
            {
                results.Add(new FinlandiaHiihtoSingleResult
                {
                    Result = new TimeSpan(2, 15, i),
                    PositionGeneral = i,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass
                    {
                        Distance = FinlandiaSkiingDistance.Thirty,
                        Style = FinlandiaSkiingStyle.Skate
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Thirty + "km " +
                               FinlandiaSkiingStyle.Skate.GetDescription(),
                        Year = year
                    },
                    Athlete = new Person { FirstName = "Aku", LastName = "Ankka" },
                    Team = "Team" + (i % 5),
                });
            }
            
            results.Add(new FinlandiaHiihtoSingleResult
            {
                Result = new TimeSpan(3, 00, 00),
                PositionGeneral = 43,
                CompetitionClass = new FinlandiaHiihtoCompetitionClass
                {
                    Distance = FinlandiaSkiingDistance.Thirty,
                    Style = FinlandiaSkiingStyle.Skate
                },
                CompetitionInfo = new Competition
                {
                    Name = (int)FinlandiaSkiingDistance.Thirty + "km " +
                           FinlandiaSkiingStyle.Skate.GetDescription(),
                    Year = year
                },
                Athlete = new Person { FirstName = "Mikki", LastName = "Hiiri" },
            });
            
            results.Add(new FinlandiaHiihtoSingleResult
            {
                Result = new TimeSpan(1, 30, 00),
                PositionGeneral = 1,
                CompetitionClass = new FinlandiaHiihtoCompetitionClass
                {
                    Distance = FinlandiaSkiingDistance.Thirty,
                    Style = FinlandiaSkiingStyle.Skate
                },
                CompetitionInfo = new Competition
                {
                    Name = (int)FinlandiaSkiingDistance.Thirty + "km " +
                           FinlandiaSkiingStyle.Skate.GetDescription(),
                    Year = year
                },
                Athlete = new Person { FirstName = "Hessu", LastName = "Hopo" },
            });
            
            // Competition #3
            for (var i = 2; i < 43; i++)
            {
                results.Add(new FinlandiaHiihtoSingleResult
                {
                    Result = new TimeSpan(3, 30, i),
                    PositionGeneral = i,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass
                    {
                        Distance = FinlandiaSkiingDistance.Fifty,
                        Style = FinlandiaSkiingStyle.Classic
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Fifty + "km " +
                               FinlandiaSkiingStyle.Classic.GetDescription(),
                        Year = year
                    },
                    Athlete = new Person { FirstName = "Aku", LastName = "Ankka" },
                    Team = "Team" + (i % 5),
                });
            }
            
            results.Add(new FinlandiaHiihtoSingleResult
            {
                Result = new TimeSpan(2, 30, 00),
                PositionGeneral = 1,
                CompetitionClass = new FinlandiaHiihtoCompetitionClass
                {
                    Distance = FinlandiaSkiingDistance.Fifty,
                    Style = FinlandiaSkiingStyle.Classic
                },
                CompetitionInfo = new Competition
                {
                    Name = (int)FinlandiaSkiingDistance.Fifty + "km " +
                           FinlandiaSkiingStyle.Classic.GetDescription(),
                    Year = year
                },
                Athlete = new Person { FirstName = "Hessu", LastName = "Hopo" },
            });
            
            results.Add(new FinlandiaHiihtoSingleResult
            {
                Result = new TimeSpan(5, 00, 00),
                PositionGeneral = 43,
                CompetitionClass = new FinlandiaHiihtoCompetitionClass
                {
                    Distance = FinlandiaSkiingDistance.Fifty,
                    Style = FinlandiaSkiingStyle.Classic
                },
                CompetitionInfo = new Competition
                {
                    Name = (int)FinlandiaSkiingDistance.Fifty + "km " +
                           FinlandiaSkiingStyle.Classic.GetDescription(),
                    Year = year
                },
                Athlete = new Person { FirstName = "Mikki", LastName = "Hiiri" },
            });
            
            // Competition #4
            results.Add(new FinlandiaHiihtoSingleResult
            {
                Result = new TimeSpan(3, 00, 00),
                PositionGeneral = 43,
                CompetitionClass = new FinlandiaHiihtoCompetitionClass
                {
                    Distance = FinlandiaSkiingDistance.Thirty,
                    Style = FinlandiaSkiingStyle.Classic
                },
                CompetitionInfo = new Competition
                {
                    Name = (int)FinlandiaSkiingDistance.Thirty + "km " +
                           FinlandiaSkiingStyle.Classic.GetDescription(),
                    Year = year
                },
                Athlete = new Person { FirstName = "Mikki", LastName = "Hiiri" },
            });
            
            results.Add(new FinlandiaHiihtoSingleResult
            {
                Result = new TimeSpan(1, 30, 00),
                PositionGeneral = 1,
                CompetitionClass = new FinlandiaHiihtoCompetitionClass
                {
                    Distance = FinlandiaSkiingDistance.Thirty,
                    Style = FinlandiaSkiingStyle.Classic
                },
                CompetitionInfo = new Competition
                {
                    Name = (int)FinlandiaSkiingDistance.Thirty + "km " +
                           FinlandiaSkiingStyle.Classic.GetDescription(),
                    Year = year
                },
                Athlete = new Person { FirstName = "Hessu", LastName = "Hopo" },
            });
            
            for (var i = 2; i < 43; i++)
            {
                results.Add(new FinlandiaHiihtoSingleResult
                {
                    Result = new TimeSpan(2, 15, i),
                    PositionGeneral = i,
                    CompetitionClass = new FinlandiaHiihtoCompetitionClass
                    {
                        Distance = FinlandiaSkiingDistance.Thirty,
                        Style = FinlandiaSkiingStyle.Classic
                    },
                    CompetitionInfo = new Competition
                    {
                        Name = (int)FinlandiaSkiingDistance.Thirty + "km " +
                               FinlandiaSkiingStyle.Classic.GetDescription(),
                        Year = year
                    },
                    Athlete = new Person { FirstName = "Aku", LastName = "Ankka" },
                    Team = "Team" + (i % 5),
                });
            }

            return new FinlandiaHiihtoResultsCollection(results);
        }
    }
}
