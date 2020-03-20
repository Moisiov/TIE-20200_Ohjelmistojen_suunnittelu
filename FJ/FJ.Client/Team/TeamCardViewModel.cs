using System;
using FJ.Client.Core;

namespace FJ.Client.Team
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
