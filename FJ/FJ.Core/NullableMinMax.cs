﻿using System;

namespace FJ.Core
{
    public class NullableMinMax<T>
        where T : struct, IComparable
    {
        public T? Min { get; set; } = null;
        public T? Max { get; set; } = null;

        public bool HasBothValues => Min.HasValue && Max.HasValue;
    }
}