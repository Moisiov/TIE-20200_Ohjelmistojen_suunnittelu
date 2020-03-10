using System;
using Prism.Events;
using FJ.Client.UIEvents;
using FJ.Client.UIServices;
using FJ.Client.Models;
using FJ.Client.UICore;

namespace FJ.Client.ViewModels
{
    public class TopBarViewModel : ViewModelBase
    {
        private TopBarModel m_model;

        private bool m_canNavigateContentBack;
        public bool CanNavigateContentBack
        {
            get => m_canNavigateContentBack;
            set => SetAndRaise(ref m_canNavigateContentBack, value);
        }

        private bool m_canNavigateContentForward;
        public bool CanNavigateContentForward
        {
            get => m_canNavigateContentForward;
            set => SetAndRaise(ref m_canNavigateContentForward, value);
        }

        private string m_topBarHeaderText;
        public string TopBarHeaderText
        {
            get => m_topBarHeaderText;
            set => SetAndRaise(ref m_topBarHeaderText, value);
        }

        public TopBarViewModel(IEventAggregator ea)
        {
            m_model = new TopBarModel();

            ea.GetEvent<ContentRegionNavigationEvent>().Subscribe((e) => UpdateNavigationButtons());

            TopBarHeaderText = "TESTITEKSTI, JOKA ON KOVIN PITKÄ LUONTEELTAAN";
        }

        public void DoNavigateContentBack()
        {
            Navigator.DoNavigateBack();
        }

        public void DoNavigateContentForward()
        {
            Navigator.DoNavigateForward();
        }

        public void DoRequestContentRefresh()
        {
            Navigator.RequestRefresh();
        }

        private void UpdateNavigationButtons()
        {
            CanNavigateContentBack = Navigator.CanNavigateBack;
            CanNavigateContentForward = Navigator.CanNavigateForward;
        }
    }
}
