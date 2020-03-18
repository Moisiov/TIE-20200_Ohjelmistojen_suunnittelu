using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FJ.Client.UICore;
using FJ.Utils;

namespace FJ.Client.Athlete
{
    public class AthleteCardViewModel : ViewModelBase<AthleteCardArgs>
    {
        private AthleteCardModel m_model;

        public ObservableCollection<AthleteParticipationItemModel> Participations { get; set; }

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

            if (AthleteName.IsNullOrEmpty())
            {
                return;
            }

            var res = m_model.GetAthleteParticipationData(AthleteName);
            Participations = new ObservableCollection<AthleteParticipationItemModel>(res);
            RaisePropertyChanged(nameof(Participations));
        }

        protected override void DoRefreshInternal()
        {
            AthleteName = null;
            Participations = new ObservableCollection<AthleteParticipationItemModel>();
            RaisePropertyChanged(nameof(Participations));
        }
    }
}
