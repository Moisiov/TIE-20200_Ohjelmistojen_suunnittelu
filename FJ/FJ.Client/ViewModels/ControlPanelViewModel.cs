using System;
using FJ.Client.Models;
using FJ.Client.UIServices;
using FJ.Client.UIUtils;

namespace FJ.Client.ViewModels
{
    public class ControlPanelViewModel : ViewModelBase
    {
        private readonly IControlPanelRegionController m_controlPanelRegionController;
        private readonly IContentRegionNavigator m_contentRegionNavigator;

        public bool IsExpanded => m_controlPanelRegionController.CurrentControlPanelSizeOption == ControlPanelSizeOption.Expanded;

        private ControlPanelModel m_model;

        public ControlPanelViewModel(IControlPanelRegionController controPanelRegionController, IContentRegionNavigator contentRegionNavigator)
        {
            m_controlPanelRegionController = controPanelRegionController;
            m_contentRegionNavigator = contentRegionNavigator;

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
