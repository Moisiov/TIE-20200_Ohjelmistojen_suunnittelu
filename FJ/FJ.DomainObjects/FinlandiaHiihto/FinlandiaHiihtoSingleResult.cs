﻿using System;

namespace FJ.DomainObjects.FinlandiaHiihto
{
    public class FinlandiaHiihtoSingleResult
    {
        public TimeSpan Result { get; set; }
        public int PositionGeneral { get; set; }
        public int? PositionMen { get; set; }
        public int? PositionWomen { get; set; }
        public FinlandiaHiihtoCompetitionClass CompetitionClass { get; set; }
        public Competition CompetitionInfo { get; set; }
        public Person Athlete { get; set; }
        public string Team { get; set; }

        public string ResultString => Result.ToString(@"hh\:mm\:ss\.ff");
    }
}
