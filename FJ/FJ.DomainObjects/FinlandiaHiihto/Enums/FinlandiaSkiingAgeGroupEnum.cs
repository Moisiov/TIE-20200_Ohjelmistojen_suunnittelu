using System;
using System.ComponentModel;

namespace FJ.DomainObjects.FinlandiaHiihto.Enums
{
    public enum FinlandiaSkiingAgeGroup
    {
        // TODO Loc
        [Description("alle 35")]
        LessThan35 = 0,

        [Description("35–39")]
        ThirtyFive = 35,

        [Description("40–44")]
        Forty = 40,

        [Description("45–49")]
        FortyFive = 45,

        [Description("50–54")]
        Fifty = 50,

        [Description("55–59")]
        FiftyFive = 55,

        [Description("60–64")]
        Sixty = 60,

        [Description("65–69")]
        SixtyFive = 65,

        [Description("70–74")]
        Seventy = 70,

        [Description("75–79")]
        SeventyFive = 75,

        [Description("80 tai enemmän")]
        OverEighty = 1000
    }
}
