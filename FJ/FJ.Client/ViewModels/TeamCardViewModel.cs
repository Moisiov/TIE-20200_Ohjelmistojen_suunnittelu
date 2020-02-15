using System;
using FJ.Client.Models;

namespace FJ.Client.ViewModels
{
    public class TeamCardViewModel : ViewModelBase
    {
        private TeamCardModel m_model;

        public TeamCardViewModel()
        {
            m_model = new TeamCardModel();
        }
    }
}
