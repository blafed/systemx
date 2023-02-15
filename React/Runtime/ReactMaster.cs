namespace React
{
    using UnityEngine;

    public class ReactMaster : MonoBehaviour
    {
        public RFC rfc;
        public void render()
        {
            rfc.render(gameObject);
        }
        public void init() { }
        public void remove() { }

    }
}