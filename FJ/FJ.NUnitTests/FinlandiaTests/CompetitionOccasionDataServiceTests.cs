using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.DomainObjects;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.NUnitTests.TestData;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Services.CoreServices;
using FJ.Services.FinlandiaHiihto;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;
using FJ.Utils;
using Moq;
using NUnit.Framework;
using FinlandiaHiihtoAPI = FinlandiaHiihtoAPI.FinlandiaHiihtoAPI;

namespace FJ.NUnitTests.FinlandiaTests
{
    [TestFixture]
    public class CompetitionOccasionDataServiceTests : TestFixtureBase
    {
        private ICompetitionOccasionDataService m_competitionOccasionDataService;

        private class MockDataFetchingService : IDataFetchingService
        {
            private FinlandiaHiihtoResultsCollection res = 
                CompetitionOccasionDummyDataProvider.PopulateResultRows(2000);
            public Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(
                FinlandiaHiihtoSearchArgs args)
            {
                return Task.FromResult(res);
            }
            public Task<FinlandiaHiihtoResultsCollection> GetFinlandiaHiihtoResultsAsync(FilterCollection filters)
            {
                return Task.FromResult(res);
            }
        }
    
        [SetUp]
        public void Setup()
        {
            m_competitionOccasionDataService = new CompetitionOccasionDataService(new MockDataFetchingService());
        }

        [Test]
        public async Task TestAllCompetitionOccasionResultsFetching()
        {
            var res = 
                await m_competitionOccasionDataService.GetCompetitionOccasionResultsAsync(2000);
            
            Assert.AreEqual(172, res.Results.Count());
        }
        
        [Test]
        public async Task TestOrderedCompetitionListsFetching()
        {
            var res = 
                await m_competitionOccasionDataService.GetOrderedCompetitionListsAsync(2000);
            
            Assert.AreEqual(4, res.Count());

            foreach (var competition in res)
            {
                Assert.AreEqual(43, competition.Results.Count());
                Assert.AreEqual(1, competition.Results.First().PositionGeneral);
                Assert.AreEqual(43, competition.Results.Last().PositionGeneral);
            }
        }
        
        [Test]
        public async Task TestOrderedCompetitionTeamListFetching()
        {
            var res = 
                await m_competitionOccasionDataService.GetOrderedCompetitionTeamListAsync(2000);
            
            Assert.AreEqual(4, res.Count());

            var thirtyWinningTime = new TimeSpan(9, 0, 38);
            var thirtyLosingTime = new TimeSpan(9, 2, 14);
            var fiftyWinningTime = new TimeSpan(14, 00, 38);
            var fiftyLosingTime = new TimeSpan(14, 02, 14);
            
            foreach (var competition in res)
            {
                Assert.AreEqual(10, competition.Results.Count());
                
                Assert.AreEqual("Team2 1", competition.Results.First().Team);
                Assert.AreEqual("Team1 2", competition.Results.Last().Team);

                switch (competition.Results.First().CompetitionClass.Distance)
                {
                    case FinlandiaSkiingDistance.Thirty:
                        Assert.AreEqual(thirtyWinningTime, competition.Results.First().Result);
                        Assert.AreEqual(thirtyLosingTime, competition.Results.Last().Result);
                        break;
                    case FinlandiaSkiingDistance.Fifty:
                        Assert.AreEqual(fiftyWinningTime, competition.Results.First().Result);
                        Assert.AreEqual(fiftyLosingTime, competition.Results.Last().Result);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
