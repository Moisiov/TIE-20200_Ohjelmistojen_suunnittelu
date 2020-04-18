using System;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.Client.ResultRegister;
using FJ.DomainObjects;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;

namespace FJ.Client.CompetitionComparison
{
    public class CompetitionComparisonViewModel : ViewModelBase<CompetitionComparisonArgs>
    {
        public CompetitionComparisonModel CompetitionComparisonModel { get; set; }
        
        public int? Competition1Year { get; set; }
        public int? Competition2Year { get; set; }
        public Competition Competition1Info { get; set; }
        public Competition Competition2Info { get; set; }
        public FinlandiaHiihtoCompetitionClass Competition1Class { get; set; }
        public FinlandiaHiihtoCompetitionClass Competition2Class { get; set; }
        public ListItemLimitWrapper<FinlandiaHiihtoSingleResult> Competition1Results { get; set; }
        public ListItemLimitWrapper<FinlandiaHiihtoSingleResult> Competition2Results { get; set; }
        public int? Competition1Participants { get; set; }
        public int? Competition2Participants { get; set; }

        public CompetitionComparisonViewModel(ICompetitionComparisonDataService competitionOccasionDataService)
        {
            CompetitionComparisonModel = new CompetitionComparisonModel(competitionOccasionDataService);
        }
        
        protected override async Task DoPopulateAsync()
        {
            using (Navigator.ShowLoadingScreen())
            {
                await base.DoPopulateAsync();
                Competition1Year = Argument.Competition1Year;
                Competition2Year = Argument.Competition2Year;
                Competition1Class = Argument.Competition1Class;
                Competition2Class = Argument.Competition2Class;

                if (Competition1Year == null 
                    || Competition2Year == null 
                    || Competition1Class == null 
                    || Competition2Class == null)
                {
                    return;
                }

                try
                {
                    await CompetitionComparisonModel.GetCompetitionComparisonData(
                        (int)Competition1Year, Competition1Class,
                        (int)Competition2Year, Competition2Class,
                        true);
                    
                    Competition1Info = CompetitionComparisonModel.Competition1Info;
                    Competition2Info = CompetitionComparisonModel.Competition2Info;
                    Competition1Participants = CompetitionComparisonModel.Competition1Participants;
                    Competition2Participants = CompetitionComparisonModel.Competition2Participants;
                    Competition1Results = new ListItemLimitWrapper<FinlandiaHiihtoSingleResult>(
                        CompetitionComparisonModel.Competition1ResultsCollection.Results);
                    Competition2Results = new ListItemLimitWrapper<FinlandiaHiihtoSingleResult>(
                        CompetitionComparisonModel.Competition2ResultsCollection.Results);
                
                    RaisePropertiesChanged();
                }
                catch (Exception e)
                {
                    await DoRefreshInternalAsync();
                    Navigator.ShowErrorMessage(e.Message);
                }
            }
        }

        protected override async Task DoRefreshInternalAsync()
        {
            await base.DoRefreshInternalAsync();

            Competition1Year = null;
            Competition2Year = null;
            Competition1Info = null;
            Competition2Info = null;
            Competition1Participants = null;
            Competition2Participants = null;
            Competition1Results = null;
            Competition2Results = null;

            RaisePropertiesChanged();
        }

        public async void FilterResults()
        {
            if (Competition1Year == null 
                || Competition2Year == null 
                || Competition1Class == null 
                || Competition2Class == null)
            {
                return;
            }

            using (Navigator.ShowLoadingScreen())
            {
                try 
                {
                    await CompetitionComparisonModel.GetCompetitionComparisonData(
                        (int)Competition1Year, Competition1Class,
                        (int)Competition2Year, Competition2Class);

                    Competition1Results = new ListItemLimitWrapper<FinlandiaHiihtoSingleResult>(
                        CompetitionComparisonModel.Competition1ResultsCollection.Results);
                    Competition2Results = new ListItemLimitWrapper<FinlandiaHiihtoSingleResult>(
                        CompetitionComparisonModel.Competition2ResultsCollection.Results);
                    
                    RaisePropertiesChanged();
                }
                catch (Exception e)
                {
                    await DoRefreshInternalAsync();
                    Navigator.ShowErrorMessage(e.Message);
                }
            }
        }

        public void NavigateToResultRegisterWithCompetitions(string competitionType)
        {
            // TODO Anna Year+competitionType navigaatioargumenttina.
            Navigator.DoNavigateTo<ResultRegisterView>();
        }
    }
}
