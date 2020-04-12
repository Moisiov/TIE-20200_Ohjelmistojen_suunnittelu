using System;
using FJ.DomainObjects;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.Client.Athlete
{
    public class AthleteParticipationItemModel
    {
        public FinlandiaHiihtoSingleResult ResultRows { get; set; }
        public bool IsSelected { get; set; }
    }
}
