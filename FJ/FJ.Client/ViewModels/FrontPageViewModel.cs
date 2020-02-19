using System;
using FJ.Client.Models;
using FJ.Client.UIServices;
using FJ.Client.Views;

namespace FJ.Client.ViewModels
{
    public class FrontPageViewModel : ViewModelBase
    {
        private readonly IContentRegionNavigator m_navigator;
        private FrontPageModel m_model;

        public FrontPageViewModel(IContentRegionNavigator navigator)
        {
            m_navigator = navigator;
            m_model = new FrontPageModel();
        }

        public void NavigateToResultRegister()
        {
            m_navigator.DoNavigateTo<ResultRegisterView>();
        }

        public void NavigateToCompetitionGeneral()
        {
            m_navigator.DoNavigateTo<CompetitionGeneralView>();
        }

        public void NavigateToAthleteCard()
        {
            m_navigator.DoNavigateTo<AthleteCardView>();
        }

        public void NavigateToTeamCard()
        {
            m_navigator.DoNavigateTo<TeamCardView>();
        }
    }
}
