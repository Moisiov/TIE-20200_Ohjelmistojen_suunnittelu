using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;
using FJ.Client.Core.UIElements.Filters;
using FJ.DomainObjects.Filters.Core;
using ReactiveUI;

namespace FJ.Client.Core.Register
{
    public abstract class RegisterModelBase : FJNotificationObject
    {
        private readonly List<RegisterFilterModelBase> m_filterModels;

        private bool m_enableSearch;
        public bool EnableSearch
        {
            get => m_enableSearch;
            set => SetAndRaise(ref m_enableSearch, value);
        }

        protected RegisterModelBase()
        {
            m_filterModels = new List<RegisterFilterModelBase>();
            SetFilterModels();
            EnableSearch = true;
        }

        public virtual Task OnActivatingAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DoExecuteSearchAsync()
        {
            EnableSearch = false;
            
            var activeFilters = new FilterCollection();
            foreach (var filterModel in m_filterModels)
            {
                activeFilters.Add(filterModel.GetActiveFilters().Where(f => f != null));
            }
            await DoExecuteSearchInternalAsync(activeFilters);
        }

        public void DoClearFilters()
        {
            m_filterModels.ForEach(f => f.ClearFilters());
            
            OnFilterChanged();
            AfterFiltersCleared();
            
            RaisePropertiesChanged();
        }

        public virtual void DoClearItems()
        {
        }

        protected abstract Task DoExecuteSearchInternalAsync(FilterCollection activeFilters);

        protected virtual void AfterFiltersCleared()
        {
        }

        protected void SetFilterModels()
        {
            // Make sure there won't be duplicate filters
            m_filterModels.ForEach(f => f.FilterChanged -= OnFilterChanged);
            m_filterModels.Clear();

            var filterProps = GetType()
                .GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(RegisterFilterModelBase)
                            || p.PropertyType.IsSubclassOf(typeof(RegisterFilterModelBase)));
            
            foreach (var filterModelProperty in filterProps)
            {
                var filterModel = filterModelProperty.GetValue(this) as RegisterFilterModelBase;
                if (filterModel == null)
                {
                    filterModel = Activator.CreateInstance(filterModelProperty.PropertyType) as RegisterFilterModelBase;
                    if (filterModel == null)
                    {
                        throw new InvalidOperationException($"Invalid register filter model: {filterModelProperty.Name}");
                    }
                    
                    filterModel.FilterChanged += OnFilterChanged;
                }
                
                m_filterModels.Add(filterModel);
                filterModelProperty.SetValue(this, filterModel);
            }
        }

        private void OnFilterChanged()
        {
            EnableSearch = true;
        }
    }
    
    public abstract class RegisterModelBase<TRegisterItem> : RegisterModelBase
        where TRegisterItem : RegisterItemModelBase
    {
        public ListItemLimitWrapper<TRegisterItem> AllItems { get; protected set; }
            = new ListItemLimitWrapper<TRegisterItem>();

        public override void DoClearItems()
        {
            AllItems.Clear();
        }
    }
}
