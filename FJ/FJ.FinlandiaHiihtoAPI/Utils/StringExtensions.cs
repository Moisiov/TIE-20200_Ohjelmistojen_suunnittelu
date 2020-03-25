﻿using System;

namespace FJ.FinlandiaHiihtoAPI.Utils
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

        public static int? ToNullableInt(this string source)
        {
            return int.TryParse(source, out var i) ? (int?)i : null;
        }

        public static string EmptyToNull(this string source)
        {
            return source.IsNullOrEmpty() || source.IsNullOrWhitespace() || source == "&nbsp;"
                ? null
                : source;
        }
    }
}