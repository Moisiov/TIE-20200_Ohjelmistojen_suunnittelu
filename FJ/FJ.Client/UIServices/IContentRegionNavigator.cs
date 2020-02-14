using System;
using Avalonia.Controls;

namespace FJ.Client.Services
{
    public interface IContentRegionNavigator
    {
        public bool CanNavigateBack { get; }
        public bool CanNavigateForward { get; }

        public void DoNavigateBack();
        public void DoNavigateForward();
        public void RequestRefresh();

        public void DoNavigateTo(string targetViewName);
        public void DoNavigateTo<TView>()
            where TView : UserControl;
    }
}
