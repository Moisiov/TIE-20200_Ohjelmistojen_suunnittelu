using System;
﻿using FJ.Client.UICore;

namespace FJ.Client.Competition
{
    public class CompetitionGeneralViewModel : ViewModelBase
    {
        private CompetitionGeneralModel m_model;

        public CompetitionGeneralViewModel()
        {
            m_model = new CompetitionGeneralModel();
        }
    }
}
