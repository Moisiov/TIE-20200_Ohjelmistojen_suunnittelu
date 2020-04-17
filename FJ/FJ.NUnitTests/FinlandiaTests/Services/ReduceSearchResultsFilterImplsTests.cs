using System;
using System.Collections.Generic;
using System.Linq;
using FinlandiaHiihtoAPI;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto.Filters;
using FJ.NUnitTests.TestData;
using FJ.Services.CoreServices;
using NUnit.Framework;

namespace FJ.NUnitTests.FinlandiaTests.Services
{
    [TestFixture]
    public class ReduceSearchResultsFilterImplsTests : TestFixtureBase
    {
        private IEnumerable<FinlandiaHiihtoAPISearchResultRow> m_resultData;
        private IFilterImplementationProvider m_filterImplementationProvider;

        [SetUp]
        public void Setup()
        {
            m_resultData = ReduceSearchResultsDummyData.GetData();
            m_filterImplementationProvider = new FilterImplementationProvider();
        }

        [Test]
        public void FullNameFilterTest()
        {
            var ageGroupFilter = new FinlandiaFullNameFilter("Mäkinen Joona");
            var filterCollection = new FilterCollection(ageGroupFilter);
            var results = 
                m_resultData.ApplyFilters(filterCollection, m_filterImplementationProvider).ToList();
                
            Assert.AreEqual(2, results.Count);
            foreach (var res in results)
            {
                Assert.AreEqual("Mäkinen Joona", res.FullName);
            }
        }
        
        [Test]
        public void ResultTimeRangeFilterTest()
        {
            var start = new TimeSpan(1, 30, 00); 
            var end = new TimeSpan(2, 00, 00);
            var ageGroupFilter = new FinlandiaResultTimeRangeFilter(start, end);
            var filterCollection = new FilterCollection(ageGroupFilter);
            var results = 
                m_resultData.ApplyFilters(filterCollection, m_filterImplementationProvider).ToList();
                
            Assert.AreEqual(2, results.Count);
            foreach (var res in results)
            {
                Assert.IsTrue(res.Result >= start && res.Result <= end);
            }
        }
        
        [Test]
        public void PositionRangeGeneralFilterTest()
        {
            const int start = 5;
            const int end = 10;
            var ageGroupFilter = new FinlandiaPositionRangeGeneralFilter(start, end);
            var filterCollection = new FilterCollection(ageGroupFilter);
            var results = 
                m_resultData.ApplyFilters(filterCollection, m_filterImplementationProvider).ToList();
                
            Assert.AreEqual(3, results.Count);
            foreach (var res in results)
            {
                Assert.IsTrue(res.Position >= start && res.Position <= end);
            }
        }
        
        [Test]
        public void PositionRangeMenTest()
        {
            const int start = 5;
            const int end = 10;
            var ageGroupFilter = new FinlandiaPositionRangeMenFilter(start, end);
            var filterCollection = new FilterCollection(ageGroupFilter);
            var results = 
                m_resultData.ApplyFilters(filterCollection, m_filterImplementationProvider).ToList();
                
            Assert.AreEqual(2, results.Count);
            foreach (var res in results)
            {
                Assert.IsTrue(res.Position >= start && res.Position <= end);
            }
        }

        [Test]
        public void PositionRangeWomenTest()
        {
            const int start = 5;
            var ageGroupFilter = new FinlandiaPositionRangeWomenFilter(start, null);
            var filterCollection = new FilterCollection(ageGroupFilter);
            var results =
                m_resultData.ApplyFilters(filterCollection, m_filterImplementationProvider).ToList();

            Assert.AreEqual(1, results.Count);
            foreach (var res in results)
            {
                Assert.IsTrue(res.Position >= start);
            }
        }

        [Test]
        public void YearsOfBirthTest()
        {
            var ageGroupFilter = new FinlandiaYearsOfBirthFilter(new[] { 1995, 1990 });
            var filterCollection = new FilterCollection(ageGroupFilter);
            var results = 
                m_resultData.ApplyFilters(filterCollection, m_filterImplementationProvider).ToList();
                
            Assert.AreEqual(2, results.Count);
            foreach (var res in results)
            {
                Assert.IsTrue(res.BornYear == 1995 || res.BornYear == 1990);
            }
        }
    }
}
