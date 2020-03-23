using System;
using System.ComponentModel;

namespace FJ.DomainObjects.Enums
{
    public enum Gender
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
