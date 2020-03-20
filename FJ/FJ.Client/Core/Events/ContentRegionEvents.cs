using System;
using Prism.Events;
using Prism.Regions;

namespace FJ.Client.Core.Events
{
    public class ContentRegionNavigationEvent : PubSubEvent<RegionNavigationEventArgs>
    {
    }

    public class ContentRegionRefreshRequestedEventArgs
    {
        public string TargetViewName { get; set; }
        public string TargetViewModelName { get; set; }
    }

    public class ContentRegionRefreshRequestedEvent : PubSubEvent<ContentRegionRefreshRequestedEventArgs>
    {
    }

    public class ContentRegionLoadingScreenEvent : PubSubEvent<bool>
    {
    }
}
