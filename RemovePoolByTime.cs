using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UnderHood;

public class RemovePoolByTime : MonoBehaviour, IPoolPassRef, IPoolCallback
{
    public float time = 1;
    public float duration = 0;
    public SuperPool pRef { get; set; }

    public void pool_actived()
    {
        SuperPool.removeByTime(pRef, time, duration);
    }

    public void pool_reset()
    {
        // throw new System.NotImplementedException();
    }

    public static IEnumerator removeSomething(SuperPool p, float offsetTime)
    {
        yield return new WaitForSeconds(offsetTime);
        SuperPool.remove(p);
    }
}