using System;
using FJ.Client.UIEvents;
using FJ.Client.UIUtils;
using Prism.Events;
using Prism.Regions;

namespace FJ.Client.UIServices
{
    public class ControlPanelRegionController : IControlPanelRegionController
    {
        private readonly IEventAggregator m_eventAggregator;

        public ControlPanelSizeOption CurrentControlPanelSizeOption { get; protected set; } = UIStartupConstants.C_InitialControlPanelSizeOption;

        public ControlPanelRegionController(IEventAggregator ea)
        {
            m_eventAggregator = ea;
        }

        public void Expand()
        {
            RaiseControlPanelSizeChanged(ControlPanelSizeOption.Expanded);
        }

        public void Minimize()
        {
            RaiseControlPanelSizeChanged(ControlPanelSizeOption.Minimized);
        }

        public void SetControlPanelSize(ControlPanelSizeOption option)
        {
            RaiseControlPanelSizeChanged(option);
        }

        private void RaiseControlPanelSizeChanged(ControlPanelSizeOption option)
        {
            if (option == CurrentControlPanelSizeOption)
            {
                return;
            }

            // Control panel size is set in MainWindowViewModel
            CurrentControlPanelSizeOption = option;
            m_eventAggregator.GetEvent<ControlPanelRegionResizeEvent>().Publish(CurrentControlPanelSizeOption);
        }
    }
}
