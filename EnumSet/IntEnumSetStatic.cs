﻿/*
EnumSet - Immutable, efficient, small, equatable IReadOnlySet for C# enums

Copyright 2024 Salvatore ISAJA. All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation
and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED THE COPYRIGHT HOLDER ``AS IS'' AND ANY EXPRESS
OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN
NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumSet;

/// Provides static methods to work with IntEnumSet&lt;T&gt;
public static class IntEnumSet
{
    /// The maximum enum value that can be saved in an EnumSet
    public const int MaxValue = sizeof(int) * 8 - 1;

    /// Singleton EnumSet containing no elements
    public static readonly EmptyEnumSet Empty = EmptyEnumSet.Empty;

    /// Creates an EnumSet from the specified value
    public static IntEnumSet<T> Of<T>(T value) where T : Enum =>
        new(ToFlag(value));

    /// Creates an EnumSet from the specified values
    public static IntEnumSet<T> Of<T>(params T[] values) where T : Enum =>
        FromList(values);

    /// Creates an EnumSet from the specified IEnumerable
    public static IntEnumSet<T> Of<T>(IEnumerable<T> values) where T : Enum =>
        values is IReadOnlyList<T> list
            ? FromList(list)
            : new IntEnumSet<T>(values.Aggregate(0u, (current, value) => current | ToFlag(value)));

    /// Creates an EnumSet from the specified IEnumerable, fluently
    public static IntEnumSet<T> ToIntEnumSet<T>(this IEnumerable<T> values) where T : Enum =>
        Of(values);

    /// Returns true if this EnumSet contains any value
    public static bool Any<T>(this IntEnumSet<T> enumSet) where T : Enum =>
        enumSet.Flags != 0;

    /// Returns a new EnumSet as the intersection of this EnumSet with the other enumerable
    public static IntEnumSet<T> Intersect<T>(this IntEnumSet<T> enumSet, IEnumerable<T> other) where T : Enum
    {
        if (other is not IntEnumSet<T> otherEnumSet)
            otherEnumSet = Of(other);
        return new IntEnumSet<T>(enumSet.Flags & otherEnumSet.Flags);
    }

    /// Returns a new EnumSet removing the specified value from this EnumSet
    public static IntEnumSet<T> Except<T>(this IntEnumSet<T> enumSet, T value) where T : Enum =>
        new(enumSet.Flags & ~ToFlag(value));

    /// Returns a new EnumSet removing the values of the other enumerable from this EnumSet
    public static IntEnumSet<T> Except<T>(this IntEnumSet<T> enumSet, IEnumerable<T> other) where T : Enum
    {
        if (other is not IntEnumSet<T> otherEnumSet)
            otherEnumSet = Of(other);
        return new IntEnumSet<T>(enumSet.Flags & ~otherEnumSet.Flags);
    }

    /// Returns a new EnumSet as the union of this EnumSet with the specified value
    public static IntEnumSet<T> Union<T>(this IntEnumSet<T> enumSet, T value) where T : Enum =>
        new(enumSet.Flags | ToFlag(value));

    /// Returns a new EnumSet as the union of this EnumSet with the other enumerable
    public static IntEnumSet<T> Union<T>(this IntEnumSet<T> enumSet, IEnumerable<T> other) where T : Enum
    {
        if (other is not IntEnumSet<T> otherEnumSet)
            otherEnumSet = Of(other);
        return new IntEnumSet<T>(enumSet.Flags | otherEnumSet.Flags);
    }

    /// Converts the specified value to a flag
    internal static uint ToFlag<T>(T value) where T : Enum
    {
        var i = Convert.ToInt32(value);
        if (i is < 0 or > MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value));
        return 1u << i;
    }

    private static IntEnumSet<T> FromList<T>(IReadOnlyList<T> list) where T : Enum
    {
        // Avoid foreach to avoid memory allocations
        var flags = 0u;
        for (var i = 0; i < list.Count; i++)
            flags |= ToFlag(list[i]);
        return new IntEnumSet<T>(flags);
    }
}