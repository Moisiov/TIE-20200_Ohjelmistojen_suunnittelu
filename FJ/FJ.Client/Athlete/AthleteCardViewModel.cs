﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.Client.ResultRegister;
using FJ.DomainObjects;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Utils;
using PlotterService;

namespace FJ.Client.Athlete
{
    public class AthleteCardViewModel : ViewModelBase<AthleteCardArgs>
    {
        private IPlotService m_plotService;
        
        private AthleteCardModel m_model;
        
        private ObservableCollection<AthleteParticipationItemModel> m_participationList;
        public ObservableCollection<AthleteParticipationItemModel> ParticipationList
        {
            get => m_participationList;
            set => SetAndRaise(ref m_participationList, value);
        }

        private Person m_athletePersonalInfo;
        public Person AthletePersonalInfo
        {
            get => m_athletePersonalInfo;
            set => SetAndRaise(ref m_athletePersonalInfo, value);
        }
        
        private bool m_plotOptionsIsActiveIsActive;
        public bool PlotOptionsIsActive
        {
            get => m_plotOptionsIsActiveIsActive;
            set => SetAndRaise(ref m_plotOptionsIsActiveIsActive, value);
        }
        
        private bool m_progressionChartUseLineChart;
        public bool ProgressionChartUseLineChart
        {
            get => m_progressionChartUseLineChart;
            set
            {
                SetAndRaise(ref m_progressionChartUseLineChart, value);
            }
        }

        private bool m_progressionChartUseBarChart;
        public bool ProgressionChartUseBarChart
        {
            get => m_progressionChartUseBarChart;
            set
            {
                SetAndRaise(ref m_progressionChartUseBarChart, value);
            }
        }

        public AthleteCardViewModel(IAthleteResultsService athleteResultsService, IPlotService plotService)
        {
            m_plotService = plotService;
            m_model = new AthleteCardModel(athleteResultsService);
            PlotOptionsIsActive = false;
        }

        protected override async Task DoPopulateAsync()
        {
            using (Navigator.ShowLoadingScreen())
            {
                await base.DoPopulateAsync();
                var athleteFirstName = Argument.AthleteFirstName ?? string.Empty;
                var athleteLastName = Argument.AthleteLastName ?? string.Empty;

                if (athleteFirstName.IsNullOrEmpty())
                {
                    return;
                }

                await m_model.GetAthleteData(athleteFirstName, athleteLastName);
            
                var participationItemModels = m_model.AthletesResultRows.Results
                        .OrderByDescending(x => x.CompetitionInfo.Year)
                        .Select(x => new AthleteParticipationItemModel { ResultRows = x });
            
                ParticipationList = new ObservableCollection<AthleteParticipationItemModel>(participationItemModels);
                AthletePersonalInfo = m_model.Athlete;   
            }
        }

        protected override async Task DoRefreshInternalAsync()
        {
            AthletePersonalInfo = null;
            ParticipationList = null;
            ProgressionChartDeactivation();
            await Task.CompletedTask;
        }
        
        public void PlotOptionsActivation()
        {
            PlotOptionsIsActive = true;
            ProgressionChartUseLineChart = true;
            ProgressionChartUseBarChart = false;
        }
        
        public void ProgressionChartDeactivation()
        {
            PlotOptionsIsActive = false;
            ProgressionChartUseLineChart = false;
            ProgressionChartUseBarChart = false;
        }

        private void ProgressionChartPopulate()
        {
            var data = ParticipationList?
                .Where(x => x.IsSelected)
                .Select(x => new PlotDataPoint
                {
                    Label = $"{x.ResultRows.CompetitionInfo.Year} {x.ResultRows.CompetitionInfo.Name}", 
                    Value = x.ResultRows.Result
                });

            if (data == null)
            {
                return;
            }
            else if (ProgressionChartUseLineChart)
            {
                m_plotService.GetPlot(data, PlotType.TimeSpanLinePlot, "Urheilijan tuloskehitys");
            }
            else                                                              
            {                                                                                              
                m_plotService.GetPlot(data, PlotType.TimeSpanBarPlot, "Urheilijan tuloskehitys");         
            }                                                                                              
        }

        public void NavigateToResultRegister()
        {
            // TODO: Hae valitut AthleteParticipationItemModelit ja mäppää niistä argsit ResultRegisterille (filter collection).
            Navigator.DoNavigateTo<ResultRegisterView>();
        }
    }
}
