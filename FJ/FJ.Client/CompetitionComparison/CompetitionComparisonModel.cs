using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.Client.Core.Register;
using FJ.Client.Core.UIElements.Filters;
using FJ.Client.ResultRegister;
using FJ.DomainObjects;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Filters;
using FJ.ServiceInterfaces.FinlandiaHiihto;

namespace FJ.Client.CompetitionComparison
{
    public class CompetitionComparisonModel : FJNotificationObject
    {
        private readonly List<RegisterFilterModelBase> m_filterModels;
        
        private readonly ICompetitionComparisonDataService m_competitionComparisonDataService;
        
        private FinlandiaCompetitionYearsFilter m_yearsFilter;

        private FinlandiaCompetitionClassesFilter m_competitionClassesFilter;

        public Competition Competition1Info { get; private set; }
        public Competition Competition2Info { get; private set; }
        public int Competition1Participants { get; private set; }
        public int Competition2Participants { get; private set; }
        public FinlandiaHiihtoResultsCollection Competition1ResultsCollection { get; private set; }
        public FinlandiaHiihtoResultsCollection Competition2ResultsCollection { get; private set; }

        public FilterModel_FinlandiaFirstNames FinlandiaFirstNamesFilter  { get; set; }
        public FilterModel_FinlandiaLastNames FinlandiaLastNamesFilter  { get; set; }
        public FilterModel_FinlandiaGenders FinlandiaGenderFilter  { get; set; }
        public FilterModel_FinlandiaHomeCities FinlandiaHomeCitiesFilter  { get; set; }
        public FilterModel_FinlandiaAgeGroups FinlandiaAgeGroupsFilter  { get; set; }
        public FilterModel_FinlandiaTeams FinlandiaTeamsFilter  { get; set; }
        public FilterModel_FinlandiaResultTimeRange FinlandiaResultTimeRangeFilter  { get; set; }
        
        private bool m_enableSearch;
        public bool EnableSearch
        {
            get => m_enableSearch;
            set => SetAndRaise(ref m_enableSearch, value);
        }
        
        public CompetitionComparisonModel(ICompetitionComparisonDataService competitionComparisonDataService)
        {
            m_competitionComparisonDataService = competitionComparisonDataService;
            m_filterModels = new List<RegisterFilterModelBase>();
            SetFilterModels();
        }

        public async Task GetCompetitionComparisonData(
            int comp1Year, FinlandiaHiihtoCompetitionClass comp1Type, 
            int comp2Year, FinlandiaHiihtoCompetitionClass comp2Type,
            bool updateInfoAndParticipants = false)
        {
            var filters = new FilterCollection();
            foreach (var filterModel in m_filterModels)
            {
                filters.Add(filterModel.GetActiveFilters().Where(f => f != null));
            }
            
            m_yearsFilter = new FinlandiaCompetitionYearsFilter(new int[] { comp1Year, comp2Year }.Distinct());
            m_competitionClassesFilter = new FinlandiaCompetitionClassesFilter(
                new[] { comp1Type, comp2Type }.Distinct());
            filters.Add(m_yearsFilter);
            filters.Add(m_competitionClassesFilter);

            var resultRows = 
                await m_competitionComparisonDataService.GetCompetitionComparisonData(filters);

            var comp1ResultRows = resultRows.Results
                .Where(x 
                    => x.CompetitionInfo.Year == comp1Year && x.CompetitionClass.Equals(comp1Type))
                .OrderBy(x => x.PositionGeneral);
            Competition1ResultsCollection = new FinlandiaHiihtoResultsCollection(comp1ResultRows);

            var comp2ResultRows = resultRows.Results
                .Where(x 
                    => x.CompetitionInfo.Year == comp2Year && x.CompetitionClass.Equals(comp2Type))
                .OrderBy(x => x.PositionGeneral);
            Competition2ResultsCollection = new FinlandiaHiihtoResultsCollection(comp2ResultRows);

            if (updateInfoAndParticipants)
            {
                Competition1Info = Competition1ResultsCollection.Results.FirstOrDefault()?.CompetitionInfo;
                Competition2Info = Competition2ResultsCollection.Results.FirstOrDefault()?.CompetitionInfo;
                Competition1Participants = Competition1ResultsCollection.Results.Count();
                Competition2Participants = Competition2ResultsCollection.Results.Count();   
            }
        }

        protected void SetFilterModels()
        {
            // Make sure there won't be duplicate filters
            m_filterModels.ForEach(f => f.FilterChanged -= OnFilterChanged);
            m_filterModels.Clear();

            var filterProps = GetType()
                .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(RegisterFilterModelBase)
                            || p.PropertyType.IsSubclassOf(typeof(RegisterFilterModelBase)));
            
            foreach (var filterModelProperty in filterProps)
            {
                var filterModel = filterModelProperty.GetValue(this) as RegisterFilterModelBase;
                if (filterModel == null)
                {
                    filterModel = Activator.CreateInstance(filterModelProperty.PropertyType) as RegisterFilterModelBase;
                    if (filterModel == null)
                    {
                        throw new InvalidOperationException($"Invalid register filter model: {filterModelProperty.Name}");
                    }
                    
                    filterModel.FilterChanged += OnFilterChanged;
                }
                
                m_filterModels.Add(filterModel);
                filterModelProperty.SetValue(this, filterModel);
            }
        }
        
        private void OnFilterChanged()
        {
            EnableSearch = true;
        }
    }
}
