﻿using NStandard.Measures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NStandard;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class IMeasureExtensions
{
    public static TMeasure QSum<TMeasure>(this IEnumerable<TMeasure> @this) where TMeasure : struct, IMeasurable
    {
        decimal sum = 0;
        foreach (var item in @this)
        {
            sum += item.Value;
        }

        return new TMeasure
        {
            Value = sum,
        };
    }

    public static TMeasure QSum<TMeasure>(this IEnumerable<TMeasure?> @this) where TMeasure : struct, IMeasurable
    {
        decimal sum = 0;
        foreach (var item in @this)
        {
            if (!item.HasValue) continue;

            sum += item.Value.Value;
        }

        return new TMeasure
        {
            Value = sum,
        };
    }

    public static TMeasure QAverage<TMeasure>(this IEnumerable<TMeasure> @this) where TMeasure : struct, IMeasurable
    {
        if (!@this.Any()) throw new InvalidOperationException("Sequence contains no elements");

        decimal sum = 0;
        int count = 0;
        foreach (var item in @this)
        {
            sum += item.Value;
            count++;
        }

        return new TMeasure
        {
            Value = sum / count,
        };
    }

    public static TMeasure QAverageOrDefault<TMeasure>(this IEnumerable<TMeasure> @this, TMeasure @default = default) where TMeasure : struct, IMeasurable
    {
        if (!@this.Any()) return @default;

        decimal sum = 0;
        int count = 0;
        foreach (var item in @this)
        {
            sum += item.Value;
            count++;
        }

        return new TMeasure
        {
            Value = sum / count,
        };
    }

    public static TMeasure? QAverage<TMeasure>(this IEnumerable<TMeasure?> @this) where TMeasure : struct, IMeasurable
    {
        if (!@this.Any()) return default;

        decimal sum = 0;
        int count = 0;
        foreach (var item in @this)
        {
            if (!item.HasValue) continue;

            sum += item.Value.Value;
            count++;
        }

        if (count == 0) return default;
        else
        {
            return new TMeasure
            {
                Value = sum / count,
            };
        }
    }

    public static TMeasure? QAverageOrDefault<TMeasure>(this IEnumerable<TMeasure?> @this, TMeasure? @default = default) where TMeasure : struct, IMeasurable
    {
        if (!@this.Any()) return @default;

        decimal sum = 0;
        int count = 0;
        foreach (var item in @this)
        {
            if (!item.HasValue) continue;

            sum += item.Value.Value;
            count++;
        }

        if (count == 0) return @default;
        else
        {
            return new TMeasure
            {
                Value = sum / count,
            };
        }
    }
}
