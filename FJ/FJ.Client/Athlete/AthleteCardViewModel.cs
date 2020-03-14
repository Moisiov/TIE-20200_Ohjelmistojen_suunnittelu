using System;
﻿using FJ.Client.UICore;

namespace FJ.Client.Athlete
{
    public class AthleteCardViewModel : ViewModelBase<AthleteCardArgs>
    {
        private AthleteCardModel m_model;

        private string m_athleteName;
        public string AthleteName
        {
            get => m_athleteName;
            set => SetAndRaise(ref m_athleteName, value);
        }

        public AthleteCardViewModel()
        {
            m_model = new AthleteCardModel();
        }

        public override void DoPopulate()
        {
            base.DoPopulate();
            AthleteName = Argument.AthleteName ?? string.Empty;
        }
    }
}
