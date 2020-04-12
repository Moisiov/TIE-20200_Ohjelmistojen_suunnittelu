using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FJ.Client.Core;
using FJ.Utils;

namespace FJ.Client.Athlete
{
    public class AthleteCardViewModel : ViewModelBase<AthleteCardArgs>
    {
        private AthleteCardModel m_model;

        public ObservableCollection<AthleteParticipationItemModel> Participations { get; set; }

        private string m_athleteFirstName;
        public string AthleteFirstName
        {
            get => m_athleteFirstName;
            set => SetAndRaise(ref m_athleteFirstName, value);
        }

        private string m_athleteLastName;
        public string AthleteLastName
        {
            get => m_athleteLastName;
            set => SetAndRaise(ref m_athleteLastName, value);
        }

        public AthleteCardViewModel()
        {
            m_model = new AthleteCardModel();
        }

        public override void DoPopulate()
        {
            base.DoPopulate();
            AthleteFirstName = Argument.AthleteFirstName ?? string.Empty;
            AthleteLastName = Argument.AthleteLastName ?? string.Empty;

            if (AthleteFirstName.IsNullOrEmpty())
            {
                return;
            }

            var res = m_model.GetAthleteParticipationData(AthleteFirstName, AthleteLastName);
            Participations = new ObservableCollection<AthleteParticipationItemModel>(res);
            RaisePropertyChanged(nameof(Participations));
        }

        protected override async Task DoRefreshInternalAsync()
        {
            AthleteFirstName = null;
            AthleteLastName = null;
            Participations = new ObservableCollection<AthleteParticipationItemModel>();
            RaisePropertyChanged(nameof(Participations));
            await Task.CompletedTask;
        }
    }
}
