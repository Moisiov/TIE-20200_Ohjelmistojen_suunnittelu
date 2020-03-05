using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FJ.Client.UIEvents;
using FJ.Client.UIServices;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;

namespace FJ.Client.ViewModels
{
    public class ViewModelBase : ReactiveObject, IRegionMemberLifetime, IJournalAware, INavigationAware
    {
        protected IEventAggregator EventAggregator { get; private set; }
        protected IContentRegionNavigator Navigator { get; private set; }

        [Unity.InjectionMethod]
        public void Initialize(IEventAggregator ea, IContentRegionNavigator navigator)
        {
            EventAggregator = ea;
            Navigator = navigator;

            EventAggregator.GetEvent<ContentRegionRefreshRequestedEvent>().Subscribe(DoRefresh);
        }

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

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
        #endregion

        public void DoRefresh(ContentRegionRefreshRequestedEventArgs eventArgs)
        {
            if (eventArgs.TargetViewModelName != GetType().Name)
            {
                return;
            }

            DoRefreshInternal();
        }

        protected virtual void DoRefreshInternal()
        {
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
