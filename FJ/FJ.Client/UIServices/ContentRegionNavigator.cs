using System;
using Avalonia.Controls;
using Prism.Events;
using Prism.Regions;
using FJ.Client.UIEvents;
using FJ.Client.UIUtils;
using System.Linq;
using FJ.Utils;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace FJ.Client.UIServices
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

        public void DoNavigateTo(string targetViewName, object navArgs)
        {
            m_regionManager.NavigateContentTo(targetViewName, navArgs);
        }

        public void DoNavigateTo<TView>(object navArgs)
            where TView : UserControl
        {
            DoNavigateTo(typeof(TView).Name, navArgs);
        }

        protected virtual void OnContentRegionNavigation(object s, RegionNavigationEventArgs e)
        {
            m_eventAggregator.GetEvent<ContentRegionNavigationEvent>().Publish(e);
        }

        private IRegionNavigationJournal GetContentRegionNavigationJournal()
        {
            return m_regionManager.Regions[Regions.ContentRegion].NavigationService.Journal;
        }

        public void SetLoadingScreen(bool doShowLoadingScreen)
        {
            m_eventAggregator.GetEvent<ContentRegionLoadingScreenEvent>().Publish(doShowLoadingScreen);
        }

        public IDisposableLoadingScreen ShowLoadingScreen()
        {
            return new DisposableLoadingScreen(SetLoadingScreen);
        }
    }
}
