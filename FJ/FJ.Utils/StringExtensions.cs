using System;
using System.Linq;

namespace FJ.Utils
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        public static bool IsNullOrWhitespace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static string ReplaceLastOccurrence(this string source, string find, string replace)
        {
            var place = source.LastIndexOf(find, StringComparison.Ordinal);
            return place < 0 ? source : source.Remove(place, find.Length).Insert(place, replace);
        }

        public static string WithoutWhitespaces(this string source)
        {
            return new string(source.Where(c => !c.IsWhitespace()).ToArray());
        }
    }

    public static class CharExtensions
    {
        public static bool IsWhitespace(this char c)
        {
            return char.IsWhiteSpace(c);
        }

        public static bool IsDigit(this char c)
        {
            return char.IsDigit(c);
        }
    }
}
