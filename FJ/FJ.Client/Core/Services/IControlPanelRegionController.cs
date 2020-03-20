using System;
using FJ.Client.Core.Common;

namespace FJ.Client.Core.Services
{
    public interface IControlPanelRegionController
    {
        ControlPanelSizeOption CurrentControlPanelSizeOption { get; }

        void Expand();
        void Minimize();
        void SetControlPanelSize(ControlPanelSizeOption option);
    }
}
