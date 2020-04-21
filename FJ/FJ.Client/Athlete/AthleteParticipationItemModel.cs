﻿using System;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.Client.Athlete
{
    public class AthleteParticipationItemModel
    {
        private readonly AthleteCardViewModel m_owner;
        public FinlandiaHiihtoSingleResult ResultRows { get; set; }

        private bool m_isSelected;
        public bool IsSelected
        {
            get => m_isSelected;
            set
            {
                m_isSelected = value;
                m_owner.ItemSelectionChanged();
            }
        }

        public AthleteParticipationItemModel(AthleteCardViewModel owner)
        {
            m_owner = owner;
        }
    }
}
