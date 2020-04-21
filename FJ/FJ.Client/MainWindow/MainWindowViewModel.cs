using System;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using FJ.Client.Core;
using FJ.Client.Core.Common;
using FJ.Client.Core.Events;
using FJ.Client.Core.Services;
using Prism.Events;
using ReactiveUI;

namespace FJ.Client.MainWindow
{
    // NOTE: Not deriving from ViewModelBase as a workaround for region instantiating
    public class MainWindowViewModel : FJNotificationObject
    {
        private readonly Lazy<IContentRegionNavigator> m_lazyNavigator;
        private IContentRegionNavigator Navigator => m_lazyNavigator.Value;
        private IClassicDesktopStyleApplicationLifetime m_lifetime;
        
        private ControlPanelSizeOption m_controlPanelSize;
        public ControlPanelSizeOption ControlPanelSize
        {
            get => m_controlPanelSize;
            set => SetAndRaise(ref m_controlPanelSize, value);
        }

        private bool m_showContentRegionLoadingScreen;
        public bool ShowContentRegionLoadingScreen
        {
            get => m_showContentRegionLoadingScreen;
            set => SetAndRaise(ref m_showContentRegionLoadingScreen, value);
        }
        
        private string m_contentRegionErrorMessage;
        public string ContentRegionErrorMessage
        {
            get => m_contentRegionErrorMessage;
            set => SetAndRaise(ref m_contentRegionErrorMessage, value);
        }

        public MainWindowViewModel(IEventAggregator ea, Lazy<IContentRegionNavigator> navigator)
        {
            m_lazyNavigator = navigator;
            ea.GetEvent<ContentRegionLoadingScreenEvent>().Subscribe(SetContentRegionLoadingScreen);
            ea.GetEvent<ControlPanelRegionResizeEvent>().Subscribe(SetControlPanelSize);
            ea.GetEvent<ContentRegionErrorEvent>().Subscribe(SetContentRegionErrorMessage);

            SetControlPanelSize(UIStartupConstants.C_InitialControlPanelSizeOption);
        }

        public void OnFrameworkInitializationCompleted()
        {
            m_lifetime = Avalonia.Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            
            /*
            Avalonia has implementation for mouse back and forward buttons waiting for release.
            TODO: Uncomment this and event handler after support is released
            
            m_lifetime?.MainWindow.AddHandler(
                InputElement.PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
            */
        }

        public void HotKeyDoNavigateBack()
        {
            if (!Navigator.CanNavigateBack)
            {
                return;
            }
            
            Navigator.DoNavigateBack();
        }

        public void HotKeyDoNavigateForward()
        {
            if (!Navigator.CanNavigateForward)
            {
                return;
            }
            
            Navigator.DoNavigateForward();
        }

        public void HotKeyDoRefresh()
        {
            Navigator.RequestRefresh();
        }

        public void HotKeyDoClosePanel()
        {
            Navigator.DoNavigateTo<FrontPage.FrontPageView>();
            Navigator.DoClearNavigationStack();
        }

        public void HotKeyDoCloseApplication(Window mainWindow)
        {
            m_lifetime?.MainWindow.Close();
        }
        
        public void ClearContentRegionErrorMessage()
        {
            ContentRegionErrorMessage = null;
        }

        private void SetContentRegionLoadingScreen(bool doShowLoadingScreen)
        {
            ShowContentRegionLoadingScreen = doShowLoadingScreen;
        }
        
        private void SetContentRegionErrorMessage(string msg)
        {
            ContentRegionErrorMessage = msg;
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
        
        /* TODO check OnFrameworkInitializationCompleted
        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.XButton1Pressed)
            {
                HotKeyDoNavigateBack();
                e.Handled = true;
            }
            else if (e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.XButton2Pressed)
            {
                HotKeyDoNavigateForward();
                e.Handled = true;
            }
        }
        */
    }
}
