using System;
using Avalonia.Controls;

namespace FJ.Client.UIServices
{
    public interface IContentRegionNavigator
    {
        bool CanNavigateBack { get; }
        bool CanNavigateForward { get; }

        void DoNavigateBack();
        void DoNavigateForward();
        void RequestRefresh();

        void DoNavigateTo(string targetViewName);
        void DoNavigateTo<TView>()
            where TView : UserControl;
    }
}
