using System;
using System.Globalization;
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
using FJ.ServiceInterfaces.Weather;
using ReactiveUI;

namespace FJ.Client.ControlPanel
{
    public class ControlPanelViewModel : ViewModelBase
    {
        private const string c_shortTimeFormatString = "HH':'mm";
        private const string c_longTimeFormatString = "HH':'mm':'ss";
        
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
        public string CurrentTemperature
        {
            get => m_currentTemperature;
            set => SetAndRaise(ref m_currentTemperature, value);
        }
        
        public string CurrentWindSpeed { get; set; }

        private IDisposable m_updateWeatherSub;
        private ControlPanelModel m_model;

        private string m_timeFormatString = c_longTimeFormatString;

        public ControlPanelViewModel(IControlPanelRegionController controlPanelRegionController, IWeatherService weatherService)
        {
            m_controlPanelRegionController = controlPanelRegionController;
            m_weatherService = weatherService;

            m_model = new ControlPanelModel();
            
            GetWeatherCommand = ReactiveCommand.CreateFromTask(GetCurrentWeather);
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
            m_timeFormatString = c_longTimeFormatString;
            RefreshTimeText();
            
            RaisePropertyChanged(nameof(IsExpanded));
        }

        public void DoMinimize()
        {
            m_controlPanelRegionController.Minimize();
            m_timeFormatString = c_shortTimeFormatString;
            RefreshTimeText();
            
            RaisePropertyChanged(nameof(IsExpanded));
        }

        public void NavigateToFrontPage()
        {
            Navigator.DoNavigateTo<FrontPageView>();
        }

        public void NavigateToResultRegister()
        {
            Navigator.DoNavigateTo<ResultRegisterView>();
        }

        public void NavigateToCompetitionGeneral()
        {
            Navigator.DoNavigateTo<CompetitionGeneralView>();
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
            CurrentTime = DateTime.Now.ToString(m_timeFormatString);
        }

        private async Task GetCurrentWeather()
        {
            var weatherData = await m_weatherService.GetCurrentWeatherAsync("Lahti");
            CurrentTemperature = weatherData.AirTemperature.HasValue
                ? weatherData.AirTemperature.Value.ToString("0.0", CultureInfo.InvariantCulture) + " °C"
                : string.Empty;
        }
    }
}
