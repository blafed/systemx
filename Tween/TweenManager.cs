using UnityEngine;

namespace Blafed.Tween.Internal
{
    public class TweenManager : MonoBehaviour
    {
        public static TweenManager instance
        {
            get
            {
                if (!Application.isPlaying)
                    return null;
                if (_instance)
                    return _instance;
                return _instance = new GameObject(nameof(TweenManager)).AddComponent<TweenManager>();
            }
        }
        static TweenManager _instance;
        event System.Action onUpdate;
        event System.Action onFixedUpdate;


        private void Update()
        {

        }
        private void FixedUpdate()
        {

        }
    }
}