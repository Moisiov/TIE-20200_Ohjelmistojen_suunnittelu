﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FJ.Utils.FinlandiaUtils
{
    public static class EnumHelpers
    {
        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
