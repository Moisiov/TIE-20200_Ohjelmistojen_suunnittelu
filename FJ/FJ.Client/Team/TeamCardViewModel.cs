using System;
﻿using FJ.Client.UICore;

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
