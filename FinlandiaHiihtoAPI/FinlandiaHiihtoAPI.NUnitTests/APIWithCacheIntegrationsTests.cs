using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FinlandiaHiihtoAPI.Cache;
using FinlandiaHiihtoAPI.Enums;
using NUnit.Framework;
// TODO Logging

namespace FinlandiaHiihtoAPI.NUnitTests
{
    [TestFixture]
    public class APIWithCacheIntegrationTests
    {
        private CacheDecorator m_testDecorator;
        private IFinlandiaHiihtoAPI m_api;

        [SetUp]
        public void Setup()
        {
            ICacheProvider cache = new MemoryCacheProvider();
            m_api = new FinlandiaHiihtoAPI();
            m_testDecorator = new CacheDecorator(m_api, cache);
        }

        [Test]
        public async Task TestGetDataForIdenticalArgs()
        {
            var args1 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010
            };
            var args2 = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2010
            };

            var res1 = (await m_testDecorator.GetData(args1)).ToList();
            var res2 = (await m_testDecorator.GetData(args2)).ToList();

            Assert.AreEqual(5384, res1.Count);
            Assert.AreEqual(5384, res2.Count);
            Assert.AreEqual(res1.First(), res2.First());
        }
        
        [Test]
        public async Task TestGetDataForDifferingArgs()
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

            var res1 = (await m_testDecorator.GetData(args1)).ToList();
            var res2 = (await m_testDecorator.GetData(args2)).ToList();

            Assert.AreEqual(3177, res1.Count);
            Assert.AreEqual(90, res2.Count);
            Assert.AreNotEqual(res1.First(), res2.First());
        }
        
        [Test]
        public async Task TestGetDataForMultipleSubQueryArgs()
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
            
            var startTime = DateTime.Now;
            var res1 = await m_testDecorator.GetData(args1);
            var endTime = DateTime.Now;
            Debug.WriteLine($"Get data 1 (ms): {(endTime - startTime).TotalMilliseconds}");
            
            startTime = DateTime.Now;
            var res2 = await m_testDecorator.GetData(args2);
            endTime = DateTime.Now;
            Debug.WriteLine($"Get data 2 (ms): {(endTime - startTime).TotalMilliseconds}");
            
            startTime = DateTime.Now;
            var res3 = await m_testDecorator.GetData(args3);
            endTime = DateTime.Now;
            Debug.WriteLine($"Get data 3 (ms): {(endTime - startTime).TotalMilliseconds}");
            
            startTime = DateTime.Now;
            var res4 = await m_testDecorator.GetData(args4);
            endTime = DateTime.Now;
            Debug.WriteLine($"Get data 4 (ms): {(endTime - startTime).TotalMilliseconds}");
            
            Assert.AreEqual(3177, res1.Count());
            Assert.AreEqual(2852, res2.Count());
            Assert.AreEqual(37, res3.Count());
            Assert.AreEqual(7, res4.Count());
        }
    }
}
