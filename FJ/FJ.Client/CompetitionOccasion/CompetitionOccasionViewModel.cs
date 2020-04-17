using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.Client.ResultRegister;
using FJ.ServiceInterfaces.FinlandiaHiihto;

namespace FJ.Client.CompetitionOccasion
{
    public class CompetitionOccasionViewModel : ViewModelBase<CompetitionOccasionArgs>
    {
        private CompetitionOccasionModel m_model;

        public ObservableCollection<CompetitionRowItemModel> CompetitionList { get; set; }
        
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
        
        public ObservableCollection<Top10TeamItemModel> Top10TeamsList { get; set; }
        
        private bool m_top10TeamsIsActive;
        public bool Top10TeamsIsActive
        {
            get => m_top10TeamsIsActive;
            set => SetAndRaise(ref m_top10TeamsIsActive, value);
        }
        
        public CompetitionOccasionViewModel(ICompetitionOccasionDataService competitionOccasionDataService)
        {
            m_model = new CompetitionOccasionModel(competitionOccasionDataService);
        }
        
        protected override async Task DoPopulateAsync()
        {
            using (Navigator.ShowLoadingScreen())
            {
                await base.DoPopulateAsync();
                OccasionYear = Argument.Year;

                if (OccasionYear == null)
                {
                    return;
                }

                var year = (int)OccasionYear;
                await m_model.GetOccasionData(year);
                TotalParticipants = m_model.TotalParticipants;
                TotalCompetitions = m_model.TotalCompetitions;
                CompetitionList = new ObservableCollection<CompetitionRowItemModel>(m_model.CompetitionList);
                
                RaisePropertiesChanged();
            }
        }

        protected override async Task DoRefreshInternalAsync()
        {
            await base.DoRefreshInternalAsync();
            
            OccasionYear = null;
            TotalParticipants = null;
            TotalCompetitions = null;
            CompetitionList = null;
            NationalityDistributionChartIsActive = false;
            Top10TeamsIsActive = false;
            
            RaisePropertiesChanged();
        }
        
        public void NavigateToResultRegisterWithOccasion()
        {
            // TODO Anna vuosi navigaatio argumenttina.
            Navigator.DoNavigateTo<ResultRegisterView>();
        }
        
        public void NavigateToResultRegisterWithCompetition(string competitionType)
        {
            // TODO Anna Year+competitionType navigaatioargumenttina.
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
        
        public void Top10TeamsActivation(IEnumerable<Top10TeamItemModel> top10TeamsList)
        {
            Top10TeamsList = new ObservableCollection<Top10TeamItemModel>(top10TeamsList);
            Top10TeamsIsActive = true;
            
            RaisePropertyChanged(nameof(Top10TeamsList));
            RaisePropertyChanged(nameof(Top10TeamsIsActive));
        }
        public void Top10TeamsDeActivation()
        {
            Top10TeamsIsActive = false;
        }
    }
}
