using System;
using FJ.Client.Core.Common;
using FJ.Client.Core.Events;
using Prism.Events;
using ReactiveUI;

namespace FJ.Client.MainWindow
{
    // TODO not deriving from ViewModelBase as a temp workaround for region instantiating
    public class MainWindowViewModel : ReactiveObject
    {
        private ControlPanelSizeOption m_controlPanelSize;
        public ControlPanelSizeOption ControlPanelSize
        {
            get => m_controlPanelSize;
            set
            {
                m_controlPanelSize = value;
                ((IReactiveObject)this).RaisePropertyChanged(nameof(ControlPanelSize));
            }
        }

        private bool m_showContentRegionLoadingScreen;
        public bool ShowContentRegionLoadingScreen
        {
            get => m_showContentRegionLoadingScreen;
            set
            {
                m_showContentRegionLoadingScreen = value;
                ((IReactiveObject)this).RaisePropertyChanged(nameof(ShowContentRegionLoadingScreen));
            }
        }

        public MainWindowViewModel(IEventAggregator ea)
        {
            ea.GetEvent<ContentRegionLoadingScreenEvent>().Subscribe(SetContentRegionLoadingScreen);
            ea.GetEvent<ControlPanelRegionResizeEvent>().Subscribe(SetControlPanelSize);

            SetControlPanelSize(UIStartupConstants.C_InitialControlPanelSizeOption);
        }

        private void SetContentRegionLoadingScreen(bool doShowLoadingScreen)
        {
            ShowContentRegionLoadingScreen = doShowLoadingScreen;
        }

        private void SetControlPanelSize(ControlPanelSizeOption option)
        {
            ControlPanelSize = option switch
            {
                ControlPanelSizeOption.Expanded => option,
                ControlPanelSizeOption.Minimized => option,
                _ => throw new NotImplementedException(option.ToString()),
            };
        }
    }
}
