using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FJ.DomainObjects.Filters.Core;
using FJ.Utils;

namespace FJ.Client.Core.UIElements.Filters
{
    public abstract class RegisterFilterModelBase : FJNotificationObject
    {
        public delegate void FilterChangedHandler();
        public event FilterChangedHandler FilterChanged;
        
        public Type ValueType { get; protected set; }
        
        private object m_baseValue;
        public object BaseValue
        {
            get => m_baseValue;
            set
            {
                SetAndRaise(ref m_baseValue, value);
                RaiseFilterChanged();
            }
        }

        private bool m_filterDisabled;
        public bool FilterDisabled
        {
            get => m_filterDisabled;
            set
            {
                if (m_filterDisabled == value)
                {
                    return;
                }

                SetAndRaise(ref m_filterDisabled, value);
                RaiseFilterChanged();
            }
        }

        public IEnumerable<FilterBase> GetActiveFilters()
        {
            var filters = DoGetActiveFilters();
            var singleFilter = DoGetActiveFilter();
            if (singleFilter != null)
            {
                filters = filters.Concat(singleFilter.ToMany());
            }

            return filters;
        }
        
        public abstract void ClearFilters();

        protected virtual IEnumerable<FilterBase> DoGetActiveFilters()
        {
            return Enumerable.Empty<FilterBase>();
        }

        protected virtual FilterBase DoGetActiveFilter()
        {
            return null;
        }

        protected void RaiseFilterChanged()
        {
            FilterChanged?.Invoke();
        }
    }
    
    public abstract class RegisterFilterModelBase<T> : RegisterFilterModelBase
        where T : class
    {
        public T Value
        {
            get => BaseValue as T;
            set => BaseValue = value;
        }

        protected RegisterFilterModelBase()
        {
            ValueType = typeof(T);
        }
    }

    public abstract class RegisterIEnumerableFilterBase<TItem> : RegisterFilterModelBase
    {
        public IEnumerable<TItem> Value
        {
            get => ((IList)BaseValue)?.Cast<TItem>();
            set => BaseValue = value;
        }

        protected RegisterIEnumerableFilterBase()
        {
            ValueType = typeof(TItem);
        }
    }

    public abstract class RegisterEnumFilterModelBase<TEnum> : RegisterFilterModelBase
        where TEnum : struct, IConvertible
    {
        public TEnum? Value
        {
            get => BaseValue != null && Enum.TryParse<TEnum>(BaseValue.ToString(), out var result)
                ? result
                : (TEnum?)null;
            set => BaseValue = value.ToString();
        }

        protected RegisterEnumFilterModelBase()
        {
            ValueType = typeof(TEnum);
        }
    }
}
