using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FJ.Client.Core.UIElements.Filters;
using ReactiveUI;

namespace FJ.Client.Core.Register
{
    public abstract class RegisterItemModelBase : FJNotificationObject
    {
        private bool m_isSelected;
        public bool IsSelected
        {
            get => m_isSelected;
            set => SetAndRaise(ref m_isSelected, value);
        }
    }
    
    public abstract class RegisterItemModelBase<TCardNavigationArgs> : RegisterItemModelBase
        where TCardNavigationArgs : NavigationArgsBase<TCardNavigationArgs>, new()
    {
        public abstract TCardNavigationArgs GetNavigationArgs();
        public abstract string GetNavigationTargetName();
    }
}
