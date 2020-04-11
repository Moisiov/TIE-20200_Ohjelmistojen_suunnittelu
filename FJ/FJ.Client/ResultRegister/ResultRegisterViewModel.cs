using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using FJ.Client.Athlete;
using FJ.Client.Core;
using FJ.DomainObjects.Enums;
using FJ.DomainObjects.Filters.Core;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.DomainObjects.FinlandiaHiihto.Filters;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Utils;
using FJ.Utils.FinlandiaUtils;
using ReactiveUI;

namespace FJ.Client.ResultRegister
{
    // TODO Disgusting. Filters + filterCollection needed. Wraps ugly as well: Force line ending in xaml before name filters.
    public class ResultRegisterViewModel : ViewModelBase
    {
        private ResultRegisterModel m_model;

        public ReactiveCommand<Unit, Unit> TestCommand { get; }
        public ReactiveCommand<Unit, Unit> FilterTestCommand { get; }
        public ObservableCollection<ResultRegisterItemModel> Results { get; set; }  // TODO: Bind from model with attribute?

        public ObservableCollection<int> CompetitionYears { get; set; }
        public ObservableCollection<FinlandiaHiihtoCompetitionClass> CompetitionClasses { get; set; }
        public ObservableCollection<Gender> Genders { get; set; }
        public ObservableCollection<FinlandiaSkiingAgeGroup> AgeGroups { get; set; }

        public string CurrentTeamString { get; set; }
        public ObservableCollection<string> TeamStrings { get; set; }  // TODO Not binded atm
        
        public TimeSpan? TimeSelection { get; set; }

        public string CurrentFirstNameString { get; set; }
        public string CurrentLastNameString { get; set; }
        public ObservableCollection<string> FirstNames { get; set; } // TODO Not binded atm
        public ObservableCollection<string> LastNames { get; set; } // TODO Not binded atm

        public string CurrentMunicipalityString { get; set; }
        public ObservableCollection<string> Municipalities { get; set; }  // TODO Not binded atm

        public string CurrentYearOfBirthString { get; set; }
        public ObservableCollection<string> YearsOfBirth { get; set; }  // TODO Not binded atm

        public ResultRegisterViewModel(ILatestFinlandiaResultsService latestFinlandiaResultsService)
        {
            m_model = new ResultRegisterModel(latestFinlandiaResultsService);

            FilterTestCommand = ReactiveCommand.CreateFromTask(FilterTestCall);
            TestCommand = ReactiveCommand.CreateFromTask(TestCall);
            Results = new ObservableCollection<ResultRegisterItemModel>();

            CompetitionYears = new ObservableCollection<int>(Enumerable.Range(
                FinlandiaConstants.C_FirstFinlandiaSkiingYear,
                DateTime.Today.Year - FinlandiaConstants.C_FirstFinlandiaSkiingYear));
            CompetitionClasses = new ObservableCollection<FinlandiaHiihtoCompetitionClass>(
                FinlandiaHiihtoCompetitionClasses.FinlandiaCompetitionClasses);
            Genders = new ObservableCollection<Gender>(
                EnumHelpers.GetEnumValues<Gender>());
            AgeGroups = new ObservableCollection<FinlandiaSkiingAgeGroup>(
                EnumHelpers.GetEnumValues<FinlandiaSkiingAgeGroup>());

            CurrentTeamString = string.Empty;
            TeamStrings = new ObservableCollection<string>();

            CurrentFirstNameString = string.Empty;
            CurrentLastNameString = string.Empty;
            FirstNames = new ObservableCollection<string>();
            LastNames = new ObservableCollection<string>();

            CurrentMunicipalityString = string.Empty;
            Municipalities = new ObservableCollection<string>();

            CurrentYearOfBirthString = string.Empty;
            YearsOfBirth = new ObservableCollection<string>();
        }

        public async Task FilterTestCall()
        {
            using (Navigator.ShowLoadingScreen())
            {
                var yearsFilter = new FinlandiaCompetitionYearsFilter(2019.ToMany());
                var timeFilter = new FinlandiaResultTimeRangeFilter(null, new TimeSpan(2, 0, 0));
                var homeCitiesFilter = new FinlandiaHomeCitiesFilter(new[] { "Hämeenlinna", "Vantaa" });
                var resultGeneralFilter = new FinlandiaPositionRangeGeneralFilter(null, 10);
                
                var filterCollection = new FilterCollection(
                    yearsFilter,
                    timeFilter,
                    homeCitiesFilter,
                    resultGeneralFilter);
                
                var res = await m_model.GetFinlandiaResultsAsync(filterCollection);
                Results = new ObservableCollection<ResultRegisterItemModel>(res);
                RaisePropertyChanged(nameof(Results));
            }
        }

        public async Task TestCall()
        {
            using (Navigator.ShowLoadingScreen())
            {
                var res = await m_model.GetLatestFinlandiaResultsAsync();
                Results = new ObservableCollection<ResultRegisterItemModel>(res);
                RaisePropertyChanged(nameof(Results));
            }
        }

        public void NavigationCommand()
        {
            var args = new AthleteCardArgs
            {
                AthleteFirstName = Results.FirstOrDefault()?.FirstName ?? "Rocky",
                AthleteLastName = Results.FirstOrDefault()?.LastName ?? "Balboa"
            };

            Navigator.DoNavigateTo<AthleteCardView>(args);
        }

        protected override void DoRefreshInternal()
        {
            // TODO Remove filter selections
            Results = new ObservableCollection<ResultRegisterItemModel>();
            RaisePropertyChanged(nameof(Results));
        }
    }
}
