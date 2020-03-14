using System;
<<<<<<< HEAD:FJ/FJ.Client/Athlete/AthleteCardViewModel.cs
﻿using FJ.Client.UICore;
=======
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FJ.Client.ArgumentClasses;
using FJ.Client.Models;
using FJ.Client.UICore;
>>>>>>> AthleteCard: Create intial layout and display some dummy data in the view.:FJ/FJ.Client/ViewModels/AthleteCardViewModel.cs

namespace FJ.Client.Athlete
{
    public class AthleteCardViewModel : ViewModelBase<AthleteCardArgs>
    {
        public class ParticipationsModel
        {
            public bool IsSelected { get; set; }
            public int Year { get; set; }
            public string StyleAndDistance { get; set; }

            public ParticipationsModel(int y, string sd)
            {
                Year = y;
                StyleAndDistance = sd;
            }
        }

        private AthleteCardModel m_model;

        public ObservableCollection<ParticipationsModel> Participations { get; set; }

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

        private static IEnumerable<ParticipationsModel> CreateTestParticipationsData()
        {
            return new[] {
                new ParticipationsModel(2005, "50km perinteinen"),
                new ParticipationsModel(2006, "30km vapaa"),
                new ParticipationsModel(2006, "50km perinteinen"),
                new ParticipationsModel(2008, "30km vapaa"),
                new ParticipationsModel(2009, "100km perinteinen"),
            };
        }

        public override void DoPopulate()
        {
            base.DoPopulate();
            AthleteName = Argument.AthleteName ?? string.Empty;

            if(!String.IsNullOrEmpty(AthleteName))
            {
                var res = CreateTestParticipationsData();
                Participations = new ObservableCollection<ParticipationsModel>(res);
                RaisePropertyChanged(nameof(Participations));
            }
        }

        protected override void DoRefreshInternal()
        {
            Participations = new ObservableCollection<ParticipationsModel>();
            RaisePropertyChanged(nameof(Participations));
            AthleteName = null;
        }
    }
}
