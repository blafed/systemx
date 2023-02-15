namespace React.JSParser
{
    public class Type
    {
        protected virtual TypeFlags flags { get; }
        public bool isNumber => flags.HasFlag(TypeFlags.Number);
        public bool isInt => flags.HasFlag(TypeFlags.Int);
        public bool isFloat => isNumber && !isInt;
        public virtual void convert(Type targetType, object origin, ref object target) { }
        public virtual object create() { return null; }

    }
    public class Type<T> : Type
    {

    }

    [System.Flags]
    public enum TypeFlags
    {
        none,
        Number = 1,
        Int = 2,
        String = 4,
        Array = 8,
        Map = 16,
        React = 32,
    }
}