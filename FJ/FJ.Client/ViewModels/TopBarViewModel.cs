using System;
using Prism.Events;
using FJ.Client.Events;
using FJ.Client.Services;
using FJ.Client.Models;

namespace FJ.Client.ViewModels
{
    public class TopBarViewModel : ViewModelBase
    {
        private readonly IContentRegionNavigator m_contentRegionJournalNavigator;

        private TopBarModel m_model;

        public bool CanNavigateContentBack { get; set; }
        public bool CanNavigateContentForward { get; set; }

        public TopBarViewModel(IEventAggregator ea, IContentRegionNavigator contentRegionJournalNavigator)
        {
            m_contentRegionJournalNavigator = contentRegionJournalNavigator;

            m_model = new TopBarModel();

            ea.GetEvent<ContentRegionNavigationEvent>().Subscribe((e) => UpdateNavigationButtons());
            UpdateNavigationButtons();
        }

        public void DoNavigateContentBack()
        {
            m_contentRegionJournalNavigator.DoNavigateBack();
        }

        public void DoNavigateContentForward()
        {
            m_contentRegionJournalNavigator.DoNavigateForward();
        }

        public void DoRequestContentRefresh()
        {
            m_contentRegionJournalNavigator.RequestRefresh();
        }

        private void UpdateNavigationButtons()
        {
            CanNavigateContentBack = m_contentRegionJournalNavigator.CanNavigateBack;
            CanNavigateContentForward = m_contentRegionJournalNavigator.CanNavigateForward;
            RaisePropertiesChanged();
        }
    }
}
