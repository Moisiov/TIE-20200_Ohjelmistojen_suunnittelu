using System;
using Prism.Regions;
using ReactiveUI;

namespace FJ.Client.ViewModels
{
    public class ViewModelBase : ReactiveObject, IRegionMemberLifetime, IJournalAware, INavigationAware
    {
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

        public virtual void DoRefresh()
        {
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            ((IReactiveObject)this).RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertiesChanged()
        {
            RaisePropertyChanged(null);
        }
    }
}
