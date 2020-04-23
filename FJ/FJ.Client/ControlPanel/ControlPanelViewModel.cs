using System;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Threading;
using FJ.Client.Athlete;
using FJ.Client.CompetitionGeneral;
using FJ.Client.Core;
using FJ.Client.Core.Common;
using FJ.Client.Core.Services;
using FJ.Client.FrontPage;
using FJ.Client.ResultRegister;
using FJ.Client.Team;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.DomainObjects.FinlandiaHiihto.Enums;
using FJ.ServiceInterfaces.Weather;
using FJ.Utils;
using ReactiveUI;

namespace FJ.Client.ControlPanel
{
    public class ControlPanelViewModel : ViewModelBase
    {
        private const string c_shortTimeFormatString = "HH':'mm";
        private const string c_longTimeFormatString = "HH':'mm':'ss";
        private const string c_hardCodedLocation = "Lahti";
        
        private readonly IControlPanelRegionController m_controlPanelRegionController;
        private readonly IWeatherService m_weatherService;

        public bool IsExpanded => m_controlPanelRegionController.CurrentControlPanelSizeOption == ControlPanelSizeOption.Expanded;

        public ReactiveCommand<Unit, Unit> GetWeatherCommand { get; }

        private string m_currentTimeText;
        public string CurrentTime
        {
            get => m_currentTimeText;
            set => SetAndRaise(ref m_currentTimeText, value);
        }

        private string m_currentTemperature;
        public string CurrentTemperatureString => IsExpanded
            ? $"{c_hardCodedLocation}: {m_currentTemperature ?? string.Empty}"
            : m_currentTemperature ?? string.Empty;
        
        public string CurrentWindSpeed { get; set; }

        private IDisposable m_updateWeatherSub;
        private ControlPanelModel m_model;

        public ControlPanelViewModel(IControlPanelRegionController controlPanelRegionController, IWeatherService weatherService)
        {
            m_controlPanelRegionController = controlPanelRegionController;
            m_weatherService = weatherService;

            m_model = new ControlPanelModel();
            
            RefreshTimeText();
            
            GetWeatherCommand = ReactiveCommand.CreateFromTask(GetCurrentWeather);
            GetWeatherCommand.Execute();
            var weatherTimer = new DispatcherTimer(
                TimeSpan.FromMinutes(10),
                DispatcherPriority.Normal,
                (o, e) => OnWeatherTimerTick());
            weatherTimer.Start();

            var clockTimer = new DispatcherTimer(
                TimeSpan.FromSeconds(1),
                DispatcherPriority.Normal,
                (o, e) => RefreshTimeText());
            clockTimer.Start();
        }

        public void DoExpand()
        {
            m_controlPanelRegionController.Expand();
            RefreshTimeText();
            
            RaisePropertyChanged(nameof(CurrentTemperatureString));
            RaisePropertyChanged(nameof(IsExpanded));
        }

        public void DoMinimize()
        {
            m_controlPanelRegionController.Minimize();
            RefreshTimeText();
            
            RaisePropertyChanged(nameof(CurrentTemperatureString));
            RaisePropertyChanged(nameof(IsExpanded));
        }

        public void NavigateToFrontPage()
        {
            Navigator.DoNavigateTo<FrontPageView>();
        }

        public void NavigateToResultRegister()
        {
            var args = new ResultRegisterArgs
            {
                CompetitionYears = 2019.ToMany().ToHashSet()
            };
            
            Navigator.DoNavigateTo<ResultRegisterView>(args);
        }

        public void NavigateToCompetitionGeneral()
        {
            var args = new CompetitionGeneralArgs
            {
                CompetitionYear = 2019,
                CompetitionClass = FinlandiaHiihtoCompetitionClass.Create(
                    FinlandiaSkiingDistance.Fifty,
                    FinlandiaSkiingStyle.Classic)
            };
            Navigator.DoNavigateTo<CompetitionGeneralView>(args);
        }

        public void NavigateToAthleteCard()
        {
            Navigator.DoNavigateTo<AthleteCardView>();
        }

        public void NavigateToTeamCard()
        {
            Navigator.DoNavigateTo<TeamCardView>();
        }

        private void OnWeatherTimerTick()
        {
            m_updateWeatherSub?.Dispose();
            m_updateWeatherSub = GetWeatherCommand.Execute().Subscribe();
        }

        private void RefreshTimeText()
        {
            CurrentTime = DateTime.Now.ToString(IsExpanded ? c_longTimeFormatString : c_shortTimeFormatString);
        }

        private async Task GetCurrentWeather()
        {
            var weatherData = await m_weatherService.GetCurrentWeatherAsync(c_hardCodedLocation);
            m_currentTemperature = weatherData.AirTemperature.HasValue
                ? $"{weatherData.AirTemperature.Value.ToString("0.0", CultureInfo.InvariantCulture)} °C"
                : string.Empty;
            
            RaisePropertyChanged(nameof(CurrentTemperatureString));
        }
    }
}
