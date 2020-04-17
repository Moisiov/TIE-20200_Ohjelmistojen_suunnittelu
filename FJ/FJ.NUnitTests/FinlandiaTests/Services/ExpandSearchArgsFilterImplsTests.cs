using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FinlandiaHiihtoAPI;
using FinlandiaHiihtoAPI.Enums;
using FJ.DomainObjects.Filters;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.DomainObjects.FinlandiaHiihto.Filters;
using FJ.Services.CoreServices;
using FJ.Utils;
using NUnit.Framework;

namespace FJ.NUnitTests.FinlandiaTests.Services
{
    [TestFixture]
    public class SearchArgsExpFilterImplsTests : TestFixtureBase
    {
        private IFilterImplementationProvider m_filterImplementationProvider;

        [SetUp]
        public void Setup()
        {
            m_filterImplementationProvider = new FilterImplementationProvider();
        }
        
        [Test]
        public void AgeGroupsFilterTest()
        {
            var ageGroupFilter = new FinlandiaAgeGroupsFilter(FinlandiaSkiingAgeGroup.Fifty.ToMany());
            var filterCollection = new FilterCollection(ageGroupFilter);
            var apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(1, apiArgs.Count);
            Assert.AreEqual(FinlandiaAgeGroup.Fifty, apiArgs.First().AgeGroup);
            Assert.IsTrue(OthersAreNull(apiArgs.First(), new[] { "AgeGroup" }));
            
            var ageGroups = new[]
            {
                FinlandiaSkiingAgeGroup.Forty, 
                FinlandiaSkiingAgeGroup.Fifty, 
                FinlandiaSkiingAgeGroup.Sixty,
                FinlandiaSkiingAgeGroup.Seventy,
            };
            ageGroupFilter = new FinlandiaAgeGroupsFilter(ageGroups);
            filterCollection = new FilterCollection(ageGroupFilter);
            apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(4, apiArgs.Count);

            var i = 0;
            foreach (var arg in apiArgs)
            {
                Assert.AreEqual((FinlandiaAgeGroup)ageGroups[i], arg.AgeGroup);
                Assert.IsTrue(OthersAreNull(arg, new[] { "AgeGroup" }));
                i++;
            }
        }
        
        [Test]
        public void CompetitionClassesFilterTest()
        {
            var competitionClass =
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)50, FinlandiaSkiingStyle.Classic);
            var competitionClassFilter = new FinlandiaCompetitionClassesFilter(competitionClass.ToMany());
            var filterCollection = new FilterCollection(competitionClassFilter);
            var apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(1, apiArgs.Count);
            Assert.AreEqual(FinlandiaCompetitionType.P50, apiArgs.First().CompetitionType);
            Assert.IsTrue(OthersAreNull(apiArgs.First(), new[] { "CompetitionType" }));
            
            var competitionTypesInput = new[]
            {
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)50, FinlandiaSkiingStyle.Classic),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)20, FinlandiaSkiingStyle.Skate),
                FinlandiaHiihtoCompetitionClass.Create(
                    (FinlandiaSkiingDistance)20, 
                    FinlandiaSkiingStyle.Skate,
                    ", juniorit alle 16v"),
                FinlandiaHiihtoCompetitionClass.Create((FinlandiaSkiingDistance)25, FinlandiaSkiingStyle.Classic),
            };
            var competitionTypesOutput = new[]
            {
                FinlandiaCompetitionType.P50,
                FinlandiaCompetitionType.V20,
                FinlandiaCompetitionType.V20jun,
                FinlandiaCompetitionType.P25,
            };
            competitionClassFilter = new FinlandiaCompetitionClassesFilter(competitionTypesInput);
            filterCollection = new FilterCollection(competitionClassFilter);
            apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(4, apiArgs.Count);

            var i = 0;
            foreach (var arg in apiArgs)
            {
                Assert.AreEqual(competitionTypesOutput[i], arg.CompetitionType);
                Assert.IsTrue(OthersAreNull(arg, new[] { "CompetitionType" }));
                i++;
            }
        }
        
        [Test]
        public void FirstNameFilterTest()
        {
            var firstNameFilter = new FinlandiaFirstNamesFilter("Jaakko".ToMany());
            var filterCollection = new FilterCollection(firstNameFilter);
            var apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(1, apiArgs.Count);
            Assert.AreEqual("Jaakko", apiArgs.First().FirstName);
            Assert.IsTrue(OthersAreNull(apiArgs.First(), new[] { "FirstName" }));
            
            var names = new[] { "Jaakko", "Jussi", "Joona", "Jani" };
            firstNameFilter = new FinlandiaFirstNamesFilter(names);
            filterCollection = new FilterCollection(firstNameFilter);
            apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(4, apiArgs.Count);

            var i = 0;
            foreach (var arg in apiArgs)
            {
                Assert.AreEqual(names[i], arg.FirstName);
                Assert.IsTrue(OthersAreNull(arg, new[] { "FirstName" }));
                i++;
            }
        }

        [Test]
        public void HomeCitiesFilterTest()
        {
            var cityFilter = new FinlandiaHomeCitiesFilter("Vantaa".ToMany());
            var filterCollection = new FilterCollection(cityFilter);
            var apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(1, apiArgs.Count);
            Assert.AreEqual("Vantaa", apiArgs.First().CompetitorHomeTown);
            Assert.IsTrue(OthersAreNull(apiArgs.First(), new[] { "CompetitorHomeTown" }));
            
            var cities = new[] { "Vantaa", "Tampere", "Espoo", "Oulu" };
            cityFilter = new FinlandiaHomeCitiesFilter(cities);
            filterCollection = new FilterCollection(cityFilter);
            apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(4, apiArgs.Count);

            var i = 0;
            foreach (var arg in apiArgs)
            {
                Assert.AreEqual(cities[i], arg.CompetitorHomeTown);
                Assert.IsTrue(OthersAreNull(arg, new[] { "CompetitorHomeTown" }));
                i++;
            }
        }
        
        [Test]
        public void LastNameFilterTest()
        {
            var lastNameFilter = new FinlandiaLastNamesFilter("Korhonen".ToMany());
            var filterCollection = new FilterCollection(lastNameFilter);
            var apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(1, apiArgs.Count);
            Assert.AreEqual("Korhonen", apiArgs.First().LastName);
            Assert.IsTrue(OthersAreNull(apiArgs.First(), new[] { "LastName" }));
            
            var names = new[] { "Korhonen", "Virtanen", "Mäkinen", "Nieminen" };
            lastNameFilter = new FinlandiaLastNamesFilter(names);
            filterCollection = new FilterCollection(lastNameFilter);
            apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(4, apiArgs.Count);

            var i = 0;
            foreach (var arg in apiArgs)
            {
                Assert.AreEqual(names[i], arg.LastName);
                Assert.IsTrue(OthersAreNull(arg, new[] { "LastName" }));
                i++;
            }
        }
        
        [Test]
        public void TeamsFilterTest()
        {
            var teamFilter = new FinlandiaTeamsFilter("T1".ToMany());
            var filterCollection = new FilterCollection(teamFilter);
            var apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(1, apiArgs.Count);
            Assert.AreEqual("T1", apiArgs.First().Team);
            Assert.IsTrue(OthersAreNull(apiArgs.First(), new[] { "Team" }));
            
            var teams = new[] { "T1", "T2", "T3", "T4" };
            teamFilter = new FinlandiaTeamsFilter(teams);
            filterCollection = new FilterCollection(teamFilter);
            apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(4, apiArgs.Count);

            var i = 0;
            foreach (var arg in apiArgs)
            {
                Assert.AreEqual(teams[i], arg.Team);
                Assert.IsTrue(OthersAreNull(arg, new[] { "Team" }));
                i++;
            }
        }
        
        [Test]
        public void YearsFilterTest()
        {
            var yearFilter = new FinlandiaCompetitionYearsFilter(2019.ToMany());
            var filterCollection = new FilterCollection(yearFilter);
            var apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(1, apiArgs.Count);
            Assert.AreEqual(2019, apiArgs.First().Year);
            Assert.IsTrue(OthersAreNull(apiArgs.First(), new[] { "Year" }));
            
            yearFilter = new FinlandiaCompetitionYearsFilter(new[] { 2019, 2018, 2017, 2016 });
            filterCollection = new FilterCollection(yearFilter);
            apiArgs = FiltersToApiSearchArgs(filterCollection);
            
            Assert.AreEqual(4, apiArgs.Count);

            var expectedYear = 2019;
            foreach (var arg in apiArgs)
            {
                Assert.AreEqual(expectedYear, arg.Year);
                Assert.IsTrue(OthersAreNull(arg, new[] { "Year" }));
                expectedYear--;
            }
        }

        private List<FinlandiaHiihtoAPISearchArgs> FiltersToApiSearchArgs(FilterCollection filters)
        {
            return new FilterSearchComposer<FinlandiaHiihtoAPISearchArgs>()
                .ApplyFilters(filters, m_filterImplementationProvider)
                .Searches;
        }

        private static bool OthersAreNull(FinlandiaHiihtoAPISearchArgs args, string[] notNullArray)
        {
            return !args.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Any(x => x.CanRead 
                          && !x.GetIndexParameters().Any() 
                          && notNullArray.All(s => s != x.Name) 
                          && x.GetValue(args, null) != null);
        }
    }
}
