# EnumSet - Immutable, efficient, small IReadOnlySet for C# enums

[![NuGet](https://img.shields.io/nuget/v/EnumSet.svg)](https://www.nuget.org/packages/EnumSet)

This little library provides an implementation of IReadOnlySet tailored for enums.

Inspired by the EnumSet collection of Java, the EnumSet provided by this library is immutable and is backed by a plain unsigned integer where enum values are stored as flags, in order be easy on the garbage collector and be very efficient in space and time, while exposing the familiar interface for C# set collections, hiding the complexity of dealing with flags.

This is especially well suited where a large number of set of enums must be created, such as a bulk of data with some validation flags attached.


## EnumSet&lt;T&gt; collection

The actual collectin is provided by the `EnumSet<T>` class, which implements `IReadOnlySet<T>` standard interface.

It is a readonly record struct containing a single `uint Flags` property, which represents the combination of enum values stored in the collection as flags, and is not meant to be used directly. The `Flags` property is public to ease testing and get equality for free thanks to records.

```csharp
public readonly record struct EnumSet<T>(uint Flags) : IReadOnlySet<T> where T : Enum
{
    /// Returns the number elements in this EnumSet
    public int Count;

    /// Returns an enumerator for this EnumSet
    public IEnumerator<T> GetEnumerator();

    /// Returns a string representation for this EnumSet
    public override string ToString();

    /// Returns true if this EnumSet contains the specified value
    public bool Contains(T value);

    /// Returns true if this EnumSet is a strict subset of the other enumerable
    public bool IsProperSubsetOf(IEnumerable<T> other);

    /// Returns true if this EnumSet is a strict superset of the other enumerable
    public bool IsProperSupersetOf(IEnumerable<T> other);

    /// Returns true if this EnumSet is a subset of the other enumerable
    public bool IsSubsetOf(IEnumerable<T> other);

    /// Returns true if this EnumSet is a superset of the other enumerable
    public bool IsSupersetOf(IEnumerable<T> other);

    /// Return true if this EnumSet overlaps with the other enumerable
    public bool Overlaps(IEnumerable<T> other);

    /// Return true if this EnumSet contains the same elements of the other enumerable
    public bool SetEquals(IEnumerable<T> other);
}
```

## Utility functions to work with EnumSets

The `EnumSet` static class provides static methods to work with `EnumSet<T>` collections.

```csharp
public static class EnumSet
{
    /// Returns an EnumSet containing no elements
    public static EnumSet<T> Empty<T>() where T : Enum;

    /// Creates an EnumSet from the specified value
    public static EnumSet<T> Of<T>(T value) where T : Enum;

    /// Creates an EnumSet from the specified values
    public static EnumSet<T> Of<T>(params T[] values) where T : Enum;

    /// Creates an EnumSet from the specified IEnumerable
    public static EnumSet<T> Of<T>(IEnumerable<T> values) where T : Enum;

    /// Returns true if this EnumSet contains any value
    public static bool Any<T>(this EnumSet<T> enumSet) where T : Enum;

    /// Returns a new EnumSet as the intersection of this EnumSet with the other enumerable
    public static EnumSet<T> Intersect<T>(this EnumSet<T> enumSet, IEnumerable<T> other) where T : Enum;

    /// Returns a new EnumSet removing the specified value from this EnumSet
    public static EnumSet<T> Remove<T>(this EnumSet<T> enumSet, T value) where T : Enum;

    /// Returns a new EnumSet removing the values of the other enumerable from this EnumSet
    public static EnumSet<T> Remove<T>(this EnumSet<T> enumSet, IEnumerable<T> other) where T : Enum;

    /// Returns a new EnumSet as the union of this EnumSet with the specified value
    public static EnumSet<T> Union<T>(this EnumSet<T> enumSet, T value) where T : Enum;

    /// Returns a new EnumSet as the union of this EnumSet with the other enumerable
    public static EnumSet<T> Union<T>(this EnumSet<T> enumSet, IEnumerable<T> other) where T : Enum;
}
```

## License

Permissive, [2-clause BSD style](https://opensource.org/licenses/BSD-2-Clause)

EnumSet - Immutable, efficient, small IReadOnlySet for C# enums

Copyright 2024 Salvatore ISAJA. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.