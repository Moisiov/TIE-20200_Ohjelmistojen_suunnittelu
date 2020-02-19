using System;
using Prism.Events;
using FJ.Client.UIEvents;
using FJ.Client.UIServices;
using FJ.Client.Models;

namespace FJ.Client.ViewModels
{
    public class TopBarViewModel : ViewModelBase
    {
        private readonly IContentRegionNavigator m_contentRegionNavigator;

        private TopBarModel m_model;

        public bool CanNavigateContentBack { get; set; }
        public bool CanNavigateContentForward { get; set; }

        public TopBarViewModel(IEventAggregator ea, IContentRegionNavigator contentRegionNavigator)
        {
            m_contentRegionNavigator = contentRegionNavigator;

            m_model = new TopBarModel();

            ea.GetEvent<ContentRegionNavigationEvent>().Subscribe((e) => UpdateNavigationButtons());
            UpdateNavigationButtons();
        }

        public void DoNavigateContentBack()
        {
            m_contentRegionNavigator.DoNavigateBack();
        }

        public void DoNavigateContentForward()
        {
            m_contentRegionNavigator.DoNavigateForward();
        }

        public void DoRequestContentRefresh()
        {
            m_contentRegionNavigator.RequestRefresh();
        }

        private void UpdateNavigationButtons()
        {
            CanNavigateContentBack = m_contentRegionNavigator.CanNavigateBack;
            CanNavigateContentForward = m_contentRegionNavigator.CanNavigateForward;
            RaisePropertiesChanged();
        }
    }
}
