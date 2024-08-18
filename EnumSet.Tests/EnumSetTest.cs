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
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace EnumSet.Tests;

[TestFixture]
public static class EnumSetTest
{
    public enum Color
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        TooSmall = -1,
        TooBig = EnumSet.MaxValue + 1
    }

    [Test]
    public static void Flags() =>
        EnumSet.Of(Color.Red, Color.Blue).Flags
            .Should().Be((1 << (int)Color.Red) | (1 << (int)Color.Blue)); // that is 101b = 5

    [Test]
    public static void FromFlags() =>
        new EnumSet<Color>((1 << (int)Color.Red) | (1 << (int)Color.Blue)) // that is 101b = 5
            .Should().Equal(EnumSet.Of(Color.Red, Color.Blue));

    [Test]
    public static void Of_Single() =>
        EnumSet.Of(Color.Blue).Flags
            .Should().Be(1 << (int)Color.Blue); // that is 10b = 2

    [Test]
    public static void Of_Params() =>
        EnumSet.Of(Color.Red, Color.Blue).Flags
            .Should().Be((1 << (int)Color.Red) | (1 << (int)Color.Blue)); // that is 101b = 5

    [Test]
    public static void Of_Enumerable() =>
        EnumSet.Of(Enumerable.Empty<Color>().Append(Color.Red).Append(Color.Blue)).Flags
            .Should().Be((1 << (int)Color.Red) | (1 << (int)Color.Blue)); // that is 101b = 5

    [Test]
    public static void ToEnumSet() =>
        Enumerable.Empty<Color>().Append(Color.Red).Append(Color.Blue).ToEnumSet().Flags
            .Should().Be((1 << (int)Color.Red) | (1 << (int)Color.Blue)); // that is 101b = 5

    [TestCase(Color.TooSmall)]
    [TestCase(Color.TooBig)]
    public static void Of_OutOfRange(Color color)
    {
        var act = () => EnumSet.Of(color);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public static void Any_Empty() =>
        EnumSet.Empty<Color>().Any().Should().BeFalse();

    [Test]
    public static void Any_NotEmpty() =>
        EnumSet.Of(Color.Red, Color.Green).Any().Should().BeTrue();

    [Test]
    public static void Contains_Containing() =>
        EnumSet.Of(Color.Red).Contains(Color.Red).Should().BeTrue();

    [Test]
    public static void Contains_NotContaining() =>
        EnumSet.Of(Color.Red).Contains(Color.Green).Should().BeFalse();

    [Test]
    public static void Count() =>
        EnumSet.Of(Color.Red, Color.Blue).Count.Should().Be(2);

    [Test]
    public static void Equals_Equal() =>
        EnumSet.Of(Color.Red, Color.Green)
            .Equals(EnumSet.Of(Color.Green, Color.Red))
            .Should().BeTrue();

    [Test]
    public static void Equals_NotEqual() =>
        EnumSet.Of(Color.Red, Color.Blue)
            .Equals(EnumSet.Of(Color.Red, Color.Green))
            .Should().BeFalse();

    [Test]
    public static void Enumerate() =>
        EnumSet.Of(Color.Red, Color.Blue)
            .Should().BeEquivalentTo(new[] { Color.Red, Color.Blue });

    [Test]
    public static void HashCode() =>
        EnumSet.Of(Color.Red, Color.Green).GetHashCode()
            .Should().Be(EnumSet.Of(Color.Red, Color.Green).GetHashCode());

    [Test]
    public static void StringRepresentation() =>
        EnumSet.Of(Color.Red, Color.Blue).ToString()
            .Should().Be("EnumSet.Of(Red, Blue)");

    [Test]
    public static void Intersect_WithEnumerable() =>
        EnumSet.Of(Color.Red, Color.Blue).Intersect(new[] { Color.Green, Color.Blue })
            .Should().Equal(EnumSet.Of(Color.Blue));

    [Test]
    public static void Intersect_WithEnumSet() =>
        EnumSet.Of(Color.Red, Color.Blue).Intersect(EnumSet.Of(Color.Green, Color.Blue))
            .Should().Equal(EnumSet.Of(Color.Blue));

    [Test]
    public static void Remove_One() =>
        EnumSet.Of(Color.Red, Color.Blue).Remove(Color.Red)
            .Should().Equal(EnumSet.Of(Color.Blue));

    [Test]
    public static void Remove_Enumerable() =>
        EnumSet.Of(Color.Red, Color.Green, Color.Blue).Remove(new[] { Color.Red, Color.Green })
            .Should().Equal(EnumSet.Of(Color.Blue));

    [Test]
    public static void Remove_EnumSet() =>
        EnumSet.Of(Color.Red, Color.Green, Color.Blue).Remove(EnumSet.Of(Color.Red, Color.Green))
            .Should().Equal(EnumSet.Of(Color.Blue));

    [Test]
    public static void Union_One() =>
        EnumSet.Of(Color.Red).Union(Color.Blue)
            .Should().Equal(EnumSet.Of(Color.Red, Color.Blue));

    [Test]
    public static void Union_WithEnumerable() =>
        EnumSet.Of(Color.Red, Color.Blue).Union(new[] { Color.Green, Color.Blue })
            .Should().Equal(EnumSet.Of(Color.Green, Color.Red, Color.Blue));

    [Test]
    public static void Union_WithEnumSet() =>
        EnumSet.Of(Color.Red, Color.Blue).Union(EnumSet.Of(Color.Green, Color.Blue))
            .Should().Equal(EnumSet.Of(Color.Green, Color.Red, Color.Blue));

    [Test]
    public static void IsProperSubsetOf_Subset() =>
        EnumSet.Of(Color.Red, Color.Blue).IsProperSubsetOf(new[] { Color.Red, Color.Red, Color.Blue, Color.Green })
            .Should().BeTrue();

    [Test]
    public static void IsProperSubsetOf_EnumSet() =>
        EnumSet.Of(Color.Red, Color.Blue).IsProperSubsetOf(EnumSet.Of(Color.Red, Color.Blue, Color.Green))
            .Should().BeTrue();

    [Test]
    public static void IsProperSubsetOf_NonStrictSubset() =>
        EnumSet.Of(Color.Red, Color.Blue).IsProperSubsetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeFalse();

    [Test]
    public static void IsProperSubsetOf_NotSubset() =>
        EnumSet.Of(Color.Red, Color.Green).IsProperSubsetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeFalse();

    [Test]
    public static void IsProperSupersetOf_Superset() =>
        EnumSet.Of(Color.Red, Color.Blue, Color.Green).IsProperSupersetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeTrue();

    [Test]
    public static void IsProperSupersetOf_EnumSet() =>
        EnumSet.Of(Color.Red, Color.Blue, Color.Green).IsProperSupersetOf(EnumSet.Of(Color.Red, Color.Blue))
            .Should().BeTrue();

    [Test]
    public static void IsProperSupersetOf_NonStrictSuperset() =>
        EnumSet.Of(Color.Red, Color.Blue).IsProperSupersetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeFalse();

    [Test]
    public static void IsProperSupersetOf_NotSuperset() =>
        EnumSet.Of(Color.Red, Color.Blue).IsProperSupersetOf(new[] { Color.Red, Color.Green })
            .Should().BeFalse();

    [Test]
    public static void IsSubsetOf_Subset() =>
        EnumSet.Of(Color.Red, Color.Blue).IsSubsetOf(new[] { Color.Red, Color.Red, Color.Blue, Color.Green })
            .Should().BeTrue();

    [Test]
    public static void IsSubsetOf_EnumSet() =>
        EnumSet.Of(Color.Red, Color.Blue).IsSubsetOf(EnumSet.Of(Color.Red, Color.Blue, Color.Green))
            .Should().BeTrue();

    [Test]
    public static void IsSubsetOf_NonStrictSubset() =>
        EnumSet.Of(Color.Red, Color.Blue).IsSubsetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeTrue();

    [Test]
    public static void IsSubsetOf_NotSubset() =>
        EnumSet.Of(Color.Red, Color.Green).IsSubsetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeFalse();

    [Test]
    public static void IsSupersetOf_Superset() =>
        EnumSet.Of(Color.Red, Color.Blue, Color.Green).IsSupersetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeTrue();

    [Test]
    public static void IsSupersetOf_EnumSet() =>
        EnumSet.Of(Color.Red, Color.Blue, Color.Green).IsSupersetOf(EnumSet.Of(Color.Red, Color.Blue))
            .Should().BeTrue();

    [Test]
    public static void IsSupersetOf_NonStrictSuperset() =>
        EnumSet.Of(Color.Red, Color.Blue).IsSupersetOf(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeTrue();

    [Test]
    public static void IsSupersetOf_NotSuperset() =>
        EnumSet.Of(Color.Red, Color.Blue).IsSupersetOf(new[] { Color.Red, Color.Green })
            .Should().BeFalse();

    [Test]
    public static void Overlaps_Overlapping() =>
        EnumSet.Of(Color.Red, Color.Blue).Overlaps(new[] { Color.Red, Color.Green })
            .Should().BeTrue();

    [Test]
    public static void Overlaps_EnumSet() =>
        EnumSet.Of(Color.Red, Color.Blue).Overlaps(EnumSet.Of(Color.Red, Color.Green))
            .Should().BeTrue();

    [Test]
    public static void Overlaps_NotOverlapping() =>
        EnumSet.Of(Color.Blue).Overlaps(new[] { Color.Red, Color.Green })
            .Should().BeFalse();

    [Test]
    public static void SetEquals_Equal() =>
        EnumSet.Of(Color.Red, Color.Blue).SetEquals(new[] { Color.Red, Color.Red, Color.Blue })
            .Should().BeTrue();

    [Test]
    public static void SetEquals_EnumSet() =>
        EnumSet.Of(Color.Red, Color.Blue).SetEquals(EnumSet.Of(Color.Red, Color.Blue))
            .Should().BeTrue();

    [Test]
    public static void SetEquals_Smaller() =>
        EnumSet.Of(Color.Red, Color.Blue).SetEquals(new[] { Color.Red, Color.Green, Color.Blue })
            .Should().BeFalse();

    [Test]
    public static void SetEquals_Larger() =>
        EnumSet.Of(Color.Red, Color.Green, Color.Blue).SetEquals(new[] { Color.Red, Color.Blue })
            .Should().BeFalse();
}