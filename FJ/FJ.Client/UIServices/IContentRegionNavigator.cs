using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace FJ.Client.UIServices
{
    public interface IDisposableLoadingScreen : IDisposable
    {
    }

    public interface IContentRegionNavigator
    {
        bool CanNavigateBack { get; }
        bool CanNavigateForward { get; }

        string GetCurrentViewName();
        string GetCurrentViewModelName();

        void DoNavigateBack();
        void DoNavigateForward();
        void RequestRefresh();

        void DoNavigateTo(string targetViewName, object navArgs = null);
        void DoNavigateTo<TView>(object navArgs = null)
            where TView : UserControl;

        void SetLoadingScreen(bool doShowLoadingScreen);

        IDisposableLoadingScreen ShowLoadingScreen();
    }
}
