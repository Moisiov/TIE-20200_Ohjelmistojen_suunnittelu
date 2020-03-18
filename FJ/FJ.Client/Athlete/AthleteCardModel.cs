using System;
using System.Collections.Generic;

namespace FJ.Client.Athlete
{
    public class AthleteCardModel
    {
        public IEnumerable<AthleteParticipationItemModel> GetAthleteParticipationData(string AthleteName)
        {
            return new[] {
                new AthleteParticipationItemModel(2005, "50km perinteinen"),
                new AthleteParticipationItemModel(2006, "30km vapaa"),
                new AthleteParticipationItemModel(2006, "50km perinteinen"),
                new AthleteParticipationItemModel(2008, "30km vapaa"),
                new AthleteParticipationItemModel(2009, "100km perinteinen"),
            };
        }
    }
}
