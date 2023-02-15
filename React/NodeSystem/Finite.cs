namespace React.Core.Internal
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Runtime.InteropServices;
    public class Palace
    {
        public List<int> _int = new List<int>();
        public List<float> _float = new List<float>();
        public List<double> _double = new List<double>();
        public List<string> _string = new List<string>();
        public List<bool> _bool = new List<bool>();

        public void call()
        {
        }
    }
    public class Struct
    {
        public virtual void createInstance() { }
        public virtual bool isValue => false;
        public bool isReference => !isValue;
    }
    public class Instance
    {
        public static void mov() { }
    }
    public enum StructName
    {
        none,
        String,
        Int,
        Float,
        Bool,
    }

    public struct Pointer
    {
        public int index;
    }

    public class Argument
    {
        public ArgumentMode mode;
        public StructName type;
        public Pointer defaultValue;
    }
    public enum ArgumentMode
    {
        input,
        output,
        refered,
    }
    public class Invokable
    {
        public List<Struct> args = new List<Struct>();

        public void invoke() { }
    }

    public class ArgsStack
    {

    }
    public class ArgsHeap
    {

    }
}