/*
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
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace EnumSet.Tests;

[TestFixture]
public static class IntEnumSetTest
{
    public enum Color
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        TooSmall = -1,
        TooBig = IntEnumSet.MaxValue + 1
    }

    [Test]
    public static void Flags() =>
        IntEnumSet.Of(Color.Red, Color.Blue).Flags
            .Should().Be((1 << (int)Color.Red) | (1 << (int)Color.Blue)); // that is 101b = 5

    [Test]
    public static void FromFlags() =>
        new IntEnumSet<Color>((1 << (int)Color.Red) | (1 << (int)Color.Blue)) // that is 101b = 5
            .Should().Equal(IntEnumSet.Of(Color.Red, Color.Blue));

    [Test]
    public static void Of_Single() =>
        IntEnumSet.Of(Color.Blue).Flags
            .Should().Be(1 << (int)Color.Blue); // that is 10b = 2

    [Test]
    public static void Of_Params() =>
        IntEnumSet.Of(Color.Red, Color.Blue).Flags
            .Should().Be((1 << (int)Color.Red) | (1 << (int)Color.Blue)); // that is 101b = 5

    [Test]
    public static void Of_Enumerable() =>
        IntEnumSet.Of(Enumerable.Empty<Color>().Append(Color.Red).Append(Color.Blue)).Flags
            .Should().Be((1 << (int)Color.Red) | (1 << (int)Color.Blue)); // that is 101b = 5

    [Test]
    public static void ToIntEnumSet() =>
        Enumerable.Empty<Color>().Append(Color.Red).Append(Color.Blue).ToIntEnumSet().Flags
            .Should().Be((1 << (int)Color.Red) | (1 << (int)Color.Blue)); // that is 101b = 5

    [TestCase(Color.TooSmall)]
    [TestCase(Color.TooBig)]
    public static void Of_OutOfRange(Color color)
    {
        var act = () => IntEnumSet.Of(color);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public static void Implicit_Empty() =>
        ((IntEnumSet<Color>)IntEnumSet.Empty).Any().Should().BeFalse();

    [Test]
    public static void Any_Empty() =>
        IntEnumSet.Of<Color>().Any().Should().BeFalse();

    [Test]
    public static void Any_NotEmpty() =>
        IntEnumSet.Of(Color.Red, Color.Green).Any().Should().BeTrue();

    [Test]
    public static void Contains_Containing() =>
        IntEnumSet.Of(Color.Red).Contains(Color.Red).Should().BeTrue();

    [Test]
    public static void Contains_NotContaining() =>
        IntEnumSet.Of(Color.Red).Contains(Color.Green).Should().BeFalse();

    [Test]
    public static void Count() =>
        IntEnumSet.Of(Color.Red, Color.Blue).Count.Should().Be(2);

    [Test]
    public static void Equals_Equal() =>
        IntEnumSet.Of(Color.Red, Color.Green)
            .Equals(IntEnumSet.Of(Color.Green, Color.Red))
            .Should().BeTrue();

    [Test]
    public static void Equals_NotEqual() =>
        IntEnumSet.Of(Color.Red, Color.Blue)
            .Equals(IntEnumSet.Of(Color.Red, Color.Green))
            .Should().BeFalse();

    [Test]
    public static void Enumerate() =>
        IntEnumSet.Of(Color.Red, Color.Blue)
            .Should().BeEquivalentTo(new[] { Color.Red, Color.Blue });

    [Test]
    public static void HashCode() =>
        IntEnumSet.Of(Color.Red, Color.Green).GetHashCode()
            .Should().Be(IntEnumSet.Of(Color.Red, Color.Green).GetHashCode());

    [Test]
    public static void StringRepresentation() =>
        IntEnumSet.Of(Color.Red, Color.Blue).ToString()
            .Should().Be("IntEnumSet.Of(Red, Blue)");

    [Test]
    public static void Intersect_WithEnumerable() =>
        IntEnumSet.Of(Color.Red, Color.Blue).Intersect(new[] { Color.Green, Color.Blue })
            .Should().Equal(IntEnumSet.Of(Color.Blue));

    [Test]
    public static void Intersect_WithEnumSet() =>
        IntEnumSet.Of(Color.Red, Color.Blue).Intersect(IntEnumSet.Of(Color.Green, Color.Blue))
            .Should().Equal(IntEnumSet.Of(Color.Blue));

    [Test]
    public static void Except_One() =>
        IntEnumSet.Of(Color.Red, Color.Blue).Except(Color.Red)
            .Should().Equal(IntEnumSet.Of(Color.Blue));

    [Test]
    public static void Except_Enumerable() =>
        IntEnumSet.Of(Color.Red, Color.Green, Color.Blue).Except(new[] { Color.Red, Color.Green })
            .Should().Equal(IntEnumSet.Of(Color.Blue));

    [Test]
    public static void Except_EnumSet() =>
        IntEnumSet.Of(Color.Red, Color.Green, Color.Blue).Except(IntEnumSet.Of(Color.Red, Color.Green))
            .Should().Equal(IntEnumSet.Of(Color.Blue));

    [Test]
    public static void Union_One() =>
        IntEnumSet.Of(Color.Red).Union(Color.Blue)
            .Should().Equal(IntEnumSet.Of(Color.Red, Color.Blue));

    [Test]
    public static void Union_WithEnumerable() =>
        IntEnumSet.Of(Color.Red, Color.Blue).Union(new[] { Color.Green, Color.Blue })
            .Should().Equal(IntEnumSet.Of(Color.Green, Color.Red, Color.Blue));

    [Test]
    public static void Union_WithEnumSet() =>
        IntEnumSet.Of(Color.Red, Color.Blue).Union(IntEnumSet.Of(Color.Green, Color.Blue))
            .Should().Equal(IntEnumSet.Of(Color.Green, Color.Red, Color.Blue));

    [Test]
    public static void IsProperSubsetOf_Subset() =>
        IntEnumSet.Of(Color.Red, Color.Blue).IsProperSubsetOf(new[] { Color.Red, Color.Red, Color.Blue, Color.Green })
            .Should().BeTrue();

    [Test]
    public static void IsProperSubsetOf_EnumSet() =>
        IntEnumSet.Of(Color.Red, Color.Blue).IsProperSubsetOf(IntEnumSet.Of(Color.Red, Color.Blue, Color.Green))
            .Should().BeTrue();

    [Test]
    public static void IsProperSubsetOf_NonStrictSubset() =>
        IntEnumSet.Of(Color.Red, Color.Blue).IsProperSubsetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeFalse();

    [Test]
    public static void IsProperSubsetOf_NotSubset() =>
        IntEnumSet.Of(Color.Red, Color.Green).IsProperSubsetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeFalse();

    [Test]
    public static void IsProperSupersetOf_Superset() =>
        IntEnumSet.Of(Color.Red, Color.Blue, Color.Green).IsProperSupersetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeTrue();

    [Test]
    public static void IsProperSupersetOf_EnumSet() =>
        IntEnumSet.Of(Color.Red, Color.Blue, Color.Green).IsProperSupersetOf(IntEnumSet.Of(Color.Red, Color.Blue))
            .Should().BeTrue();

    [Test]
    public static void IsProperSupersetOf_NonStrictSuperset() =>
        IntEnumSet.Of(Color.Red, Color.Blue).IsProperSupersetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeFalse();

    [Test]
    public static void IsProperSupersetOf_NotSuperset() =>
        IntEnumSet.Of(Color.Red, Color.Blue).IsProperSupersetOf(new[] { Color.Red, Color.Green })
            .Should().BeFalse();

    [Test]
    public static void IsSubsetOf_Subset() =>
        IntEnumSet.Of(Color.Red, Color.Blue).IsSubsetOf(new[] { Color.Red, Color.Red, Color.Blue, Color.Green })
            .Should().BeTrue();

    [Test]
    public static void IsSubsetOf_EnumSet() =>
        IntEnumSet.Of(Color.Red, Color.Blue).IsSubsetOf(IntEnumSet.Of(Color.Red, Color.Blue, Color.Green))
            .Should().BeTrue();

    [Test]
    public static void IsSubsetOf_NonStrictSubset() =>
        IntEnumSet.Of(Color.Red, Color.Blue).IsSubsetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeTrue();

    [Test]
    public static void IsSubsetOf_NotSubset() =>
        IntEnumSet.Of(Color.Red, Color.Green).IsSubsetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeFalse();

    [Test]
    public static void IsSupersetOf_Superset() =>
        IntEnumSet.Of(Color.Red, Color.Blue, Color.Green).IsSupersetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeTrue();

    [Test]
    public static void IsSupersetOf_EnumSet() =>
        IntEnumSet.Of(Color.Red, Color.Blue, Color.Green).IsSupersetOf(IntEnumSet.Of(Color.Red, Color.Blue))
            .Should().BeTrue();

    [Test]
    public static void IsSupersetOf_NonStrictSuperset() =>
        IntEnumSet.Of(Color.Red, Color.Blue).IsSupersetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeTrue();

    [Test]
    public static void IsSupersetOf_NotSuperset() =>
        IntEnumSet.Of(Color.Red, Color.Blue).IsSupersetOf(new[] { Color.Red, Color.Green })
            .Should().BeFalse();

    [Test]
    public static void Overlaps_Overlapping() =>
        IntEnumSet.Of(Color.Red, Color.Blue).Overlaps(new[] { Color.Red, Color.Green })
            .Should().BeTrue();

    [Test]
    public static void Overlaps_EnumSet() =>
        IntEnumSet.Of(Color.Red, Color.Blue).Overlaps(IntEnumSet.Of(Color.Red, Color.Green))
            .Should().BeTrue();

    [Test]
    public static void Overlaps_NotOverlapping() =>
        IntEnumSet.Of(Color.Blue).Overlaps(new[] { Color.Red, Color.Green })
            .Should().BeFalse();

    [Test]
    public static void SetEquals_Equal() =>
        IntEnumSet.Of(Color.Red, Color.Blue).SetEquals(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeTrue();

    [Test]
    public static void SetEquals_EnumSet() =>
        IntEnumSet.Of(Color.Red, Color.Blue).SetEquals(IntEnumSet.Of(Color.Red, Color.Blue))
            .Should().BeTrue();

    [Test]
    public static void SetEquals_Smaller() =>
        IntEnumSet.Of(Color.Red, Color.Blue).SetEquals(new[] { Color.Red, Color.Green, Color.Blue })
            .Should().BeFalse();

    [Test]
    public static void SetEquals_Larger() =>
        IntEnumSet.Of(Color.Red, Color.Green, Color.Blue).SetEquals(new[] { Color.Red, Color.Blue })
            .Should().BeFalse();
}