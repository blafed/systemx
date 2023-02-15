
using System.Collections;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Runtime;
using System.Runtime.CompilerServices;


public class SuperPool
{
    GameObject go;
    List<IPoolComponentBase> components;

    IEnumerator removeCoro;
    Row row;

    public GameObject gameObject => go;
    public Transform transform => go.transform;


    public bool isActive { get; private set; }

    class Row
    {
        public GameObject go;
        public int maxDepth = 0;
        public Queue<SuperPool> unused = new Queue<SuperPool>();
    }


    static GameObject _lastGO;
    static Row _lastRow;
    static Transform ROOT;
    static MonoBehaviour manager;
    static void __init()
    {
        if (ROOT)
            return;
        ROOT = new GameObject("POOL_ROOT").transform;
        manager = new GameObject("POOL_MANAGER").gameObject.AddComponent<UnityEngine.UnderHood.ObjectOfPoolRoot>();
        ROOT.gameObject.SetActive(false);
    }
    static Row register(GameObject go)
    {
        __init();
        go = GameObject.Instantiate(go);
        go.transform.parent = ROOT;
        Row row = new Row
        {
            go = go
        };
        rows.checkCapacityAdd(row, 10);
        return row;
    }
    static Row toPoolSource(GameObject go)
    {

        if (_lastGO == go)
            return _lastRow;
        _lastGO = go;
        foreach (var r in rows)
        {
            if (r.go == go)
            {
                _lastRow = r;
                return r;
            }
        }
        _lastRow = register(go);
        return _lastRow;
    }
    static SuperPool create(Row r)
    {
        var go = GameObject.Instantiate(r.go);

        SuperPool s = new SuperPool
        {
            row = r,
            go = go,
            components = new List<IPoolComponentBase>(go.GetComponentsInChildren<IPoolComponentBase>(true))

        };
        foreach (var c in s.components)
        {
            var x = c as IPoolCallbackInit;
            if (x != null)
                x.pool_init();
        }
        return s;
    }
    static void activate(SuperPool s)
    {
        if (s.isActive)
            return;
        s.transform.parent = null;
        s.isActive = true;
        foreach (var c in s.components)
        {
            var x2 = c as IPoolPassRef;
            if (x2 != null)
            {
                x2.pRef = s;
            }
            var x = c as IPoolCallback;
            if (x != null)
                x.pool_actived();
        }
    }

    static List<Row> rows = new List<Row>();
    public static void remove(SuperPool s)
    {
        if (!s.go)
        {
            Debug.LogWarning("Trying to remove destroyed pool ");
            return;
        }
        if (!s.isActive)
            return;
        cancelRemove(s);
        foreach (var c in s.components)
        {
            var x = c as IPoolCallback;
            if (x != null)
                x.pool_reset();
        }
        s.transform.parent = ROOT;
        s.isActive = false;
        s.row.unused.Enqueue(s);
    }
    public static SuperPool use(GameObject go)
    {
        var row = toPoolSource(go);
        SuperPool sp;
        if (row.unused.Count > 0)
        {
            sp = row.unused.Dequeue();
        }
        else
        {
            sp = create(row);
        }
        activate(sp);
        return sp;
    }
    public static void cancelRemove(SuperPool p)
    {
        if (p.removeCoro != null)
            manager.StopCoroutine(p.removeCoro);
        p.removeCoro = null;
    }
    public static void removeByTime(SuperPool p, float time, float duration = 0)
    {
        p.removeCoro = _removeCoro(p, time, duration);
        manager.StartCoroutine(p.removeCoro);

    }
    static IEnumerator _removeCoro(SuperPool s, float time, float duration)
    {
        float targetTime = Time.time + time;
        yield return new WaitUntil(() => Time.time > targetTime);
        duration = duration.limitMin(0);
        yield return manager.StartCoroutine(Extensions2.makeUpdate(MakeUpdateType.fixedUpdate, duration, t =>
        {
            foreach (var item in s.components)
            {
                var x = item as IPoolCustomRemove;
                if (x != null)
                    x.pool_remove(t);
            }
        }));
        s.removeCoro = null;
        remove(s);
    }
}


public interface IPoolComponentBase
{

}
public interface IPoolPassRef : IPoolComponentBase
{
    public SuperPool pRef { get; set; }
}
public interface IPoolCallback : IPoolComponentBase
{
    void pool_reset();
    void pool_actived();
}

public interface IPoolCallbackInit : IPoolComponentBase
{
    void pool_init();
}
public interface IPoolCustomRemove : IPoolComponentBase
{
    void pool_remove(float t);
}