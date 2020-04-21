using System;
using Avalonia.Controls;

namespace FJ.Client.Core.Services
{
    /// <summary>
    /// Shows loading screen between creation and destruction of this
    /// </summary>
    public interface IDisposableLoadingScreen : IDisposable
    {
    }

    /// <summary>
    /// Handles activities in relation to whole Content-region
    /// </summary>
    public interface IContentRegionNavigator
    {
        /// <summary>
        /// Navigation mode included in navigation events for targets to decide e.g. if there is a need for repopulating
        /// </summary>
        public enum NavigationMode
        {
            Unknown,
            New,
            Back,
            Forward,
            Refresh
        }

        /// <summary>
        /// Is there any views under currently shown view in Content-region's
        /// navigation stack
        /// </summary>
        bool CanNavigateBack { get; }

        /// <summary>
        /// Is there any views above currently shown view in Content-region's
        /// navigation stack
        /// </summary>
        bool CanNavigateForward { get; }

        /// <summary>
        /// Gets the name of the view that is currently shown in Content-region
        /// </summary>
        /// <returns></returns>
        string GetCurrentViewName();

        /// <summary>
        /// Gets the name of the view's data context that is currently shown
        /// in Content-region
        /// </summary>
        /// <returns></returns>
        string GetCurrentViewModelName();

        /// <summary>
        /// Navigates Content-region to view that is under currently shown
        /// view in Content-region's navigation stack. If no such view exists,
        /// does not do anything
        /// </summary>
        void DoNavigateBack();

        /// <summary>
        /// Navigates Content-region to view that is above currently shown
        /// view in Content-region's navigation stack. If no such view exists,
        /// does not do anything
        /// </summary>
        void DoNavigateForward();

        /// <summary>
        /// Raises global event that tells the view currently shown in
        /// Content-region to refresh itself
        /// </summary>
        void RequestRefresh();

        /// <summary>
        /// Navigates the Content-region to target view
        /// </summary>
        /// <param name="targetViewName">
        /// Name of the view that will be navigated to</param>
        /// <param name="navArgs">
        /// Argument that will be passed to the navigation target</param>
        void DoNavigateTo(string targetViewName, object navArgs = null);

        /// <summary>
        /// Navigates the Content-region to target view
        /// </summary>
        /// <typeparam name="TView">
        /// Type of the view that is navigated to</typeparam>
        /// <param name="navArgs">
        /// Argument that will be passed to the navigation target</param>
        void DoNavigateTo<TView>(object navArgs = null)
            where TView : UserControl;

        /// <summary>
        /// Clears the navigation stack
        /// </summary>
        void DoClearNavigationStack();
        
        /// <summary>
        /// Shows specified error message in a popup box
        /// </summary>
        /// <param name="msg">
        /// Error message that is shown in the popup box</param>
        void ShowErrorMessage(string msg);

        /// <summary>
        /// Sets the loading screen visibility in Content-region
        /// </summary>
        /// <param name="doShowLoadingScreen">
        /// Is loading screen wanted to be shown</param>
        void SetLoadingScreen(bool doShowLoadingScreen);

        /// <summary>
        /// Shows the loading screen in Content-region until the returned
        /// instance is destroyed
        /// </summary>
        /// <returns><see cref="IDisposableLoadingScreen"/></returns>
        IDisposableLoadingScreen ShowLoadingScreen();
    }
}
