using System;
﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.Client.Core.Register;
using FJ.Client.Core.UIElements.Filters.FilterModels;
using FJ.DomainObjects.Enums;
using FJ.DomainObjects.Filters.CommonFilters;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.DomainObjects.FinlandiaHiihto.Filters;
using FJ.ServiceInterfaces.FinlandiaHiihto;

namespace FJ.Client.ResultRegister
{
    public class ResultRegisterModel : RegisterModelBase<ResultRegisterItemModel>
    {
        private readonly ILatestFinlandiaResultsService m_latestFinlandiaResultsService;
        
        public FilterModel_FinlandiaFirstNames FinlandiaFirstNamesFilter  { get; set; }
        public FilterModel_FinlandiaLastNames FinlandiaLastNamesFilter  { get; set; }
        public FilterModel_FinlandiaGender FinlandiaGenderFilter  { get; set; }
        public FilterModel_FinlandiaHomeCities FinlandiaHomeCitiesFilter  { get; set; }
        public FilterModel_FinlandiaYearsOfBirth FinlandiaYearsOfBirthFilter  { get; set; }
        
        public FilterModel_FinlandiaCompetitionYears CompetitionYearsFilter { get; set; }
        public FilterModel_FinlandiaCompetitionClasses CompetitionClassesFilter { get; set; }
        public FilterModel_FinlandiaAgeGroups FinlandiaAgeGroupsFilter  { get; set; }
        public FilterModel_FinlandiaTeams FinlandiaTeamsFilter  { get; set; }
        public FilterModel_FinlandiaResultTimeRange FinlandiaResultTimeRangeFilter  { get; set; }

        public ResultRegisterModel(ILatestFinlandiaResultsService latestFinlandiaResultsService)
        {
            m_latestFinlandiaResultsService = latestFinlandiaResultsService;
        }
 
        protected override async Task DoExecuteSearchInternalAsync(FilterCollection filters)
        {
            // TODO some hard-coded limitations at this point of development
            var collection = await m_latestFinlandiaResultsService.GetFinlandiaResultsAsync(filters);
            AllItems = collection.Results
                .OrderBy(x => x.PositionGeneral)
                .Take(100)
                .Select(x => (ResultRegisterItemModel)x)
                .ToList();
            
            RaisePropertyChanged(() => AllItems);
        }
    }
    
    #region Filters
    
    public class FilterModel_FinlandiaCompetitionYears : FJFilterModel_MultiComboBox<int, FinlandiaCompetitionYearsFilter>
    {
        public FilterModel_FinlandiaCompetitionYears()
            : base(years => new FinlandiaCompetitionYearsFilter(years))
        {
        }
    }
    
    public class FilterModel_FinlandiaCompetitionClasses
        : FJFilterModel_MultiComboBox<FinlandiaHiihtoCompetitionClass, FinlandiaCompetitionClassesFilter>
    {
        public FilterModel_FinlandiaCompetitionClasses()
            : base(classes => new FinlandiaCompetitionClassesFilter(classes))
        {
        }
    }
    
    public class FilterModel_FinlandiaGender : FJFilterModel_NullableEnumComboBox<Gender, GenderFilter>
    {
        public FilterModel_FinlandiaGender()
            : base(gender => gender.HasValue ? new GenderFilter(new[] { gender.Value }) : null)
        {
        }
    }
    
    public class FilterModel_FinlandiaAgeGroups
        : FJFilterModel_MultiEnumComboBox<FinlandiaSkiingAgeGroup, FinlandiaAgeGroupsFilter>
    {
        public FilterModel_FinlandiaAgeGroups()
            : base(ageGroups => new FinlandiaAgeGroupsFilter(ageGroups))
        {
        }
    }
    
    public class FilterModel_FinlandiaTeams : FJFilterModel_MultiTextBox<FinlandiaHomeCitiesFilter>
    {
        public FilterModel_FinlandiaTeams()
            : base(teams => new FinlandiaHomeCitiesFilter(teams))
        {
        }
    }
    
    public class FilterModel_FinlandiaFirstNames : FJFilterModel_MultiTextBox<FinlandiaFirstNamesFilter>
    {
        public FilterModel_FinlandiaFirstNames()
            : base(names => new FinlandiaFirstNamesFilter(names))
        {
        }
    }
    
    public class FilterModel_FinlandiaLastNames : FJFilterModel_MultiTextBox<FinlandiaLastNamesFilter>
    {
        public FilterModel_FinlandiaLastNames()
            : base(names => new FinlandiaLastNamesFilter(names))
        {
        }
    }

    public class FilterModel_FinlandiaHomeCities : FJFilterModel_MultiTextBox<FinlandiaHomeCitiesFilter>
    {
        public FilterModel_FinlandiaHomeCities()
            : base(cities => new FinlandiaHomeCitiesFilter(cities))
        {
        }
    }
    
    public class FilterModel_FinlandiaYearsOfBirth : FJFilterModel_MultiComboBox<int, FinlandiaYearsOfBirthFilter>
    {
        public FilterModel_FinlandiaYearsOfBirth()
            : base(years => new FinlandiaYearsOfBirthFilter(years))
        {
        }
    }
    
    public class FilterModel_FinlandiaResultTimeRange : FJFilterModel_TimeRange<FinlandiaResultTimeRangeFilter>
    {
        public FilterModel_FinlandiaResultTimeRange()
            : base(range => new FinlandiaResultTimeRangeFilter(range))
        {
        }
    }
    
    #endregion
}
