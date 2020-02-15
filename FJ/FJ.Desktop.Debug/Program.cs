using System;
using FJ.Client;
using Unity;

namespace FJ.Desktop.Debug
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            Registrations.DoRegistrations(container);

            // Entry point for client
            FJClientEntryPoint.RunApplication(container, args);
        }
    }
}
