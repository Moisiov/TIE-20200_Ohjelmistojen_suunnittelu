using System;

namespace FJ.Client.Core
{
    /// <summary>
    /// Base class for any navigation args classes
    /// </summary>
    /// <typeparam name="T">Subclass type</typeparam>
    public abstract class NavigationArgsBase<T>
        where T : new()
    {
        public T Argument { get; set; }
        public string ArgumentTypeString => typeof(T).Name;
    }
    
    /// <summary>
    /// Dummy navigation args implementation for views that do take args via navigation
    /// </summary>
    public sealed class EmptyNavigationArgs : NavigationArgsBase<EmptyNavigationArgs>
    {
    }
}
