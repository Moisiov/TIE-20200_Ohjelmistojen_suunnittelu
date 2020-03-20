using System;
﻿using FJ.Client.UICore;

namespace FJ.Client.Athlete
{
    public class AthleteCardArgs : NavigationArgsBase<AthleteCardArgs>
    {
        public string AthleteFirstName { get; set; }
        public string AthleteLastName { get; set; }
    }
}
