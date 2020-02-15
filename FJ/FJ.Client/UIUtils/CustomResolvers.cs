using System;
using System.Reflection;

namespace FJ.Client.UIUtils
{
    public static class CustomResolvers
    {
        /// <summary>
        /// Resolves the correct ViewModel that should be wired to the View
        /// </summary>
        /// <param name="viewType">View type of which view model type should be resolved</param>
        /// <returns>Type of the ViewModel that should be wired to the view type</returns>
        public static Type ViewToViewModelResolver(Type viewType)
        {
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
            var viewModelName = $"{viewName}{suffix}";

            return viewType.GetTypeInfo().Assembly.GetType(viewModelName, true);
        }
    }
}
