using System;
using FJ.Client.Core;
using FJ.Client.Core.Common;
using FJ.Client.Core.Services;

namespace FJ.Client.ControlPanel
{
    public class ControlPanelViewModel : ViewModelBase
    {
        private readonly IControlPanelRegionController m_controlPanelRegionController;

        public bool IsExpanded => m_controlPanelRegionController.CurrentControlPanelSizeOption == ControlPanelSizeOption.Expanded;

        private ControlPanelModel m_model;

        public ControlPanelViewModel(IControlPanelRegionController controPanelRegionController)
        {
            m_controlPanelRegionController = controPanelRegionController;

            m_model = new ControlPanelModel();
        }

        public void DoExpand()
        {
            m_controlPanelRegionController.Expand();
            RaisePropertyChanged(nameof(IsExpanded));
        }

        public void DoMinimize()
        {
            m_controlPanelRegionController.Minimize();
            RaisePropertyChanged(nameof(IsExpanded));
        }
    }
}
