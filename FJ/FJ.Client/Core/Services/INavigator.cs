using System;
using Avalonia.Controls;

namespace FJ.Client.Core.Services
{
    public interface INavigator
    {
        /// <summary>
        /// Navigates to target view
        /// </summary>
        /// <param name="targetViewName">
        /// Name of the view that will be navigated to</param>
        /// <param name="navArgs">
        /// Argument that will be passed to the navigation target</param>
        void DoNavigateTo(string targetViewName, object navArgs = null);

        /// <summary>
        /// Navigates  to target view
        /// </summary>
        /// <typeparam name="TView">
        /// Type of the view that is navigated to</typeparam>
        /// <param name="navArgs">
        /// Argument that will be passed to the navigation target</param>
        void DoNavigateTo<TView>(object navArgs = null)
            where TView : UserControl;
    }
}
