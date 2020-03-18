using System;

namespace FJ.Client.Athlete
{
    public class AthleteParticipationItemModel
    {
        public int Year { get; set; }
        public string StyleAndDistance { get; set; }

        public bool IsSelected { get; set; }

        public AthleteParticipationItemModel(int participationYear, string styleAndDistString)
        {
            Year = participationYear;
            StyleAndDistance = styleAndDistString;
        }
    }
}
