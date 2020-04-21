using System;
using System.Linq;
using Avalonia.Controls;
using FJ.Client.Core.Common;
using FJ.Client.Core.Events;
using FJ.Utils;
using Prism.Events;
using Prism.Regions;

namespace FJ.Client.Core.Services
{
    public class DisposableLoadingScreen : IDisposableLoadingScreen
    {
        private bool m_disposed;

        private readonly Action<bool> m_setLoadingScreenAction;

        public DisposableLoadingScreen(Action<bool> setLoadingScreenAction)
        {
            m_setLoadingScreenAction = setLoadingScreenAction;
            m_setLoadingScreenAction.Invoke(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_disposed)
            {
                return;
            }

            if (disposing)
            {
                m_setLoadingScreenAction(false);
            }

            m_disposed = true;
        }
    }

    public class ContentRegionNavigator : IContentRegionNavigator
    {
        private readonly IEventAggregator m_eventAggregator;
        private readonly IRegionManager m_regionManager;

        private IContentRegionNavigator.NavigationMode
            m_navigationMode = IContentRegionNavigator.NavigationMode.Unknown;
        
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
            return m_regionManager.Regions[Regions.ContentRegion].ActiveViews.FirstOrDefault()?.GetType().Name;
        }

        public string GetCurrentViewModelName()
        {
            return GetCurrentViewName().ReplaceLastOccurrence("View", "ViewModel");
        }

        public void DoNavigateBack()
        {
            m_navigationMode = IContentRegionNavigator.NavigationMode.Back;
            GetContentRegionNavigationJournal().GoBack();
        }

        public void DoNavigateForward()
        {
            m_navigationMode = IContentRegionNavigator.NavigationMode.Forward;
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

        public void DoNavigateTo(string targetViewName, object navArgs)
        {
            // Don't allow navigation to view that is currently showing
            if (targetViewName == GetCurrentViewName())
            {
                return;
            }
            
            m_navigationMode = IContentRegionNavigator.NavigationMode.New;
            m_regionManager.NavigateContentTo(targetViewName, navArgs);
        }

        public void DoNavigateTo<TView>(object navArgs)
            where TView : UserControl
        {
            DoNavigateTo(typeof(TView).Name, navArgs);
        }

        public void DoClearNavigationStack()
        {
            GetContentRegionNavigationJournal().Clear();
            m_eventAggregator.GetEvent<ContentRegionNavigationStackClearedEvent>().Publish();
        }
        
        public void ShowErrorMessage(string msg)
        {
            m_eventAggregator.GetEvent<ContentRegionErrorEvent>().Publish(msg);
        }

        public void SetLoadingScreen(bool doShowLoadingScreen)
        {
            m_eventAggregator.GetEvent<ContentRegionLoadingScreenEvent>().Publish(doShowLoadingScreen);
        }

        public IDisposableLoadingScreen ShowLoadingScreen()
        {
            return new DisposableLoadingScreen(SetLoadingScreen);
        }

        protected virtual void OnContentRegionNavigation(object s, RegionNavigationEventArgs e)
        {
            var eventArgs = new ContentRegionNavigationEventArgs
            {
                EventArgs = e,
                TargetViewName = GetCurrentViewName(),
                TargetViewModelName = GetCurrentViewModelName(),
                NavigationMode = m_navigationMode
            };
                
            m_eventAggregator.GetEvent<ContentRegionNavigationEvent>().Publish(eventArgs);
            m_navigationMode = IContentRegionNavigator.NavigationMode.Unknown;
        }

        private IRegionNavigationJournal GetContentRegionNavigationJournal()
        {
            return m_regionManager.Regions[Regions.ContentRegion].NavigationService.Journal;
        }
    }
}
