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
using IlmatieteenLaitosAPI;
using ReactiveUI;

namespace FJ.Client.ControlPanel
{
    public class ControlPanelViewModel : ViewModelBase
    {
        private readonly IControlPanelRegionController m_controlPanelRegionController;
        private readonly IIlmatieteenLaitosAPI m_ilmatieteenLaitosAPI;

        public bool IsExpanded => m_controlPanelRegionController.CurrentControlPanelSizeOption == ControlPanelSizeOption.Expanded;

        public ReactiveCommand<Unit, Unit> GetWeatherCommand { get; }
        public string CurrentTime { get; set; }
        public string CurrentTemperature { get; set; }
        public string CurrentWindSpeed { get; set; }

        private readonly ControlPanelModel m_model;

        public ControlPanelViewModel(IControlPanelRegionController controPanelRegionController)
        {
            m_controlPanelRegionController = controPanelRegionController;

            m_ilmatieteenLaitosAPI = new IlmatieteenLaitosAPI.IlmatieteenLaitosAPI();
            GetWeatherCommand = ReactiveCommand.CreateFromTask(GetCurrentWeather);
            GetWeatherCommand.Execute().Subscribe();
            DispatcherTimer weatherTimer = new DispatcherTimer(TimeSpan.FromMinutes(10), DispatcherPriority.Normal, OnWeatherTimerTick);
            weatherTimer.Start();

            RefreshTimeText();
            DispatcherTimer clockTimer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, OnClockTimerTick);
            clockTimer.Start();

            m_model = new ControlPanelModel();
        }

        public void DoExpand()
        {
            m_controlPanelRegionController.Expand();
            RaisePropertyChanged(nameof(IsExpanded));
        }

        public void DoMinimize()
        {
            m_controlPanelRegionController.Minimize();
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

        private void OnWeatherTimerTick(object sender, EventArgs e)
        {
            GetWeatherCommand.Execute().Subscribe();
        }

        private void OnClockTimerTick(object sender, EventArgs e)
        {
            RefreshTimeText();
        }

        private void RefreshTimeText()
        {
            CurrentTime = DateTime.Now.ToString("HH':'mm");
            RaisePropertyChanged(nameof(CurrentTime));
        }

        private async Task GetCurrentWeather()
        {
            var weatherData = await m_ilmatieteenLaitosAPI.GetWeatherNow("Lahti");
            CurrentTemperature = weatherData.AirTemperature.HasValue ? weatherData.AirTemperature.Value.ToString("0.0", CultureInfo.InvariantCulture) + " °C" : "";
            RaisePropertyChanged(nameof(CurrentTemperature));
        }
    }
}
