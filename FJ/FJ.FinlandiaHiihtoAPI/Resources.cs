using System;

namespace FJ.FinlandiaHiihtoAPI
{
    internal static class Resources
    {
        internal const string C_Url = "https://www.finlandiahiihto.fi/Tulokset/Tulosarkisto";

        internal static string[] S_RequestFieldNames = {
            "dnn$ctr1025$Etusivu$ddlVuosi2x",
            "dnn$ctr1025$Etusivu$txtHakuEtunimi2",
            "dnn$ctr1025$Etusivu$txtHakuSukunimi2",
            "dnn$ctr1025$Etusivu$ddlMatka2x",
            "dnn$ctr1025$Etusivu$ddlIkaluokka2",
            "dnn$ctr1025$Etusivu$txtHakuPaikkakunta2",
            "dnn$ctr1025$Etusivu$txtHakuJoukkue2",
            "dnn$ctr1025$Etusivu$chkLstSukupuoli2",
            "dnn$ctr1025$Etusivu$ddlKansalaisuus2x"

        };

        internal static string[] S_DataHeaders = {
            "Vuosi",
            "Matka",
            "Tulos",
            "Sija",
            "Sija/Miehet",
            "Sija/Naiset",
            "Sukupuoli",
            "Sukunimi Etunimi",
            "Paikkakunta",
            "Kansallisuus",
            "Syntymävuosi",
            "Joukkue"
        };
    }
}
