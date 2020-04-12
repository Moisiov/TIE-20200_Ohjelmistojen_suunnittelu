using System;
using FJ.Client.Athlete;
using FJ.Client.CompetitionOccasion;
using FJ.Client.Core;
using FJ.Client.ResultRegister;
using FJ.Client.Team;

namespace FJ.Client.FrontPage
{
    public class FrontPageViewModel : ViewModelBase
    {
        private FrontPageModel m_model;

        public FrontPageViewModel()
        {
            m_model = new FrontPageModel();
        }

        public void NavigateToResultRegister()
        {
            var args = new ResultRegisterArgs
            {
                CompetitionYears = new[] { 2019 },
                HomeCities = new[] { "Tampere" }
            };
            Navigator.DoNavigateTo<ResultRegisterView>(args);
        }

        public void NavigateToCompetitionOccasion()
        {
            Navigator.DoNavigateTo<CompetitionOccasionView>();
        }

        public void NavigateToAthleteCard()
        {
            Navigator.DoNavigateTo<AthleteCardView>();
        }

        public void NavigateToTeamCard()
        {
            Navigator.DoNavigateTo<TeamCardView>();
        }
    }
}
