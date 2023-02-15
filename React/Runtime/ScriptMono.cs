using UnityEngine;

namespace React.Core
{
    public class ScriptMono : MonoBehaviour
    {
        public ReactMaster master => _master ? _master : _master = GetComponentInParent<ReactMaster>();
        ReactMaster _master;
        internal virtual void start()
        {

        }

        internal virtual void onRender() { }
    }
}