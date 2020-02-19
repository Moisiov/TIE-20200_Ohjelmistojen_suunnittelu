using System;
using Prism.Events;
using Prism.Regions;

namespace FJ.Client.UIEvents
{
    public class ContentRegionNavigationEvent : PubSubEvent<RegionNavigationEventArgs>
    {
    }

    public class ContentRegionRefreshRequestedEvent : PubSubEvent
    {
    }
}
