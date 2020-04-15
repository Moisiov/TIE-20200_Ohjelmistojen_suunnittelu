using System;
using FJ.Client.Athlete;
using FJ.Client.CompetitionGeneral;
using FJ.Client.Core;
using FJ.Client.Core.Common;
using FJ.Client.Core.Services;
using FJ.Client.FrontPage;
using FJ.Client.ResultRegister;
using FJ.Client.Team;

namespace FJ.Client.ControlPanel
{
    public class ControlPanelViewModel : ViewModelBase
    {
        private readonly IControlPanelRegionController m_controlPanelRegionController;

        public bool IsExpanded => m_controlPanelRegionController.CurrentControlPanelSizeOption == ControlPanelSizeOption.Expanded;

        private ControlPanelModel m_model;

        public ControlPanelViewModel(IControlPanelRegionController controPanelRegionController)
        {
            m_controlPanelRegionController = controPanelRegionController;

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
    }
}
