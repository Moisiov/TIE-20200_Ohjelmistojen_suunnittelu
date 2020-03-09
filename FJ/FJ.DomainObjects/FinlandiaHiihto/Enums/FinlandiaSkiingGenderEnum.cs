using System;
using System.ComponentModel;

namespace FJ.DomainObjects.FinlandiaHiihto.Enums
{
    public enum FinlandiaSkiingGender
    {
        // TODO Loc
        [Description("Ei tiedossa")]
        Unknown = 0,

        [Description("Mies")]
        Man = 1,

        [Description("Nainen")]
        Woman = 2
    }
}
