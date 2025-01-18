# EnumSet - Immutable, efficient, small, equatable IReadOnlySet for C# enums

[![NuGet](https://img.shields.io/nuget/v/EnumSet.svg)](https://www.nuget.org/packages/EnumSet)

This little library provides an implementation of IReadOnlySet tailored for enums.

Inspired by the EnumSet collection of Java, the `IntEnumSet` provided by this library is immutable and is backed by a plain unsigned integer where enum values are stored as flags, in order be easy on the garbage collector and be very efficient in space and time, while exposing the familiar interface for C# set collections, hiding the complexity of dealing with flags.

This is especially well suited where a large number of set of enums must be created, such as a bulk of data with some validation flags attached.


## IntEnumSet&lt;T&gt; collection

The actual collection is provided by the `IntEnumSet<T>` readonly record struct, which implements the `IReadOnlySet<T>` standard interface.

It contains a single `uint Flags` property, which represents the combination of enum values stored in the collection as flags, and is not meant to be used directly. The `Flags` property is public to ease testing and get equality for free thanks to records.

**Note:** since the actual storage is a bit field containing 32 bits (the `uint Flags` property), you can store at most 32 different enum values, ranging from 0 to 31. If you try to add a value outside this range, an `ArgumentOutOfRangeException` is thrown.

```csharp
namespace EnumSet;

public readonly record struct IntEnumSet<T>(uint Flags) : IReadOnlySet<T> where T : Enum
{
    /// Named constructors
    public static IntEnumSet<T> Of(T value);
    public static IntEnumSet<T> Of(params T[] values);
    public static IntEnumSet<T> Of(IEnumerable<T> values);
    public static IntEnumSet<T> Of(IReadOnlyList<T> list);

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

    /// Returns true if this EnumSet contains any value
    public bool Any();

    /// Returns a new EnumSet as the intersection of this EnumSet with the other
    public IntEnumSet<T> Intersect(IntEnumSet<T> other);
    public IntEnumSet<T> Intersect(IEnumerable<T> other);

    /// Returns a new EnumSet removing other from this EnumSet
    public IntEnumSet<T> Except(T other);
    public IntEnumSet<T> Except(IntEnumSet<T> other);
    public IntEnumSet<T> Except(IEnumerable<T> other);

    /// Returns a new EnumSet as the union of this EnumSet with other
    public IntEnumSet<T> Union(T value);
    public IntEnumSet<T> Union(IntEnumSet<T> other);
    public IntEnumSet<T> Union(IEnumerable<T> other);
}
```

The `IntEnumSet` static class provides functionality to simplify working with `IntEnumSet<T>` collections.

```csharp
namespace EnumSet;

public static class IntEnumSet
{
    /// Singleton non-generic EnumSet containing no elements
    public static readonly EmptyEnumSet Empty = EmptyEnumSet.Empty;

    /// Named constructors with type inference
    public static IntEnumSet<T> Of<T>(T value) where T : Enum;
    public static IntEnumSet<T> Of<T>(params T[] values) where T : Enum;
    public static IntEnumSet<T> Of<T>(IEnumerable<T> values) where T : Enum;
    public static IntEnumSet<T> Of<T>(IReadOnlyList<T> values) where T : Enum;

    /// Creates an EnumSet from the specified IEnumerable, fluently
    public static IntEnumSet<T> ToIntEnumSet<T>(this IEnumerable<T> values) where T : Enum;
}
```
The `EmptyEnumSet` non-generic type, which is implicitly convertible to any `IntEnumSet<T>`, provides for a simple way to pass empty enum sets inferring the type. Just use `IntEnumSet.Empty` to specify an enum set containing no elements.


## License

Permissive, [2-clause BSD style](https://opensource.org/licenses/BSD-2-Clause)

EnumSet - Immutable, efficient, small, equatable IReadOnlySet for C# enums

Copyright 2024-2025 Salvatore ISAJA. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.