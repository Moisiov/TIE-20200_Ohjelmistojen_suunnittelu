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

        public ResultRegisterViewModel(IFinlandiaResultsService finlandiaResultsService)
        {
            RegisterModel = new ResultRegisterModel(finlandiaResultsService);

            CompetitionYears = new ObservableCollection<int>(Enumerable.Range(
                FinlandiaConstants.C_FirstFinlandiaSkiingYear,
                DateTime.Today.Year - FinlandiaConstants.C_FirstFinlandiaSkiingYear + 1).Reverse());
            FinlandiaCompetitionClassItems = new ObservableCollection<FinlandiaHiihtoCompetitionClass>(
                FinlandiaHiihtoCompetitionClasses.FinlandiaCompetitionClasses);
        }

        protected override Task OnDoPopulateAsync()
        {
            return Task.CompletedTask;
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
    }
}
