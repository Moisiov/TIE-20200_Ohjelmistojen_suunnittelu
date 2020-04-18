using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FJ.Client.Core
{
    public class ListItemLimitWrapper<T> : FJNotificationObject, IEnumerable<T>
    {
        private int m_batchesVisible;

        private ObservableCollection<T> m_data = new ObservableCollection<T>();

        public ObservableCollection<T> Data
        {
            get => m_data;
            private set => SetAndRaise(ref m_data, value ?? new ObservableCollection<T>());
        }
        
        public List<T> TList { get; private set; }
        public int BatchSize { get; set; }
        public bool MoreDataLeft => (TList?.Count ?? 0) > (Data?.Count ?? 0);

        public ListItemLimitWrapper()
            : this(null)
        {
        }
        
        public ListItemLimitWrapper(IEnumerable<T> listItems, int batchSize = 20)
        {
            TList = listItems?.ToList() ?? new List<T>();
            BatchSize = batchSize;
            m_batchesVisible = 1;
            Data = new ObservableCollection<T>(TList.Take(BatchSize));

            RaisePropertiesChanged();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            TList.Clear();
            Data.Clear();
            
            RaisePropertiesChanged();
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            // TODO might cause problem
            return TList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void GetMoreData()
        {
            m_batchesVisible++;
            Data = new ObservableCollection<T>(TList.Take(m_batchesVisible * BatchSize));
            
            RaisePropertiesChanged();
        }
    }

    public static class ListItemWrapperExtensions
    {
        public static ListItemLimitWrapper<T> ToListItemWrapper<T>(this IEnumerable<T> source)
        {
            return new ListItemLimitWrapper<T>(source);
        }
    }
}
