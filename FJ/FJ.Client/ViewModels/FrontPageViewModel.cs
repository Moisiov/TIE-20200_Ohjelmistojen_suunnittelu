using System;
using FJ.Client.Models;
using FJ.Client.UICore;
using FJ.Client.UIServices;
using FJ.Client.Views;

namespace FJ.Client.ViewModels
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
    }
}
