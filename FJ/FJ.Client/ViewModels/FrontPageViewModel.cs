using System;
using FJ.Client.Models;

namespace FJ.Client.ViewModels
{
    public class FrontPageViewModel : ViewModelBase
    {
        private FrontPageModel m_model;

        public FrontPageViewModel()
        {
            m_model = new FrontPageModel();
        }
    }
}
