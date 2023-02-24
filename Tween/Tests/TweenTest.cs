namespace Blafed.Tween.Tests
{
    using Testing;
    using UnityEngine;
    public class TweenTest : MonoBehaviour
    {
        public float duration = 5;
        public LoopType loopType;
        public EaseType2 easeType;

        private void Start()
        {
            new ActionTween(() => print("tween stopped")).stop();
            new ActionTween(() => print("tween"));
            new DurationTween(
                new LoopTween(new GeneralTween(f => transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.one * 100, f)),
                0, LoopType.yoyo), duration)
            ;
        }
        public void runPlay()
        {

        }
    }

}