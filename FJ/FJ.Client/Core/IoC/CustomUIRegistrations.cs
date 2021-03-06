﻿using System;
using Avalonia.Controls;
using FJ.Utils;
using Prism.Ioc;

namespace FJ.Client.Core.IoC
{
    public static class CustomUIRegistrations
    {
        /// <summary>
        /// Automatically registers all views derived from UserControl for navigation
        /// </summary>
        /// <param name="containerRegistry">Container registry to register views</param>
        public static void AutoRegisterViewsForNavigation(IContainerRegistry containerRegistry)
        {
            var views = ReflectionHelpers.GetClasses<UserControl>(t => t.Name.EndsWith("View"));
            foreach (var view in views)
            {
                containerRegistry.RegisterForNavigation(view, view.Name);
            }
        }
    }
}
