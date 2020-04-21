using System;
using System.Linq;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.Client.Core.Register;
using FJ.Client.Core.UIElements.Filters.FilterModels;
using FJ.DomainObjects.Enums;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.DomainObjects.FinlandiaHiihto.Filters;
using FJ.ServiceInterfaces.FinlandiaHiihto;

namespace FJ.Client.ResultRegister
{
    public class ResultRegisterModel : RegisterModelBase<ResultRegisterItemModel>
    {
        private readonly IFinlandiaResultsService m_finlandiaResultsService;
        
        public FilterModel_FinlandiaFirstNames FinlandiaFirstNamesFilter  { get; set; }
        public FilterModel_FinlandiaLastNames FinlandiaLastNamesFilter  { get; set; }
        public FilterModel_FinlandiaGenders FinlandiaGenderFilter  { get; set; }
        public FilterModel_FinlandiaHomeCities FinlandiaHomeCitiesFilter  { get; set; }
        public FilterModel_FinlandiaYearsOfBirth FinlandiaYearsOfBirthFilter  { get; set; }
        
        public FilterModel_FinlandiaCompetitionYears CompetitionYearsFilter { get; set; }
        public FilterModel_FinlandiaCompetitionClasses CompetitionClassesFilter { get; set; }
        public FilterModel_FinlandiaAgeGroups FinlandiaAgeGroupsFilter  { get; set; }
        public FilterModel_FinlandiaTeams FinlandiaTeamsFilter  { get; set; }
        public FilterModel_FinlandiaResultTimeRange FinlandiaResultTimeRangeFilter  { get; set; }

        public ResultRegisterModel(IFinlandiaResultsService finlandiaResultsService)
        {
            m_finlandiaResultsService = finlandiaResultsService;
        }
 
        protected override async Task DoExecuteSearchInternalAsync(FilterCollection filters)
        {
            var collection = await m_finlandiaResultsService.GetFinlandiaResultsAsync(filters);
            AllItems = collection.Results
                .OrderBy(x => x.PositionGeneral)
                .Select(x => (ResultRegisterItemModel)x)
                .ToListItemWrapper();
            
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
    
    public class FilterModel_FinlandiaGenders : FJFilterModel_MultiEnumComboBox<Gender, FinlandiaGenderFilter>
    {
        public FilterModel_FinlandiaGenders()
            : base(genders => new FinlandiaGenderFilter(genders))
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
    
    public class FilterModel_FinlandiaYearsOfBirth : FJFilterModel_MultiTextBox_Parseable<int, FinlandiaYearsOfBirthFilter>
    {
        public FilterModel_FinlandiaYearsOfBirth()
            : base(yearStrings => new FinlandiaYearsOfBirthFilter(yearStrings
                    .Select(s => 
                    {
                        var isParseable = int.TryParse(s, out var year);
                        return new { isParseable, year };
                    })
                    .Where(p => p.isParseable)
                    .Select(p => p.year)))
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
