using System;
using FJ.Client.Core.Common;
using Prism.Regions;

namespace FJ.Client.Core.Services
{
    public static class RegionManagerExtensions
    {
        /// <summary>
        /// Navigates to view in content region
        /// </summary>
        /// <param name="regionManager">Region manager which controls the content region</param>
        /// <param name="targetView">Name of navigation target view</param>
        public static void NavigateControlPanelTo(this IRegionManager regionManager, string targetView, object navArgs = null)
        {
            regionManager.RequestNavigate(Regions.ControlPanelRegion, targetView, CreateNavigationParameters(navArgs));
        }

        /// <summary>
        /// Navigates to view in top bar region
        /// </summary>
        /// <param name="regionManager">Region manager which controls the top bar region</param>
        /// <param name="targetView">Name of navigation target view</param>
        /// <param name="navArgs">Optional navigation argument</param>
        public static void NavigateTopBarTo(this IRegionManager regionManager, string targetView, object navArgs = null)
        {
            regionManager.RequestNavigate(Regions.TopBarRegion, targetView, CreateNavigationParameters(navArgs));
        }

        /// <summary>
        /// Navigates to view in control panel region
        /// </summary>
        /// <param name="regionManager">Region manager which controls the control panel region</param>
        /// <param name="targetView">Name of navigation target view</param>
        /// /// <param name="navArgs">Optional navigation argument</param>
        public static void NavigateContentTo(this IRegionManager regionManager, string targetView, object navArgs = null)
        {
            regionManager.RequestNavigate(Regions.ContentRegion, targetView, CreateNavigationParameters(navArgs));
        }

        private static NavigationParameters CreateNavigationParameters(object navArgs)
        {
            if (navArgs == null)
            {
                return null;
            }

            var navParams = new NavigationParameters
            {
                { navArgs.GetType().Name, navArgs }
            };

            return navParams;
        }
    }
}
