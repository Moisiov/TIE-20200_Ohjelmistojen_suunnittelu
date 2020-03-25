using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.FinlandiaHiihtoAPI.Exceptions;
using FJ.FinlandiaHiihtoAPI.Utils;
using NUnit.Framework;

namespace FJ.FinlandiaHiihtoAPI.NUnitTests
{
    [TestFixture]
    public class FinlandiaHiihtoAPITests
    {
        private IFinlandiaHiihtoAPI m_api;
        
        [SetUp]
        public void Setup()
        {
            m_api = new FinlandiaHiihtoAPI();
        }
        
        [Test]
        public void TestInvalidArgument()
        {
            var args = new FinlandiaHiihtoAPISearchArgs
            {
                CompetitionType = "False Type"
            };
            
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await m_api.GetData(args));
            Assert.AreEqual("Invalid filter arguments.", ex.Message);
        }
        
        [Test]
        public async Task TestMultipleSearches()
        {
            static FinlandiaHiihtoAPISearchArgs GetNew()
            {
                return new FinlandiaHiihtoAPISearchArgs();
            }

            var taskList = new List<Task<IEnumerable<FinlandiaHiihtoAPISearchResultRow>>>
            {
                m_api.GetData(GetNew().SetPropertyValue(x => x.FirstName, "Irmeli")),
                m_api.GetData(GetNew().SetPropertyValue(x => x.Year, 2000)),
                m_api.GetData(GetNew().SetPropertyValue(x => x.LastName, "Metsola")),
                m_api.GetData(GetNew().SetPropertyValue(x => x.CompetitionType, "V53")),
                m_api.GetData(GetNew().SetPropertyValue(x => x.CompetitorHomeTown, "Akaa")),
                m_api.GetData(GetNew().SetPropertyValue(x => x.Team, "Kelan Liikuntaseura")),
                m_api.GetData(GetNew().SetPropertyValue(x => x.Nationality, "CA")),
                m_api.GetData(GetNew()
                    .SetPropertyValue(x => x.CompetitorHomeTown, "Mikkeli")
                    .SetPropertyValue(x => x.Gender, "N"))
            };

            await Task.WhenAll(taskList.ToArray());
            foreach (var task in taskList)
            {
                var res = await task;
                Assert.IsTrue(res.Any());
            }
        }
        
        [Test]
        public async Task TestNoResults()
        {
            var args = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 1994,
                CompetitionType = "V42"
            };
            
            var result = await m_api.GetData(args);
            Assert.AreEqual(0, result.Count());
        }
        
        [Test]
        public async Task TestSingleResult()
        {
            var args = new FinlandiaHiihtoAPISearchArgs
            {
                Year = 2006,
                FirstName = "Irmeli",
                LastName = "Thomasson"
            };
            
            var result = await m_api.GetData(args);
            Assert.AreEqual(1, result.Count());
        }
        
        [Test]
        public void TestTooManyResults()
        {
            var args = new FinlandiaHiihtoAPISearchArgs();
            var ex = Assert.ThrowsAsync<TooMuchResultsExceptions>(async () => await m_api.GetData(args));
            Assert.AreEqual("Too many results to show.", ex.Message);
        }
    }
}