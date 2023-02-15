using UnityEngine;

[System.Serializable]
public struct OptionalRef<T> where T : class
{
    [SerializeField]
    bool _hasValue;
    [SerializeField]
    [Typed]
    [SerializeReference]
    T _value;

    public bool hasValue => _hasValue;
    public T value => !_hasValue ? throw new("Has no value") : _value;


    public OptionalRef(T value)
    {
        _hasValue = true;
        _value = value;
    }

    public static implicit operator OptionalRef<T>(T v) => new(v);

}