namespace React.Core
{
    using UnityEngine;
    [System.Serializable]
    public class Hook
    {

        public HookType type;
        public Object obj;
        public string path;

        public Core.NodeSystem.Factory function;
    }

    public enum HookType
    {
        none,
        Start,
        picker,
        resources,
    }

    public class HookScript : MonoBehaviour
    {
        public virtual void execute() { }
    }
}