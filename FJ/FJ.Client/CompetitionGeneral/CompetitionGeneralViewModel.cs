using System;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.DomainObjects;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;

namespace FJ.Client.CompetitionGeneral
{
    public class CompetitionGeneralViewModel : ViewModelBase<CompetitionGeneralArgs>
    {
        private readonly CompetitionGeneralModel m_competitionGeneralModel;
        
        public int? CompetitionYear { get; set; }
        public Competition CompetitionInfo { get; set; }
        public FinlandiaHiihtoCompetitionClass CompetitionClass { get; set; }
        public ListItemLimitWrapper<FinlandiaHiihtoSingleResult> CompetitionResults { get; set; }
        public int? CompetitionParticipants { get; set; }

        public CompetitionGeneralViewModel(ICompetitionDataService competitionDataService)
        {
            m_competitionGeneralModel = new CompetitionGeneralModel(competitionDataService);
        }
        
        protected override async Task DoPopulateAsync()
        {
            using (Navigator.ShowLoadingScreen())
            {
                await base.DoPopulateAsync();
                CompetitionYear = Argument.CompetitionYear;
                CompetitionClass = Argument.CompetitionClass;

                if (CompetitionYear == null || CompetitionClass == null)
                {
                    return;
                }

                await PoPulateView((int) CompetitionYear, CompetitionClass);
            }
        }
        
        protected override async Task DoRefreshInternalAsync()
        {
            await base.DoRefreshInternalAsync();

            CompetitionYear = null;
            CompetitionInfo = null;
            CompetitionParticipants = null;
            CompetitionResults = null;

            RaisePropertiesChanged();
        }

        private async Task PoPulateView(int year, FinlandiaHiihtoCompetitionClass competitionClass)
        {
            try
            {
                await m_competitionGeneralModel.GetCompetitionGeneralData((int)year, competitionClass);
                    
                CompetitionInfo = m_competitionGeneralModel.CompetitionInfo;
                CompetitionParticipants = m_competitionGeneralModel.CompetitionParticipants;
                CompetitionResults = new ListItemLimitWrapper<FinlandiaHiihtoSingleResult>(
                    m_competitionGeneralModel.CompetitionResultsCollection.Results);

                RaisePropertiesChanged();
            }
            catch (Exception e)
            {
                await DoRefreshInternalAsync();
                Navigator.ShowErrorMessage(e.Message);
            }
        }
    }
}
