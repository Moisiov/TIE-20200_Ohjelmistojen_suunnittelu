using System;
using FJ.DomainObjects.FinlandiaHiihto;

namespace FJ.Utils.FinlandiaUtils
{
    public static class FinlandiaExtensions
    {
        public static string AsString(this FinlandiaHiihtoCompetitionClass cc)
        {
            return $"{FinlandiaHelpers.GetDistanceAndStyleLongString(cc.Distance, cc.Style)}{cc.AdditionalDescription}";
        }
    }
}
