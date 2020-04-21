using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.Client.ResultRegister;
using FJ.DomainObjects.FinlandiaHiihto;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using FJ.ServiceInterfaces.Weather;
using FJ.Utils;
using PlotterService;

namespace FJ.Client.CompetitionOccasion
{
    public class CompetitionOccasionViewModel : ViewModelBase<CompetitionOccasionArgs>
    {
        private const string c_hardCodedLocation = "Lahti";

        private readonly IPlotService m_plotService;
        private readonly IWeatherService m_weatherService;
        
        private CompetitionOccasionModel m_model;

        public ObservableCollection<CompetitionRowItemModel> CompetitionList { get; set; }

        public string HeaderText => $"Vuoden {OccasionYear.ToString()} kilpailutapahtuman yhteenveto";
        
        private int? m_occasionYear;
        public int? OccasionYear
        {
            get => m_occasionYear;
            set
            {
                SetAndRaise(ref m_occasionYear, value);
                RaisePropertyChanged(nameof(HeaderText));
            }
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

        private string m_temperatureText;
        public string TemperatureText
        {
            get => m_temperatureText;
            set => SetAndRaise(ref m_temperatureText, value);
        }

        private bool m_temperatureIsVisible;

        public bool TemperatureIsVisible
        {
            get => m_temperatureIsVisible;
            set => SetAndRaise(ref m_temperatureIsVisible, value);
        }

        private string m_windSpeedText;
        public string WindSpeedText
        {
            get => m_windSpeedText;
            set => SetAndRaise(ref m_windSpeedText, value);
        }

        private bool m_windSpeedIsVisible;
        public bool WindSpeedIsVisible
        {
            get => m_windSpeedIsVisible;
            set => SetAndRaise(ref m_windSpeedIsVisible, value);
        }

        private string m_weatherDescription;
        public string WeatherDescription
        {
            get => m_weatherDescription;
            set => SetAndRaise(ref m_weatherDescription, value);
        }

        private bool m_weatherDescriptionIsVisible;
        public bool WeatherDescriptionIsVisible
        {
            get => m_weatherDescriptionIsVisible;
            set => SetAndRaise(ref m_weatherDescriptionIsVisible, value);
        }

        public CompetitionOccasionViewModel(
            ICompetitionOccasionDataService competitionOccasionDataService, 
            IPlotService plotService,
            IWeatherService weatherService)
        {
            m_model = new CompetitionOccasionModel(competitionOccasionDataService);
            m_plotService = plotService;
            m_weatherService = weatherService;

            TemperatureIsVisible = false;
            WindSpeedIsVisible = false;
            WeatherDescriptionIsVisible = false;
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
                    await GetCompetitionDayWeather(year);
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
            var args = new ResultRegisterArgs();
            if (OccasionYear.HasValue)
            {
                args.CompetitionYears = OccasionYear.Value.ToMany().ToHashSet();
            }
            
            Navigator.DoNavigateTo<ResultRegisterView>(args);
        }
        
        public void NavigateToResultRegisterWithCompetition(CompetitionRowItemModel competitionRowItem)
        {
            var args = new ResultRegisterArgs
            {
                CompetitionYears = competitionRowItem.CompetitionInfo.Year.ToMany().ToHashSet(),
                CompetitionClasses = competitionRowItem.CompetitionClass.ToMany().ToHashSet()
            };
            
            Navigator.DoNavigateTo<ResultRegisterView>(args);
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

            m_plotService.GetPlot(
                data,
                NationalityDistributionChartUseBarChart ? PlotType.BarPlot : PlotType.PiePlot,
                $"Kansalaisuusjakauma {OccasionYear}");
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

        private async Task GetCompetitionDayWeather(int year)
        {
            var competitionStart = new DateTime(year, 02, 27, 8, 0, 0, new CultureInfo("fi-FI").Calendar);
            var competitionEnd = new DateTime(year, 02, 27, 16, 0, 0, new CultureInfo("fi-FI").Calendar);

            var weatherData = await m_weatherService.GetWeatherAvgAsync(c_hardCodedLocation, competitionStart, competitionEnd);
            
            m_temperatureText = weatherData.AirTemperature.HasValue
                ? $"{weatherData.AirTemperature.Value.ToString("0.0", CultureInfo.InvariantCulture)} °C"
                : string.Empty;
            TemperatureIsVisible = !string.IsNullOrWhiteSpace(m_temperatureText);

            m_windSpeedText = weatherData.WindSpeed.HasValue
                ? $"{weatherData.WindSpeed.Value.ToString("0.00", CultureInfo.InvariantCulture)} m/s"
                : string.Empty;
            WindSpeedIsVisible = !string.IsNullOrWhiteSpace(m_windSpeedText);

            m_weatherDescription = weatherData.MostSignificantWeatherDescription;
            WeatherDescriptionIsVisible = !string.IsNullOrWhiteSpace(m_weatherDescription);
        }
    }
}
