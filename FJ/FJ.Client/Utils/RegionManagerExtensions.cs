using System;
using Prism.Regions;

namespace FJ.Client.Utils
{
    public static class RegionManagerExtensions
    {
        /// <summary>
        /// Navigates to view in content region
        /// </summary>
        /// <param name="regionManager">Region manager which controls the content region</param>
        /// <param name="targetView">Name of navigation target view</param>
        public static void NavigateControlPanelTo(this IRegionManager regionManager, string targetView)
        {
            regionManager.RequestNavigate(Regions.ControlPanelRegion, targetView);
        }

        /// <summary>
        /// Navigates to view in top bar region
        /// </summary>
        /// <param name="regionManager">Region manager which controls the top bar region</param>
        /// <param name="targetView">Name of navigation target view</param>
        public static void NavigateTopBarTo(this IRegionManager regionManager, string targetView)
        {
            regionManager.RequestNavigate(Regions.TopBarRegion, targetView);
        }

        /// <summary>
        /// Navigates to view in control panel region
        /// </summary>
        /// <param name="regionManager">Region manager which controls the control panel region</param>
        /// <param name="targetView">Name of navigation target view</param>
        public static void NavigateContentTo(this IRegionManager regionManager, string targetView)
        {
            regionManager.RequestNavigate(Regions.ContentRegion, targetView);
        }
    }
}
