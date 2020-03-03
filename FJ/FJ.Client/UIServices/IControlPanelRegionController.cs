using System;
using FJ.Client.UIUtils;

namespace FJ.Client.UIServices
{
    public interface IControlPanelRegionController
    {
        ControlPanelSizeOption CurrentControlPanelSizeOption { get; }

        void Expand();
        void Minimize();
        void SetControlPanelSize(ControlPanelSizeOption option);
    }
}
