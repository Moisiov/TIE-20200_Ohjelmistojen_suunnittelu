using System;
using FJ.Client.Models;

namespace FJ.Client.ViewModels
{
    public class AthleteCardViewModel : ViewModelBase
    {
        private AthleteCardModel m_model;

        public AthleteCardViewModel()
        {
            m_model = new AthleteCardModel();
        }
    }
}
