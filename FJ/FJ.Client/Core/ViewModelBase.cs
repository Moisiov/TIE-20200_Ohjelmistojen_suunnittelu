using System;
using System.Threading.Tasks;
using FJ.Client.Core.Events;
using FJ.Client.Core.Services;
using FJ.Utils;
using Prism.Events;
using Prism.Regions;

namespace FJ.Client.Core
{
    public abstract class ViewModelBase : ViewModelBase<EmptyNavigationArgs>
    {
    }

    public abstract class ViewModelBase<TArgument> : FJNotificationObject, IRegionMemberLifetime, IJournalAware, INavigationAware
        where TArgument : NavigationArgsBase<TArgument>, new()
    {
        protected IEventAggregator EventAggregator { get; private set; }
        protected IContentRegionNavigator Navigator { get; private set; }

        [Unity.InjectionMethod]
        public void Initialize(IEventAggregator ea, IContentRegionNavigator navigator)
        {
            EventAggregator = ea;
            Navigator = navigator;

            EventAggregator.GetEvent<ContentRegionRefreshRequestedEvent>()
                .Subscribe(async e => await DoRefreshAsync(e));

            EventAggregator.GetEvent<ContentRegionNavigationEvent>()
                .Subscribe(async e => await OnContentRegionNavigation(e));
        }

        public TArgument Argument { get; set; }

        #region IRegionMemberLifetime
        public virtual bool KeepAlive => true;
        #endregion

        #region IJournalAware
        public virtual bool PersistInHistory()
        {
            return true;
        }
        #endregion

        #region INavigationAware
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var navParams = navigationContext.Parameters;
            if (navParams != null && navParams.ContainsKey(typeof(TArgument).Name))
            {
                Argument = (TArgument)navParams[typeof(TArgument).Name];
            }
            else
            {
                Argument = new TArgument();
            }
        }
        #endregion

        private async Task OnContentRegionNavigation(ContentRegionNavigationEventArgs e)
        {
            if (e.TargetViewModelName != GetType().Name
                || e.NavigationMode.IsIn(
                    IContentRegionNavigator.NavigationMode.Back,
                    IContentRegionNavigator.NavigationMode.Forward))
            {
                await Task.CompletedTask;
                return;
            }

            await DoPopulateAsync();
        }

        /// <summary>
        /// Override this method if you need to use Argument on async populating
        /// </summary>
        /// <returns>Task</returns>
        protected virtual async Task DoPopulateAsync()
        {
            await Task.CompletedTask;
        }

        private async Task DoRefreshAsync(ContentRegionRefreshRequestedEventArgs eventArgs)
        {
            if (eventArgs.TargetViewModelName != GetType().Name)
            {
                await Task.CompletedTask;
                return;
            }

            await DoRefreshInternalAsync();
        }

        protected virtual async Task DoRefreshInternalAsync()
        {
            await Task.CompletedTask;
        }
    }
}
