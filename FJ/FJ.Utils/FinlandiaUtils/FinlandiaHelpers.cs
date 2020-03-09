using System;
using FJ.DomainObjects.FinlandiaHiihto.Enums;

namespace FJ.Utils.FinlandiaUtils
{
    public static class FinlandiaHelpers
    {
        public static string GetDistanceAndStyleLongString(FinlandiaSkiingDistance distance, FinlandiaSkiingStyle style)
        {
            // TODO Loc
            return $"{(int)distance}km {style.GetDescription()}";
        }

        public static string GetDistanceAndStyleShortString(FinlandiaSkiingDistance distance, FinlandiaSkiingStyle style)
        {
            return $"{style.GetShortDescription()}{(int)distance}";
        }
    }
}
