using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.Client.ResultRegister;
using FJ.DomainObjects;
using FJ.DomainObjects.Enums;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.Utils;
using SkiaSharp;

namespace FJ.Client.Athlete
{
    public class AthleteCardViewModel : ViewModelBase<AthleteCardArgs>
    {
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
        
        private bool m_progressChartIsActive;
        public bool ProgressChartIsActive
        {
            get => m_progressChartIsActive;
            set => SetAndRaise(ref m_progressChartIsActive, value);
        }
        
        private bool m_progressionChartUseTime;
        public bool ProgressionChartUseTime
        {
            get => m_progressionChartUseTime;
            set
            {
                SetAndRaise(ref m_progressionChartUseTime, value);
                if (ProgressChartIsActive && value)
                {
                    ProgressionChartPopulate();   
                }
            }
        }

        private bool m_progressionChartUsePosition;
        public bool ProgressionChartUsePosition
        {
            get => m_progressionChartUsePosition;
            set
            {
                SetAndRaise(ref m_progressionChartUsePosition, value);
                if (ProgressChartIsActive && value)
                {
                    ProgressionChartPopulate();   
                }
            }
        }

        public AthleteCardViewModel(IAthleteResultsService athleteResultsService)
        {
            m_model = new AthleteCardModel(athleteResultsService);
            ProgressChartIsActive = false;
        }

        public override async Task DoPopulateAsync()
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
                .Select(x => new AthleteParticipationItemModel { ResultRows = x });
            
            ParticipationList = new ObservableCollection<AthleteParticipationItemModel>(participationItemModels);
            AthletePersonalInfo = m_model.Athlete;
        }

        protected override async Task DoRefreshInternalAsync()
        {
            AthletePersonalInfo = null;
            ParticipationList = null;
            ProgressionChartDeactivation();
            await Task.CompletedTask;
        }
        
        public void ProgressionChartActivation()
        {
            ProgressChartIsActive = true;
            ProgressionChartUseTime = true;
            ProgressionChartUsePosition = false;
        }
        
        public void ProgressionChartDeactivation()
        {
            ProgressChartIsActive = false;
            ProgressionChartUseTime = false;
            ProgressionChartUsePosition = false;
        }

        private void ProgressionChartPopulate()
        {
        }

        public void NavigateToResultRegister()
        {
            // TODO: Hae valitut AthleteParticipationItemModelit ja mäppää niistä argsit ResultRegisterille (filter collection).
            Navigator.DoNavigateTo<ResultRegisterView>();
        }
    }
}
