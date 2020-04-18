using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Filters;
using FJ.NUnitTests.TestData;
using FJ.Services.CoreServices;
using FJ.Services.FinlandiaHiihto.FinlandiaDataFetchingServices;
using FJ.Utils;
using Moq;
using NUnit.Framework;

namespace FJ.NUnitTests.FinlandiaTests
{
    [TestFixture]
    public class SimpleDataFetcherCacheUnitTests : TestFixtureBase
    {
        private SimpleDataFetcherCacheDecorator m_testDecorator;
        private Mock<IDataFetchingService> m_actualFetcherMock;

        private readonly Expression<Func<IDataFetchingService, Task<FinlandiaHiihtoResultsCollection>>> m_mockedFunc
            = s => s.GetFinlandiaHiihtoResultsAsync(It.IsAny<FilterCollection>());
        
        [SetUp]
        public void Setup()
        {
            ICacheProvider cache = new MemoryCacheProvider();
            m_actualFetcherMock = new Mock<IDataFetchingService>();
            m_actualFetcherMock.Setup(m_mockedFunc)
                .ReturnsAsync(() =>
                    new FinlandiaHiihtoResultsCollection(FinlandiaHiihtoSingleResultDummyDataProvider.Create().ToMany()));
            
            m_testDecorator = new SimpleDataFetcherCacheDecorator(m_actualFetcherMock.Object, cache);
        }

        [Test]
        public async Task TestDifferingSearches()
        {
            var filters1 = CreateSuperMinimalFilters(2000);
            var filters2 = CreateSuperMinimalFilters(2010);

            var res1 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(filters1);
            var res2 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(filters2);
            
            Assert.AreNotSame(res1, res2);
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(2));
        }
        
        [Test]
        public async Task TestIdenticalSearches()
        {
            var filters1 = CreateSuperMinimalFilters(2000);
            var filters2 = CreateSuperMinimalFilters(2000);
            Assert.AreNotSame(filters1, filters2);

            var res1 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(filters1);
            var res2 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(filters2);
            var res3 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(filters1);
            
            Assert.AreSame(res1, res2);
            Assert.AreSame(res1, res3);
            Assert.AreSame(res2, res3);
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(1));
        }

        [Test]
        public async Task TestDifferingAndIdenticalSearches()
        {
            var filters1 = CreateSuperMinimalFilters(2000);
            var filters2 = CreateSuperMinimalFilters(2010);
            var filters3 = CreateSuperMinimalFilters(2000);
            Assert.AreNotSame(filters1, filters3);
            
            var res1 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(filters1);
            var res2 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(filters2);
            var res3 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(filters3);
            
            Assert.AreSame(res1, res3);
            Assert.AreNotSame(res1, res2);
            Assert.AreNotSame(res2, res3);
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(2));
        }

        private static FilterCollection CreateSuperMinimalFilters(int year)
        {
            return new FilterCollection(new FinlandiaCompetitionYearsFilter(year.ToMany()));
        }
    }
}
