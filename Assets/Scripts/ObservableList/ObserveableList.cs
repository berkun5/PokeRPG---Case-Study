using System;
using System.Collections.Generic;

namespace ObservableList
{
    public class ObservableList<T> : List<T>
    {
        public event Action<T,int> ItemAdded;
        public event Action<T,int> ItemRemoved;

        public new void Add(T item)
        {
            base.Add(item);
            ItemAdded?.Invoke(item, Count);
        }

        public new void Remove(T item)
        {
            base.Remove(item);
            ItemRemoved?.Invoke(item, Count);
        }
    }
}