using System;

namespace FJ.Utils
{
    public static class StringExtensions
    {
        public static string ReplaceLastOccurrence(this string source, string find, string replace)
        {
            var place = source.LastIndexOf(find);
            return place < 0 ? source : source.Remove(place, find.Length).Insert(place, replace);
        }
    }
}
