using UnityEngine;

namespace React.Fake
{
    [CreateAssetMenu(fileName = "object-container-global", menuName = "React/Object Container Global")]
    public class ObjectContainerGlobal : ScriptableObject
    {
        public ObjectContainer container = new ObjectContainer();
    }
}