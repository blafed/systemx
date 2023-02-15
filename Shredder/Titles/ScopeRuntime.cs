namespace Shredder.Titles
{
    using UnityEngine;

    public class ScopeArgs
    {
        public static ScopeMode mode;
        public static string fieldName;
        public static float _float;
        public static int _int;
        public static Vector2 _vector2;
        public static Vector3 _vector3;
        public static bool _bool;
        public static string _string;
        public static string[] fields;
    }

    public enum ScopeMode
    {
        fieldGet,
        fieldSet,
        listFields
    }
}