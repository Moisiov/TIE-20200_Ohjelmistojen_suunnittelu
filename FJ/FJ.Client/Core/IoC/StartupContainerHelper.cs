using System;
using Unity;

namespace FJ.Client.Core.IoC
{
    internal static class StartupContainerHelper
    {
        internal static IUnityContainer StartupContainer { get; private set; }

        internal static void SetStartupContainer(IUnityContainer container)
        {
            if (StartupContainer != null)
            {
                throw new Exception("Startup container should only be set once");
            }

            StartupContainer = container;
        }
    }
}
