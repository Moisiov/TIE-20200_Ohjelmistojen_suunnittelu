using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.Client.ResultRegister;

namespace FJ.Client.CompetitionOccasion
{
    public class CompetitionOccasionViewModel : ViewModelBase<CompetitionOccasionArgs>
    {
        private CompetitionOccasionModel m_model;

        private List<CompetitionRowItemModel> m_competitionList;
        public List<CompetitionRowItemModel> CompetitionList
        {
            get => m_competitionList;
            set => SetAndRaise(ref m_competitionList, value);
        }
        
        private int? m_occasionYear;
        public int? OccasionYear
        {
            get => m_occasionYear;
            set => SetAndRaise(ref m_occasionYear, value);
        }
        
        private int? m_totalParticipants;
        public int? TotalParticipants
        {
            get => m_totalParticipants;
            set => SetAndRaise(ref m_totalParticipants, value);
        }
        
        private int? m_totalCompetitions;
        public int? TotalCompetitions
        {
            get => m_totalCompetitions;
            set => SetAndRaise(ref m_totalCompetitions, value);
        }

        // TODO Janne
        // private Chart m_nationalityDistributionChart;
        // public Chart NationalityDistributionChart
        // {
        //     get => m_nationalityDistributionChart;
        //     set => SetAndRaise(ref m_nationalityDistributionChart, value);
        // }

        private bool m_nationalityDistributionChartIsActive;
        public bool NationalityDistributionChartIsActive
        {
            get => m_nationalityDistributionChartIsActive;
            set => SetAndRaise(ref m_nationalityDistributionChartIsActive, value);
        }
        
        private List<Top10TeamItemModel> m_top10TeamsList;
        public List<Top10TeamItemModel> Top10TeamsList
        {
            get => m_top10TeamsList;
            set => SetAndRaise(ref m_top10TeamsList, value);
        }
        
        private bool m_top10TeamsIsActive;
        public bool Top10TeamsIsActive
        {
            get => m_top10TeamsIsActive;
            set => SetAndRaise(ref m_top10TeamsIsActive, value);
        }
        
        public CompetitionOccasionViewModel()
        {
            m_model = new CompetitionOccasionModel();
        }
        
        public override void DoPopulate()
        {
            base.DoPopulate();
            OccasionYear = Argument.Year ?? 2019;

            m_model.GetOccasionData(OccasionYear);
            TotalParticipants = m_model.TotalParticipants;
            TotalCompetitions = m_model.TotalCompetitions;
            CompetitionList = m_model.CompetitionList;
        }

        protected override async Task DoRefreshInternalAsync()
        {
            OccasionYear = null;
            TotalParticipants = null;
            CompetitionList = null;
            NationalityDistributionChartIsActive = false;
            Top10TeamsIsActive = false;
            await Task.CompletedTask;
        }
        
        public void NavigateToResultRegisterWithOccasion()
        {
            // TODO Anna vuosi navigaatio argumenttina.
            Navigator.DoNavigateTo<ResultRegisterView>();
        }
        
        public void NavigateToResultRegisterWithCompetition(string competitionType)
        {
            // TODO Anna Year+competitionType navigaatioargumenttina.
            Console.WriteLine(competitionType);
            Navigator.DoNavigateTo<ResultRegisterView>();
        }

        public void NationalityDistributionChartActivation()
        {
            // TODO Lisää modelilta saatava kansallisuus distribuutio chartin entryihin.
            NationalityDistributionChartIsActive = true;
        }
        public void NationalityDistributionChartDeActivation()
        {
            NationalityDistributionChartIsActive = false;
        }
        
        public void Top10TeamsActivation(List<Top10TeamItemModel> top10TeamsList)
        {
            // TODO Muuta parametri listaksi ja lisää saatu lista UI DataTemplaattiin.
            Top10TeamsList = top10TeamsList;
            Top10TeamsIsActive = true;
        }
        public void Top10TeamsDeActivation()
        {
            Top10TeamsIsActive = false;
        }
    }
}
