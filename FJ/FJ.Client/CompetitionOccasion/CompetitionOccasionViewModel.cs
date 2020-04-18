using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.Client.ResultRegister;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using PlotterService;

namespace FJ.Client.CompetitionOccasion
{
    public class CompetitionOccasionViewModel : ViewModelBase<CompetitionOccasionArgs>
    {
        private readonly IPlotService m_plotService;
        
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
        
        private IEnumerable<(string Nationality, int TotalCount)> m_nationalityDistribution;
        public IEnumerable<(string Nationality, int TotalCount)> NationalityDistribution
        {
            get => m_nationalityDistribution;
            set => SetAndRaise(ref m_nationalityDistribution, value);
        }

        private bool m_nationalityDistributionChartOptionIsActive;
        public bool NationalityDistributionChartOptionIsActive
        {
            get => m_nationalityDistributionChartOptionIsActive;
            set => SetAndRaise(ref m_nationalityDistributionChartOptionIsActive, value);
        }
        
        private bool m_nationalityDistributionChartUseBarChart;
        public bool NationalityDistributionChartUseBarChart
        {
            get => m_nationalityDistributionChartUseBarChart;
            set => SetAndRaise(ref m_nationalityDistributionChartUseBarChart, value);
        }
        
        private bool m_nationalityDistributionChartUsePieChart;
        public bool NationalityDistributionChartUsePieChart
        {
            get => m_nationalityDistributionChartUsePieChart;
            set => SetAndRaise(ref m_nationalityDistributionChartUsePieChart, value);
        }

        public ObservableCollection<Top10TeamItemModel> Top10TeamsList { get; set; }
        
        private bool m_top10TeamsIsActive;
        public bool Top10TeamsIsActive
        {
            get => m_top10TeamsIsActive;
            set => SetAndRaise(ref m_top10TeamsIsActive, value);
        }
        
        public CompetitionOccasionViewModel(
            ICompetitionOccasionDataService competitionOccasionDataService, 
            IPlotService plotService)
        {
            m_model = new CompetitionOccasionModel(competitionOccasionDataService);
            m_plotService = plotService;
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
                try
                {
                    await m_model.GetOccasionData(year);
                    TotalParticipants = m_model.TotalParticipants;
                    TotalCompetitions = m_model.TotalCompetitions;
                    NationalityDistribution = m_model.NationalityDistribution;
                    CompetitionList = new ObservableCollection<CompetitionRowItemModel>(m_model.CompetitionList);

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
            
            OccasionYear = null;
            TotalParticipants = null;
            TotalCompetitions = null;
            CompetitionList = null;
            NationalityDistributionChartOptionIsActive = false;
            NationalityDistributionChartUseBarChart = false;
            NationalityDistributionChartUsePieChart = false;
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

        public void NationalityDistributionChartOptionActivation()
        {
            NationalityDistributionChartOptionIsActive = true;
        }
        public void NationalityDistributionChartDeActivation()
        {
            NationalityDistributionChartOptionIsActive = false;
        }
        
        public void NationalityDistributionChartOpen()
        {
            var data = NationalityDistribution?
                .OrderByDescending(x => x.TotalCount)
                .Select(x => new PlotDataPoint
                {
                    Label = x.Nationality, 
                    Value = x.TotalCount
                });

            if (data == null)
            {
                return;
            }
            else if (NationalityDistributionChartUseBarChart)
            {
                m_plotService.GetPlot(data, PlotType.BarPlot, $"Kansalaisuusjakauma {OccasionYear}");
            }
            else                                                              
            {                                                                                              
                m_plotService.GetPlot(data, PlotType.PiePlot, $"Kansalaisuusjakauma {OccasionYear}");         
            }                                                                                              
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
