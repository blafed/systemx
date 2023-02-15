using System.Collections.Generic;
namespace React.Core.NodeSystem
{


    [System.Serializable]
    public class Factory
    {
        public List<Node> nodes = new List<Node>();

        public virtual string toCode()
        {
            var s = "";
            foreach (var x in nodes)
            {

            }
            return s;
        }

        public void compile()
        {

        }
        public void execute() { }

        class CompiledNode
        {
            public Node node;
            public int index;

        }
    }
    public interface INodeCode
    {
        IEnumerable<INodeCode> children();
        IEnumerable<INodeCode> dependencies();
        string defines { get; }
    }

    public class RawValueNode : INodeCode
    {
        public string defines => throw new System.NotImplementedException();

        public IEnumerable<INodeCode> children()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<INodeCode> dependencies()
        {
            throw new System.NotImplementedException();
        }
    }
    public class CallNode : INodeCode
    {
        public INodeCode method;

        public string defines => throw new System.NotImplementedException();

        public IEnumerable<INodeCode> children()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<INodeCode> dependencies()
        {
            throw new System.NotImplementedException();
        }
    }
    public class LetNode : INodeCode
    {
        public string name;
        public INodeCode defaultValueNode;
        public INodeCode declareType;

        public string defines => name;

        public IEnumerable<INodeCode> children()
        {
            yield break;
        }

        public IEnumerable<INodeCode> dependencies()
        {
            yield return this.defaultValueNode;
            yield return this.declareType;
        }
    }
    public interface INode
    {
        void generateIndex();

    }
    [System.Serializable]
    public struct Node
    {
        public NodeRole role;
        public string main;
        public string extend;

    }
    public enum NodeRole
    {
        none,
        define,
        call,
        set,
        get,
        raw,
        begin,
        end,
        _if,
        _else,
        _in,
        _out,
        native,
    }
    public enum NodeType
    {

    }

}

namespace React.Core.NodeSystem
{
    public class NodeCode
    {
        public virtual string defines => "";
        public virtual IEnumerable<NodeCode> children() { yield break; }
        public virtual IEnumerable<NodeCode> dependencies() { yield break; }
        public virtual void interact(NodeCode other) { }
        public virtual string asPlain() { return ""; }
        public virtual void parse(string s) { }
        public override sealed string ToString()
        {
            return asPlain();
        }
    }


    namespace NodeCodes
    {
        public class Type : NodeCode
        {
            public string nativeFullName;
            public string name;
            public NodeCode parent;

        }
        public class Raw : NodeCode
        {

        }
        public class Define : NodeCode
        {
            public string name;
            public NodeCode defaultValueNode;
            public NodeCode declareType;

            public override string asPlain()
            {
                return $"let {name} = {defaultValueNode}";
            }
        }
    }
}