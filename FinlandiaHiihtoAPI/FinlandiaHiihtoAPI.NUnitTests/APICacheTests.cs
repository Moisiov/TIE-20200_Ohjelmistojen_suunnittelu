using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FinlandiaHiihtoAPI.Cache;
using FinlandiaHiihtoAPI.Enums;
using Moq;
using NUnit.Framework;

namespace FinlandiaHiihtoAPI.NUnitTests
{
    [TestFixture]
    public class APICacheTests
    {
        private CacheDecorator m_testDecorator;
        private Mock<IFinlandiaHiihtoAPI> m_actualFetcherMock;

        private readonly Expression<Func<IFinlandiaHiihtoAPI, Task<IEnumerable<FinlandiaHiihtoAPISearchResultRow>>>> m_mockedFunc
            = s => s.GetData(It.IsAny<FinlandiaHiihtoAPISearchArgs>());
        
        [SetUp]
        public void Setup()
        {
            ICacheProvider cache = new MemoryCacheProvider();
            m_actualFetcherMock = new Mock<IFinlandiaHiihtoAPI>();
            m_actualFetcherMock.Setup(m_mockedFunc)
                .ReturnsAsync(() =>
                    new List<FinlandiaHiihtoAPISearchResultRow>
                    {
                        new FinlandiaHiihtoAPISearchResultRow(new List<string>
                        { "2000", "P50", "2:20:20.04", "1", "", "", "M", "John Doe", "Tampere", "", "1980", "" })
                    });
            
            m_testDecorator = new CacheDecorator(m_actualFetcherMock.Object, cache);
        }

        [Test]
        public async Task TestDifferingSearchArgs()
        {
            var args1 = CreateSuperMinimalArgs(2000);
            var args2 = CreateSuperMinimalArgs(2010);

            await m_testDecorator.GetData(args1);
            await m_testDecorator.GetData(args2);
            
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(2));
        }
        
        [Test]
        public async Task TestIdenticalSearchArgs()
        {
            var args1 = CreateSuperMinimalArgs(2000);
            var args2 = CreateSuperMinimalArgs(2000);

            await m_testDecorator.GetData(args1);
            await m_testDecorator.GetData(args2);
            await m_testDecorator.GetData(args1);
            
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(1));
        }

        [Test]
        public async Task TestDifferingAndIdenticalArgs()
        {
            var args1 = CreateSuperMinimalArgs(2000);
            var args2 = CreateSuperMinimalArgs(2010);
            var args3 = CreateSuperMinimalArgs(2000);

            await m_testDecorator.GetData(args1);
            await m_testDecorator.GetData(args2);
            await m_testDecorator.GetData(args3);

            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(2));
        }
        
        [Test]
        public async Task TestSubQueryArgs()
        {
            var args1 = CreateSuperMinimalArgs(2010);
            var args2 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.V42
            };

            await m_testDecorator.GetData(args1);
            await m_testDecorator.GetData(args2);

            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(1));
        }
        
        [Test]
        public async Task TestNonSubQueryArgs()
        {
            var args1 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.P50
            };
            var args2 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.V20
            };

            await m_testDecorator.GetData(args1);
            await m_testDecorator.GetData(args2);
            
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(2));
        }
        
        [Test]
        public async Task TestArgsNotModified()
        {
            var args1 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.P50
            };
            var args2 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.P50,
                Gender = FinlandiaGender.Male,
                FirstName = "Janne",
                CompetitorHomeTown = "Espoo"
            };
            
            await m_testDecorator.GetData(args1);
            await m_testDecorator.GetData(args2);
            
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(1));
            Assert.AreEqual(2010, args2.Year);
            Assert.AreEqual(FinlandiaCompetitionType.P50, args2.CompetitionType);
            Assert.AreEqual(FinlandiaGender.Male, args2.Gender);
            Assert.AreEqual("Janne", args2.FirstName);
            Assert.AreEqual("Espoo", args2.CompetitorHomeTown);
        }
        
        [Test]
        public async Task TestMultipleSubQueryArgs()
        {
            var args1 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.P50
            };
            var args2 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.P50,
                Gender = FinlandiaGender.Male,
            };
            var args3 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.P50,
                Gender = FinlandiaGender.Male,
                FirstName = "Janne"
            };
            var args4 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.P50,
                Gender = FinlandiaGender.Male,
                FirstName = "Janne",
                CompetitorHomeTown = "Espoo"
            };
            
            await m_testDecorator.GetData(args1);
            await m_testDecorator.GetData(args2);
            await m_testDecorator.GetData(args3);
            await m_testDecorator.GetData(args4);
            
            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(1));
        }

        [Test]
        public async Task TestMultipleParentQueryArgs()
        {
            var args1 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.P50
            };
            var args2 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                FirstName = "Janne"
            };
            var args3 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010,
                CompetitionType = FinlandiaCompetitionType.P50,
                FirstName = "Janne"
            };
            
            await m_testDecorator.GetData(args1);
            await m_testDecorator.GetData(args2);
            await m_testDecorator.GetData(args3);

            m_actualFetcherMock.Verify(m_mockedFunc, Times.Exactly(2));
        }
        private static FinlandiaHiihtoAPISearchArgs CreateSuperMinimalArgs(int year)
        {
            return new FinlandiaHiihtoAPISearchArgs
            {
                Year = year
            };
        } 
    }
}
