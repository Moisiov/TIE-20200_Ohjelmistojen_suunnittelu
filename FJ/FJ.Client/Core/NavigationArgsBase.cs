using System;

namespace FJ.Client.Core
{
    public abstract class NavigationArgsBase<T>
        where T : new()
    {
        public T Argument { get; set; }
        public string ArgumentTypeString => typeof(T).Name;
    }
}
