using System;
using FJ.Client.UIEvents;
using FJ.Client.UIUtils;
using Prism.Events;

namespace FJ.Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ControlPanelSizeOption m_controlPanelSize;
        public ControlPanelSizeOption ControlPanelSize
        {
            get => m_controlPanelSize;
            set => SetAndRaise(ref m_controlPanelSize, value);
        }

        public MainWindowViewModel(IEventAggregator ea)
        {
            ea.GetEvent<ControlPanelRegionResizeEvent>().Subscribe(SetControlPanelSize);

            SetControlPanelSize(UIStartupConstants.C_InitialControlPanelSizeOption);
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
