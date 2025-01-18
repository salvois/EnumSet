/*
EnumSet - Immutable, efficient, small, equatable IReadOnlySet for C# enums

Copyright 2024-2025 Salvatore ISAJA. All rights reserved.

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

namespace EnumSet;

internal class IntEnumSetEnumerator<T> : IEnumerator<T> where T : Enum
{
    private readonly IntEnumSet<T> _enumSet;
    private uint _flags;
    private int _index;

    public IntEnumSetEnumerator(IntEnumSet<T> enumSet)
    {
        _enumSet = enumSet;
        _flags = _enumSet.Flags;
        _index = -1;
    }

    public bool MoveNext()
    {
        for (; _flags != 0; _index++)
        {
            var flag = 1u << _index;
            if ((_flags & flag) != 0)
            {
                _flags &= ~flag;
                return true;
            }
        }
        return false;
    }

    public void Reset()
    {
        _flags = _enumSet.Flags;
        _index = -1;
    }

    public T Current => (T)Enum.ToObject(typeof(T), _index);

    object IEnumerator.Current => Current;

    public void Dispose() { }
}