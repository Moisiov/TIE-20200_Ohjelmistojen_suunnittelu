using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FJ.Client.Athlete;
using FJ.Client.CompetitionOccasion;
using FJ.Client.Core.Register;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Utils.FinlandiaUtils;

namespace FJ.Client.ResultRegister
{
    public class ResultRegisterViewModel : RegisterViewModelBase<ResultRegisterModel, ResultRegisterArgs>
    {
        #region FJFilterMultiComboBox selectable items
        
        public ObservableCollection<int> CompetitionYears { get; }
        public ObservableCollection<FinlandiaHiihtoCompetitionClass> FinlandiaCompetitionClassItems { get; }
        
        #endregion

        public ResultRegisterViewModel(ILatestFinlandiaResultsService latestFinlandiaResultsService)
        {
            RegisterModel = new ResultRegisterModel(latestFinlandiaResultsService);

            CompetitionYears = new ObservableCollection<int>(Enumerable.Range(
                FinlandiaConstants.C_FirstFinlandiaSkiingYear,
                DateTime.Today.Year - FinlandiaConstants.C_FirstFinlandiaSkiingYear + 1));
            FinlandiaCompetitionClassItems = new ObservableCollection<FinlandiaHiihtoCompetitionClass>(
                FinlandiaHiihtoCompetitionClasses.FinlandiaCompetitionClasses);
        }

        protected override async Task OnDoPopulateAsync()
        {
            if (!Argument.CompetitionYears?.Any() == true || !Argument.HomeCities?.Any() == true)
            {
                await Task.CompletedTask;
                return;
            }
            
            // TODO proof-of-concept, real one would accept values from Argument to filters
            using (Navigator.ShowLoadingScreen())
            {
                await ExecuteSearchAsync();
            }
        }

        public void NavigationToAthleteCardCommand()
        {
            var args = RegisterModel.AllItems.FirstOrDefault(x => x.IsSelected)?.GetNavigationArgs();
            Navigator.DoNavigateTo<AthleteCardView>(args);
        }
        
        public void NavigationToCompetitionOccasionCommand()
        {
            var args = new CompetitionOccasionArgs
            {
                Year = RegisterModel.AllItems.FirstOrDefault()?.Year ?? 2019,
            };

            Navigator.DoNavigateTo<CompetitionOccasionView>(args);
        }

        protected override Task DoRefreshInternalAsync()
        {
            RegisterModel.AllItems = new List<ResultRegisterItemModel>();
            RegisterModel.DoClearFilters();
            RaisePropertyChanged(nameof(RegisterModel.AllItems));
            
            return Task.CompletedTask;
        }
    }
}
