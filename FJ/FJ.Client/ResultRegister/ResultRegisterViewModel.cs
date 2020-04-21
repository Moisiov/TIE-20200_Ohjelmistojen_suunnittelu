using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FJ.Client.Athlete;
using FJ.Client.CompetitionComparison;
using FJ.Client.CompetitionOccasion;
using FJ.Client.Core.Register;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Utils.FinlandiaUtils;

namespace FJ.Client.ResultRegister
{
    public class ResultRegisterViewModel : RegisterViewModelBase<ResultRegisterModel, ResultRegisterArgs>
    {
        #region Limited selectable items
        
        public ObservableCollection<int> CompetitionYears { get; }
        public ObservableCollection<FinlandiaHiihtoCompetitionClass> FinlandiaCompetitionClassItems { get; }
        
        #endregion

        private bool m_averageSpeedIsActive;
        public bool AverageSpeedIsActive
        {
            get => m_averageSpeedIsActive;
            set => SetAndRaise(ref m_averageSpeedIsActive, value);
        }
        
        private string m_averageSpeed;
        public string AverageSpeed
        {
            get => m_averageSpeed;
            set => SetAndRaise(ref m_averageSpeed, value);
        }
        
        public ResultRegisterViewModel(IFinlandiaResultsService finlandiaResultsService)
        {
            RegisterModel = new ResultRegisterModel(finlandiaResultsService);

            CompetitionYears = new ObservableCollection<int>(Enumerable.Range(
                FinlandiaConstants.C_FirstFinlandiaSkiingYear,
                DateTime.Today.Year - FinlandiaConstants.C_FirstFinlandiaSkiingYear).Reverse());
            FinlandiaCompetitionClassItems = new ObservableCollection<FinlandiaHiihtoCompetitionClass>(
                FinlandiaHiihtoCompetitionClasses.FinlandiaCompetitionClasses);
            
            AverageSpeedIsActive = false;
        }

        protected override async Task OnDoPopulateAsync()
        {
            RegisterModel.DoClearFilters();

            if (Argument.Empty)
            {
                await Task.CompletedTask;
                return;
            }
            
            RegisterModel.CompetitionYearsFilter.AcceptValue(Argument.CompetitionYears.ToList());
            RegisterModel.CompetitionClassesFilter.AcceptValue(Argument.CompetitionClasses.ToList());
            RegisterModel.FinlandiaFirstNamesFilter.AcceptValue(Argument.FirstNames.ToList());
            RegisterModel.FinlandiaLastNamesFilter.AcceptValue(Argument.LastNames.ToList());

            await ExecuteSearchAsync();
        }

        public void NavigateToAthleteCardCommand()
        {
            var args = RegisterModel.AllItems.FirstOrDefault(x => x.IsSelected)?.GetNavigationArgs();
            Navigator.DoNavigateTo<AthleteCardView>(args);
        }
        
        public void NavigateToCompetitionOccasionCommand()
        {
            var args = new CompetitionOccasionArgs
            {
                Year = RegisterModel.AllItems.FirstOrDefault()?.Year ?? 2019,
            };

            Navigator.DoNavigateTo<CompetitionOccasionView>(args);
        }

        public void NavigateToCompetitionComparisonCommand()
        {
            var selected = 
                RegisterModel.AllItems.Where(x => x.IsSelected).ToList();
            if (selected.Count != 2
                || (selected.First().Year == selected.Last().Year
                    && selected.First().CompetitionClass.Equals(selected.Last().CompetitionClass)))
            {
                return;
            }
            
            var args = new CompetitionComparisonArgs
            {
                Competition1Year = selected.First().Year,
                Competition2Year = selected.Last().Year,
                Competition1Class = selected.First().CompetitionClass,
                Competition2Class = selected.Last().CompetitionClass,
            };

            Navigator.DoNavigateTo<CompetitionComparisonView>(args);
        }

        protected override Task DoRefreshInternalAsync()
        {
            RegisterModel.AllItems.Clear();
            RegisterModel.DoClearFilters();
            
            RaisePropertiesChanged();
            
            return Task.CompletedTask;
        }

        public void CalculateAverageSpeedForResults()
        {
            AverageSpeedIsActive = true;
            var totalHours = RegisterModel.AllItems.Sum(x => x.Result.TotalHours);
            var totalKilometers = RegisterModel.AllItems.Sum(x => (int)x.CompetitionClass.Distance);
            AverageSpeed = (totalKilometers / totalHours).ToString("0.00");
        }

        public void AverageSpeedDeactivation()
        {
            AverageSpeedIsActive = false;
        }
    }
}
