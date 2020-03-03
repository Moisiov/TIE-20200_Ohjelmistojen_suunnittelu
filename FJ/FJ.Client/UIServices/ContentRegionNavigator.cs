﻿using System;
using Avalonia.Controls;
using Prism.Events;
using Prism.Regions;
using FJ.Client.UIEvents;
using FJ.Client.UIUtils;
using System.Linq;
using FJ.Utils;

namespace FJ.Client.UIServices
{
    public class ContentRegionNavigator : IContentRegionNavigator
    {
        private readonly IEventAggregator m_eventAggregator;
        private readonly IRegionManager m_regionManager;

        public bool CanNavigateBack => GetContentRegionNavigationJournal().CanGoBack;
        public bool CanNavigateForward => GetContentRegionNavigationJournal().CanGoForward;

        public ContentRegionNavigator(IEventAggregator ea, IRegionManager regionManager)
        {
            m_eventAggregator = ea;
            m_regionManager = regionManager;
            m_regionManager.Regions[Regions.ContentRegion].NavigationService.Navigated += OnContentRegionNavigation;
        }

        public string GetCurrentViewName()
        {
            return m_regionManager.Regions[Regions.ContentRegion].ActiveViews.FirstOrDefault().GetType().Name;
        }

        public string GetCurrentViewModelName()
        {
            return GetCurrentViewName().ReplaceLastOccurrence("View", "ViewModel");
        }

        public void DoNavigateBack()
        {
            GetContentRegionNavigationJournal().GoBack();
        }

        public void DoNavigateForward()
        {
            GetContentRegionNavigationJournal().GoForward();
        }

        public void RequestRefresh()
        {
            var eventArgs = new ContentRegionRefreshRequestedEventArgs
            {
                TargetViewName = GetCurrentViewName(),
                TargetViewModelName = GetCurrentViewModelName()
            };

            m_eventAggregator.GetEvent<ContentRegionRefreshRequestedEvent>().Publish(eventArgs);
        }

        public void DoNavigateTo(string targetViewName)
        {
            m_regionManager.NavigateContentTo(targetViewName);
        }

        public void DoNavigateTo<TView>()
            where TView : UserControl
        {
            DoNavigateTo(typeof(TView).Name);
        }

        protected virtual void OnContentRegionNavigation(object s, RegionNavigationEventArgs e)
        {
            m_eventAggregator.GetEvent<ContentRegionNavigationEvent>().Publish(e);
        }

        private IRegionNavigationJournal GetContentRegionNavigationJournal()
        {
            return m_regionManager.Regions[Regions.ContentRegion].NavigationService.Journal;
        }
    }
}
