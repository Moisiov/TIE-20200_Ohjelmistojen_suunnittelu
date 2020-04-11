using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FJ.DomainObjects.Filters.Core
{
    public class FilterCollection : IEnumerable<FilterBase>
    {
        private readonly List<FilterBase> m_filters;

        public int Count => m_filters.Count;

        public FilterCollection()
        {
            m_filters = new List<FilterBase>();
        }

        public FilterCollection(FilterBase filter)
            : this()
        {
            Add(filter);
        }

        public FilterCollection(IEnumerable<FilterBase> filters)
            : this()
        {
            Add(filters);
        }

        public FilterCollection(params FilterBase[] filters)
            : this((IEnumerable<FilterBase>)filters)
        {
        }

        public void Clear()
        {
            m_filters.Clear();
        }

        public void Add(FilterBase filter)
        {
            if (filter == null)
            {
                return;
            }
            
            m_filters.Add(filter);
        }

        public void Add(IEnumerable<FilterBase> filters)
        {
            foreach (var filter in filters)
            {
                Add(filter);
            }
        }

        public void AddOrReplace(FilterBase filter)
        {
            if (filter == null)
            {
                return;
            }

            m_filters.RemoveAll(f => f.GetType() == filter.GetType());
            m_filters.Add(filter);
        }

        public TFilter ExtractFilter<TFilter>(Func<TFilter, bool> predicate = null)
            where TFilter : class
        {
            for (var i = 0; i < m_filters.Count; i++)
            {
                if (m_filters[i] is TFilter filter && predicate?.Invoke(filter) != false)
                {
                    m_filters.RemoveAt(i);
                    return filter;
                }
            }

            return null;
        }

        public bool ContainsFilter<TFilter>(Func<TFilter, bool> predicate = null)
            where TFilter : class
        {
            var filter = m_filters.OfType<TFilter>().FirstOrDefault();
            return filter != null && predicate?.Invoke(filter) != false;
        }

        public IEnumerator<FilterBase> GetEnumerator()
        {
            return m_filters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public FilterCollection Clone()
        {
            return new FilterCollection(m_filters);
        }

        public TFilter Get<TFilter>() => m_filters.OfType<TFilter>().FirstOrDefault();
    }
}
