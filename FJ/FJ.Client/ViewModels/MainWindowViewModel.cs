using System;
using FJ.Client.UIEvents;
using FJ.Client.UIUtils;
using Prism.Events;
using ReactiveUI;

namespace FJ.Client.ViewModels
{
    // TODO not deriving from ViewModelBase as a temp workaround for region instantiating
    public class MainWindowViewModel : ReactiveObject
    {
        public ControlPanelSizeOption m_controlPanelSize;
        public ControlPanelSizeOption ControlPanelSize
        {
            get => m_controlPanelSize;
            set
            {
                m_controlPanelSize = value;
                ((IReactiveObject)this).RaisePropertyChanged(nameof(ControlPanelSize));
            }
        }
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
