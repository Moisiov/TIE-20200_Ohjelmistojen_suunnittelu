using System;
using FJ.Client.Core.Services;
using Prism.Events;
using Prism.Regions;

namespace FJ.Client.Core.Events
{
    public class ContentRegionNavigationEventArgs
    {
        public RegionNavigationEventArgs EventArgs { get; set; }
        public string TargetViewName { get; set; }
        public string TargetViewModelName { get; set; }
        public IContentRegionNavigator.NavigationMode NavigationMode { get; set; }
    }
    
    public class ContentRegionNavigationEvent : PubSubEvent<ContentRegionNavigationEventArgs>
    {
    }

    public class ContentRegionRefreshRequestedEventArgs
    {
        public string TargetViewName { get; set; }
        public string TargetViewModelName { get; set; }
        public IContentRegionNavigator.NavigationMode NavigationMode => IContentRegionNavigator.NavigationMode.Refresh;
    }

    public class ContentRegionRefreshRequestedEvent : PubSubEvent<ContentRegionRefreshRequestedEventArgs>
    {
    }

    public class ContentRegionLoadingScreenEvent : PubSubEvent<bool>
    {
    }
    
    public class ContentRegionErrorEvent : PubSubEvent<string>
    {
    }

    public class ContentRegionNavigationStackClearedEvent : PubSubEvent
    {
    }
}
