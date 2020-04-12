using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FJ.Client.Core.Events;
using FJ.Client.Core.Services;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;

namespace FJ.Client.Core
{
    public abstract class ViewModelBase : ViewModelBase<EmptyNavigationArgs>
    {
    }

    public abstract class ViewModelBase<TArgument> : ReactiveObject, IRegionMemberLifetime, IJournalAware, INavigationAware
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
                .Subscribe(async e => await DoPopulateAsync());
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

            DoPopulate();
        }
        #endregion

        /// <summary>
        /// Override this method if you need to use Argument on populating
        /// </summary>
        public virtual void DoPopulate()
        {
        }

        /// <summary>
        /// Override this method if you need to use Argument on async populating
        /// </summary>
        /// <returns>Task</returns>
        public virtual async Task DoPopulateAsync()
        {
            await Task.CompletedTask;
        }

        public async Task DoRefreshAsync(ContentRegionRefreshRequestedEventArgs eventArgs)
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

        protected bool SetAndRaise<T>(ref T field, T value, [CallerMemberName] string caller = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            RaisePropertyChanged(caller);

            return true;
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            ((IReactiveObject)this).RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            RaisePropertyChanged(expression.Member.Name);
        }

        protected void RaisePropertiesChanged()
        {
            RaisePropertyChanged(null);
        }
    }
}
