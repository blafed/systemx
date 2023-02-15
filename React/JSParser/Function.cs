using UnityEngine;
using System.Collections.Generic;
namespace React.JSParser.Function
{
    public class Function
    {
        public List<Function> children = new List<Function>();
        public virtual void getArgs(Argument[] arguments, int startCount) { }
        public virtual void execute() { }
    }
    public class Argument
    {
        public bool isInput => role == ArgumentRole.input;
        public bool isOutput => role == ArgumentRole.output;
        public ArgumentRole role;
    }
    public enum ArgumentRole
    {
        input,
        output,
    }

    public partial struct ArgumentStack { }
}