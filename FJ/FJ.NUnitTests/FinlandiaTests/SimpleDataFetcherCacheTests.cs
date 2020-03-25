using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FJ.DomainObjects.FinlandiaHiihto;
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
            = s => s.GetFinlandiaHiihtoResultsAsync(It.IsAny<FinlandiaHiihtoSearchArgs>());
        
        [SetUp]
        public void Setup()
        {
            ICacheProvider cache = new MemoryCacheProvider();
            m_actualFetcherMock = new Mock<IDataFetchingService>();
            m_actualFetcherMock.Setup(m_mockedFunc)
                .ReturnsAsync(() =>
                    new FinlandiaHiihtoResultsCollection(
                        new FinlandiaHiihtoSearchArgs(), FinlandiaHiihtoSingleResultDummyDataProvider.Create().ToMany()));
            
            m_testDecorator = new SimpleDataFetcherCacheDecorator(m_actualFetcherMock.Object, cache);
        }

        [Test]
        public async Task TestDifferingSearchArgs()
        {
            var args1 = CreateSuperMinimalArgs(2000);
            var args2 = CreateSuperMinimalArgs(2010);

            var res1 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(args1);
            var res2 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(args2);
            
            Assert.AreNotSame(res1, res2);
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(2));
        }
        
        [Test]
        public async Task TestIdenticalSearchArgs()
        {
            var args1 = CreateSuperMinimalArgs(2000);
            var args2 = CreateSuperMinimalArgs(2000);
            Assert.AreNotSame(args1, args2);

            var res1 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(args1);
            var res2 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(args2);
            var res3 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(args1);
            
            Assert.AreSame(res1, res2);
            Assert.AreSame(res1, res3);
            Assert.AreSame(res2, res3);
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(1));
        }

        [Test]
        public async Task TestDifferingAndIdenticalArgs()
        {
            var args1 = CreateSuperMinimalArgs(2000);
            var args2 = CreateSuperMinimalArgs(2010);
            var args3 = CreateSuperMinimalArgs(2000);
            Assert.AreNotSame(args1, args3);
            
            var res1 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(args1);
            var res2 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(args2);
            var res3 = await m_testDecorator.GetFinlandiaHiihtoResultsAsync(args3);
            
            Assert.AreSame(res1, res3);
            Assert.AreNotSame(res1, res2);
            Assert.AreNotSame(res2, res3);
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(2));
        }

        private static FinlandiaHiihtoSearchArgs CreateSuperMinimalArgs(int year)
        {
            return new FinlandiaHiihtoSearchArgs
            {
                CompetitionYears = year.ToMany()
            };
        }
    }
}
