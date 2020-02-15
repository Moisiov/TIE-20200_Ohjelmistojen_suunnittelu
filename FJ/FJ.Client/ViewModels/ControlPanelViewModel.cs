using System;
using FJ.Client.Models;
using FJ.Client.Services;

namespace FJ.Client.ViewModels
{
    public class ControlPanelViewModel
    {
        private readonly IContentRegionNavigator m_contentRegionNavigator;

        private ControlPanelModel m_model;

        public ControlPanelViewModel(IContentRegionNavigator contentRegionNavigator)
        {
            m_contentRegionNavigator = contentRegionNavigator;

            m_model = new ControlPanelModel();
        }
    }
}
