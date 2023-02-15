using UnityEngine;
using System.Collections.Generic;
using System;
[System.Serializable]
public struct Nullable<T> where T : struct
{
    [SerializeField]
    bool _hasValue;
    [SerializeField]
    T _value;

    public bool hasValue => _hasValue;
    public T value => !_hasValue ? throw new("Has no value") : _value;

    public T? nullable => _hasValue ? null : _value;

    public Nullable(T value)
    {
        _hasValue = true;
        _value = value;
    }
    public Nullable(T? nullable)
    {
        _hasValue = nullable.HasValue;
        _value = _hasValue ? nullable.Value : default;
    }

    public static implicit operator Nullable<T>(T v) => new(v);
    public static implicit operator Nullable<T>(T? v) => new(v);
}