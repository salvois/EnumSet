/*
EnumSet - Immutable, efficient, small IReadOnlySet for C# enums

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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace EnumSet;

/// Immutable, efficient IReadOnlySet for enum values with the memory footprint of 32-bit integer
public readonly record struct EnumSet<T>(uint Flags) : IReadOnlySet<T> where T : Enum
{
    /// Returns the number elements in this EnumSet
    public int Count => BitOperations.PopCount(Flags);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// Returns an enumerator for this EnumSet
    public IEnumerator<T> GetEnumerator() => new EnumSetEnumerator<T>(this);

    /// Returns a string representation for this EnumSet
    public override string ToString() =>
        $"EnumSet.Of({string.Join(", ", this.Select(e => e.ToString()))})";

    /// Returns true if this EnumSet contains the specified value
    public bool Contains(T value) => (Flags & EnumSet.ToFlag(value)) != 0;

    /// Returns true if this EnumSet is a strict subset of the other enumerable
    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        if (other is not EnumSet<T> otherEnumSet)
            otherEnumSet = EnumSet.Of(other);
        return Flags != otherEnumSet.Flags && (Flags | otherEnumSet.Flags) == otherEnumSet.Flags;
    }

    /// Returns true if this EnumSet is a strict superset of the other enumerable
    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        if (other is not EnumSet<T> otherEnumSet)
            otherEnumSet = EnumSet.Of(other);
        return Flags != otherEnumSet.Flags && (Flags | otherEnumSet.Flags) == Flags;
    }

    /// Returns true if this EnumSet is a subset of the other enumerable
    public bool IsSubsetOf(IEnumerable<T> other)
    {
        if (other is not EnumSet<T> otherEnumSet)
            otherEnumSet = EnumSet.Of(other);
        return (Flags | otherEnumSet.Flags) == otherEnumSet.Flags;
    }

    /// Returns true if this EnumSet is a superset of the other enumerable
    public bool IsSupersetOf(IEnumerable<T> other)
    {
        if (other is not EnumSet<T> otherEnumSet)
            otherEnumSet = EnumSet.Of(other);
        return (Flags | otherEnumSet.Flags) == Flags;
    }

    /// Return true if this EnumSet overlaps with the other enumerable
    public bool Overlaps(IEnumerable<T> other)
    {
        if (other is not EnumSet<T> otherEnumSet)
            otherEnumSet = EnumSet.Of(other);
        return (Flags & otherEnumSet.Flags) != 0u;
    }

    /// Return true if this EnumSet contains the same elements of the other enumerable
    public bool SetEquals(IEnumerable<T> other)
    {
        if (other is not EnumSet<T> otherEnumSet)
            otherEnumSet = EnumSet.Of(other);
        return Flags == otherEnumSet.Flags;
    }
}