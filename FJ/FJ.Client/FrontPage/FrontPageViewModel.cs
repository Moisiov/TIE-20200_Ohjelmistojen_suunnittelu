using System;
﻿using FJ.Client.Athlete;
using FJ.Client.Competition;
using FJ.Client.ResultRegister;
using FJ.Client.Team;
using FJ.Client.UICore;

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
