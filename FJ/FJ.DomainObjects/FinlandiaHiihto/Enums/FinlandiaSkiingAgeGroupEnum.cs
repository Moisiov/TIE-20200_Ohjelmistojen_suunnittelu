using System;
using System.ComponentModel;

namespace FJ.DomainObjects.FinlandiaHiihto.Enums
{
    public enum FinlandiaSkiingAgeGroup
    {
        // TODO Loc
        [Description("alle 35-vuotiaat")]
        LessThan35 = 0,

        [Description("35–39-vuotiaat")]
        ThirtyFive = 35,

        [Description("40–44-vuotiaat")]
        Forty = 40,

        [Description("45–49-vuotiaat")]
        FortyFive = 45,

        [Description("50–54-vuotiaat")]
        Fifty = 50,

        [Description("55–59-vuotiaat")]
        FiftyFive = 55,

        [Description("60–64-vuotiaat")]
        Sixty = 60,

        [Description("65–69-vuotiaat")]
        SixtyFive = 65,

        [Description("70–74-vuotiaat")]
        Seventy = 70,

        [Description("75–79-vuotiaat")]
        SeventyFive = 75,

        [Description("80-vuotiaat ja vanhemmat")]
        OverEighty = 1000
    }
}
