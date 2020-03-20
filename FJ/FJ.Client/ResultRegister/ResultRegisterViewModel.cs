using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using FJ.Client.Athlete;
using FJ.Client.UICore;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
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
        public ObservableCollection<ResultRegisterItemModel> Results { get; set; }  // TODO: Bind from model with attribute?

        public ObservableCollection<int> CompetitionYears { get; set; }
        public ObservableCollection<FinlandiaHiihtoCompetitionClass> CompetitionClasses { get; set; }
        public ObservableCollection<FinlandiaSkiingGender> Genders { get; set; }
        public ObservableCollection<FinlandiaSkiingAgeGroup> AgeGroups { get; set; }

        public string CurrentTeamString { get; set; }
        public ObservableCollection<string> TeamStrings { get; set; }  // TODO Not binded atm

        public int FromHourSelection { get; set; }
        public int FromMinuteSelection { get; set; }
        public int ToHourSelection { get; set; }
        public int ToMinuteSelection { get; set; }
        public int[] FromHourOptions { get; set; }
        public int[] FromMinuteOptions { get; set; }
        public int[] ToHourOptions { get; set; }
        public int[] ToMinuteOptions { get; set; }

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

            TestCommand = ReactiveCommand.CreateFromTask(TestCall);
            Results = new ObservableCollection<ResultRegisterItemModel>();

            CompetitionYears = new ObservableCollection<int>(Enumerable.Range(
                FinlandiaConstants.C_FirstFinlandiaSkiingYear,
                DateTime.Today.Year - FinlandiaConstants.C_FirstFinlandiaSkiingYear));
            CompetitionClasses = new ObservableCollection<FinlandiaHiihtoCompetitionClass>(
                FinlandiaHiihtoCompetitionClasses.FinlandiaCompetitionClasses);
            Genders = new ObservableCollection<FinlandiaSkiingGender>(
                EnumHelpers.GetEnumValues<FinlandiaSkiingGender>());
            AgeGroups = new ObservableCollection<FinlandiaSkiingAgeGroup>(
                EnumHelpers.GetEnumValues<FinlandiaSkiingAgeGroup>());

            CurrentTeamString = string.Empty;
            TeamStrings = new ObservableCollection<string>();

            FromHourOptions = CommonConstants.S_Hours;
            FromMinuteOptions = CommonConstants.S_Minutes;
            ToHourOptions = CommonConstants.S_Hours;
            ToMinuteOptions = CommonConstants.S_Minutes;

            CurrentFirstNameString = string.Empty;
            CurrentLastNameString = string.Empty;
            FirstNames = new ObservableCollection<string>();
            LastNames = new ObservableCollection<string>();

            CurrentMunicipalityString = string.Empty;
            Municipalities = new ObservableCollection<string>();

            CurrentYearOfBirthString = string.Empty;
            YearsOfBirth = new ObservableCollection<string>();
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
