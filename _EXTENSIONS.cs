//TODO: PLEASE DON"T FORGET TO UPDATE THIS NUMBER BEFORE PUSH
//serial ver 5
using System;
using System.Collections.Generic;
using System.Collections;
//using Arsenal;
//using Lockstep.Data;
//using TrueSync;
using UnityEditor;
//using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using URandom = UnityEngine.Random;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public delegate void Prime<T>(ref T t);

namespace UnityEditor
{

}

public delegate ret FuncOut<ret, arg, Out>(arg x, out Out o);

public enum ThreeBool
{
    Null,
    False,
    True,
}

public enum AngleQuart
{
    first,
    second,
    third,
    Fourth,
}
public enum MakeUpdateType
{
    update,
    fixedUpdate,
    unsacaledTime,
}
// public enum ComponentsScope
// {
//     self, parent, children,
// }
public static partial class Extensions2
{
    const int COMPSCOPE_SELF = 0, COMPSCOPE_PARENT = 1, COMPSCOPE_CHILDREN = 2;
    const string INIXTAG = "inix";
    public const int PLAYER = 16;
    public const int ENEMY_DIS = 1;
    public const int TEAM_DIS = 2;
    public const float E = (float)System.Math.E;

    public static bool contains(this BoundingSphere b, Vector3 point)
    {
        return (b.position - point).sqrMagnitude < b.radius.squared();
    }
    public static Rect rectFromCenter(Vector2 center, Vector2 size) => new Rect(center - size * .5f, size);
    public static string capitalize(this string s)
    {
        switch (s)
        {
            case null: throw new ArgumentNullException();
            case "": throw new ArgumentException($"{nameof(s)} cannot be empty", nameof(s));
            default: return s[0].ToString().ToUpper() + s.Substring(1);
        }
    }
    public static float signOrZero(this float f) => f == 0 ? 0 : f > 0 ? 1 : -1;
    public static float sign(this float f) => f >= 0 ? 1 : -1;
    public static Vector2 getNormal(this Vector2 v) => new Vector2(-v.y, v.x);
    public static void iterate(this int count, Action<int> action)
    {
        for (int j = 0; j < count; j++)
        {
            action(j);
        }
    }
    public static void iterate(this Vector2Int count, Action<Vector2Int> action)
    {
        for (int i = 0; i < count.x; i++)
        {
            for (int j = 0; j < count.y; j++)
            {
                action(new Vector2Int(i, j));
            }
        }
    }
    public static Vector2 randomInsideRect => new Vector2(URandom.value - 0.5f, URandom.value - 0.5f);
    public static Vector2 randomInsideRect01 => new Vector2(URandom.value, URandom.value);
    public static bool randomBool => URandom.value > 0.5f;
    public static Vector2Int randomVectorIntInc => new(randomBool ? 1 : -1, randomBool ? 1 : -1);


    public static bool hasDuplicates<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (i == j)
                    continue;
                if (Equals(list[i], list[j]))
                    return true;
            }
        }
        return false;
    }
    public static bool hasFlag(this int i, int flag) => (i & flag) != 0;
    public static Vector2 randomPointInside(this Rect r) => r.pointByRatio(new Vector2(Random.value, Random.value));
    public static Vector2 pointByRatio(this Rect r, Vector2 ratio)
    {
        return r.position + ratio * r.size;
    }
    public static float aspectRatio(this Vector2 v) => v.x / v.y;
    public static Vector3 addX(this Vector3 v, float f) => v.setX(v.x + f);
    public static Vector3 addY(this Vector3 v, float f) => v.setY(v.y + f);
    public static Vector3 addZ(this Vector3 v, float f) => v.setZ(v.z + f);

    public static bool nullOrEmpty(this string str) => string.IsNullOrEmpty(str);
    public static float area(this Vector2 v) => v.x * v.y;
    public static T getRandom<T>(this IList<T> list) => list.Count == 0 ? default : list[Random.Range(0, list.Count)];
    public static T getRandom<T>(this IList<T> list, int maxExlusive) => list.Count == 0 ? default : list[Random.Range(0, maxExlusive)];
    public static T getClamped<T>(this IReadOnlyList<T> list, int index) => list[index.modNotNeg(list.Count)];
    public static void shuffleThis<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static T pop<T>(this IList<T> list)
    {
        var last = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return last;
    }
    public static T pop<T>(this IList<T> list, T fallback) => list.Count == 0 ? fallback : list.pop();

    public static int modNotNeg(this int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
    public static Vector2 multInverse(this Vector2 v) => new(1 / v.x, 1 / v.y);
    public static Vector2 abs(this Vector2 v) => new(v.x.abs(), v.y.abs());
    public static float highestAbs(this Vector2 v) => v.y.abs() < v.x.abs() ? v.x.abs() : v.y.abs();
    public static Vector2 rectNormalized(this Vector2 v) => v / v.highestAbs();
    public static Vector2 fromMinComponent(this Vector2 v)
    {
        var abs = v.abs();
        if (abs.x < abs.y)
            return new(v.x, v.x);
        else
            return new(v.y, v.y);
    }
    public static Vector2Int toInt(this Vector2 v) => new Vector2Int((int)v.x, (int)v.y);
    public static Vector3Int toInt(this Vector3 v) => new Vector3Int((int)v.x, (int)v.y);
    public static int area(this Vector2Int v) => v.x * v.y;
    public static bool inRange(this IList list, int index) => index >= 0 && index < list.Count;
    public static bool isRectInside(this Rect container, Rect rect, bool allCorners = false)
    {
        if (allCorners)
            return container.Contains(rect.min) && container.Contains(rect.max);
        return container.Contains(rect.min) || container.Contains(rect.max)
        || container.Contains(new Vector2(rect.min.x, rect.max.y))
        || container.Contains(new Vector2(rect.max.x, rect.min.y))
        ;

    }
    public static Vector2? rectTouchEdge(this Rect container, Rect rect)
    {
        if (!container.isRectInside(rect) || container.isRectInside(rect, true))
            return null;

        if (rect.Contains(container.min))
            return new Vector2(-1, -1);
        if (rect.Contains(container.max))
            return new Vector2(1, 1);
        if (rect.Contains(new Vector2(container.min.x, container.max.y)))
            return new Vector2(-1, 1);
        if (rect.Contains(new Vector2(container.max.x, container.min.y)))
            return new Vector2(1, -1);

        if (container.max.x.betw(rect.min.x, rect.max.x))
            return Vector2.right;
        if (container.min.x.betw(rect.min.x, rect.max.x))
            return Vector2.left;
        if (container.max.y.betw(rect.min.y, rect.max.y))
            return Vector2.up;
        if (container.min.y.betw(rect.min.y, rect.max.y))
            return Vector2.down;
        return null;

    }
    [System.Obsolete]
    public static bool isPointInside(this Rect r, Vector2 point)
    {
        return point.between(r.min, r.max);
    }
    public static Vector2 closestPoint(this Rect rect, Vector2 point)
    {

        var dir = point - rect.center;
        dir = dir.rectNormalized();
        return rect.center + rect.size * .5f * dir;
    }

    public static bool between(this Vector2 v, Vector2 min, Vector2 max) =>
 v.x.betw(min.x, max.x) && v.y.betw(min.y, max.y);



    public static Vector2 vector2(this float f) => Vector2.one * f;

    public static Vector3 vector(this float f) => Vector3.one * f;
    public static void iterateTwo<T, K>(this IReadOnlyList<T> a, IReadOnlyList<K> b, Action<T, K> action)
    {
        var min = Mathf.Min(a.Count, b.Count);
        for (int i = 0; i < min; i++)
        {
            action(a[i], b[i]);
        }
    }



    public static void iterate<T>(this IReadOnlyList<T> list, Action<T, int> a)
    {
        for (int i = 0; i < list.Count; i++)
        {
            a(list[i], i);
        }
    }
    public static Vector2 swap(this Vector2 v)
    {
        return new Vector2(v.y, v.x);
    }
    public static object inMake(this MakeUpdateType m, ref float t)
    {
        object y = m == MakeUpdateType.update ? null : new WaitForFixedUpdate();
        float dt = m == MakeUpdateType.update ? Time.deltaTime : m == MakeUpdateType.unsacaledTime ? Time.unscaledDeltaTime : Time.fixedDeltaTime;
        if (m == MakeUpdateType.unsacaledTime)
            y = new WaitForSecondsRealtime(dt);
        t += dt;
        return y;
    }
    public static void changeLayer(this GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform t in go.transform)
        {
            changeLayer(t.gameObject, layer);
        }

    }
    public static void forEach<T>(this IEnumerable<T> t, Action<T> act)
    {
        foreach (var x in t)
        {
            act(x);
        }
    }
    public static T[] getComponentCachedAll<T>(this Component c, ref T[] t, int scope = 0)
    {
        if (t == null)
        {
            if (scope == COMPSCOPE_SELF)
                t = c.GetComponents<T>();
            else if (scope == COMPSCOPE_CHILDREN)
                t = c.GetComponentsInChildren<T>();
            else if (scope == COMPSCOPE_PARENT)
                t = c.GetComponentsInParent<T>();
        }
        return t;
    }
    public static bool hasFlag(this byte b, int f) => (b & f) != 0;
    public static void orient(this SuperPool s, Transform t, bool makeParent = false) => s.transform.orient(t, makeParent);
    public static Vector3 toVector(this SnapAxis s, bool normalized = false)
    {
        var v = Vector3.zero;
        if (s.HasFlag(SnapAxis.X)) v.x = 1;
        if (s.HasFlag(SnapAxis.Y)) v.y = 1;
        if (s.HasFlag(SnapAxis.Z)) v.z = 1;
        if (normalized) v = v.normalized;
        return v;
    }
    public static float limitMin(this float f, float limit)
    {
        return f.max(limit);
    }
    public static float limitMax(this float f, float limit)
    {
        return f.min(limit);
    }
    public static Vector3 randomPointInside(this Bounds b) => b.center + Vector3.Scale(b.size, Random.insideUnitSphere);
    public static Bounds scaleBounds(this Bounds a, Bounds b)
    {
        return new Bounds(a.center + b.center, Vector3.Scale(a.size, b.size));
    }
    public static Bounds getBoundsWorld(this Transform t) => new Bounds(t.position, t.lossyScale);
    public static bool isIn(this TouchPhase p) => p.isStay() || p == TouchPhase.Began;
    public static T getOrAdd<T>(this Component c) where T : Component
    {
        var g = c.GetComponent<T>();
        if (!g)
            return c.gameObject.AddComponent<T>();
        return g;
    }
    public static T safeAccess<T>(this IList<T> array, int index, T defaultValue = default(T))
    {
        if (array != null)
        {
            T o;
            if (array.tryGetElement(index, out o))
                return o;
        }
        return defaultValue;
    }
    public static bool tryGetElement<T>(this IList<T> array, int index, out T element)
    {

        if (index < array.Count && index >= 0)
        {
            element = array[index];
            return true;
        }
        element = default(T);
        return false;
    }
    public static bool hasValue(this ThreeBool b) => b != 0;
    public static bool toBool(this ThreeBool b) => b != ThreeBool.Null && b != ThreeBool.False;
    public static bool isFalse(this ThreeBool b) => b == ThreeBool.False;
    public static bool isTrue(this ThreeBool b) => b.toBool();
    public static bool nan(this float f) => float.IsNaN(f);
    public static int asFlag(this int i) => 1 << i;
    public static short addFlag(this short s, int f)
    {
        return (short)(s | (short)f);
    }
    public static bool hasFlag(this short s, int f)
    {
        return (s & f) != 0;
    }
    public static short removeFlag(this short s, int f)
    {
        return (short)(s & ~f);
    }

    // public static int abs(this int x) => Mathf.Abs(x);

    public static Vector3 toDirOne(this Vector3 v) => v.abs().maxComp().approx(0) ? Vector3.zero : v / v.abs().maxComp();
    public static Vector2 toVector2(this Vector2Int v) => (Vector2)v;
    public static TouchPhase GetTouchPhase(int variation = 0)
    {
        Vector2 pos;
        return GetTouchPhase(0, out pos);
    }
    public static TouchPhase GetTouchPhase(int variation, out Vector2 pos)
    {
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > variation)
            {
                var touch = Input.GetTouch(variation);
                pos = touch.position;
                return touch.phase;
            }
            pos = new Vector2();
            return TouchPhase.Canceled;
        }

        pos = Input.mousePosition;
        if (Input.GetMouseButtonDown(variation))
            return TouchPhase.Began;
        if (Input.GetMouseButtonUp(variation))
            return TouchPhase.Ended;
        if (Input.GetMouseButton(variation))
            return TouchPhase.Moved;
        return TouchPhase.Canceled;
    }
    public static IEnumerator makeUpdate(this MakeUpdateType type, float period, Action<float> action, EaseType2 e = EaseType2.Linear)
    {
        float t = 0;
        object y = type == MakeUpdateType.update ? null : new WaitForFixedUpdate();
        while (t <= period)
        {
            float dt = type == MakeUpdateType.update ? Time.deltaTime : type == MakeUpdateType.unsacaledTime ? Time.unscaledDeltaTime : Time.fixedDeltaTime;
            if (type == MakeUpdateType.unsacaledTime)
                y = new WaitForSecondsRealtime(dt);
            float p = e.evaluate(t / period).clamp01();
            action(p);
            t += dt;
            yield return y;
            if (p >= 1)
                break;
        }
    }
    public static int signOrZero(this int i) => i == 0 ? 0 : i > 0 ? 1 : -1;
    public static Vector2Int size(this Texture2D tex) => new Vector2Int(tex.width, tex.height);
    public static int ceil(this float f) => Mathf.CeilToInt(f);
    public static int floor(this float f) => Mathf.FloorToInt(f);

    public static Vector2 dir(this Vector2 v)
    {
        return v * (1f / v.max().abs());
    }
    public static Vector2Int floor(this Vector2 v)
    {
        return new Vector2Int(v.x.floor(), v.y.floor());
    }
    public static Vector2Int roundToInt(this Vector2 v)
    {
        return new Vector2Int(v.x.roundToInt(), v.y.roundToInt());
    }
    public static Vector2Int clampToUnity(this Vector2Int v)
    {
        if (v.x > 0) v.x = 1;
        if (v.x < 0) v.x = -1;
        if (v.y > 0) v.y = 1;
        if (v.y < 0) v.y = -1;

        return v;
    }
    public static int sumAbsParts(this Vector2Int v) => v.x.abs() + v.y.abs();
    public static Vector2Int clampEx(this Vector2Int v, Vector2Int min, Vector2Int max)
    {

        v.Clamp(min, max - Vector2Int.one);
        return v;
    }
    public static int min(this int a, int b)
    {
        return Mathf.Min(a, b);
    }

    public static int max(this int a, int b) => Mathf.Max(a, b);

    public static Vector2Int max(this Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x.max(b.x), a.y.max(b.y));
    }
    public static Vector2Int min(this Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x.min(b.x), a.y.min(b.y));
    }

    public static bool toBool(this byte b)
    {
        return b != 0;
    }
    public static float negaToBaka(this float f)
    {
        return (f * 0.5f) + 0.5f;
    }

    public static float bakaToNega(this float f)
    {
        return (f - 0.5f) * 2;
    }



    public static T[,] createSub<T>(this T[,] grid, Vector2Int pos, Vector2Int size)
    {
        var maxSize = size = grid.size() - pos;
        size.Clamp(size, maxSize);
        T[,] newArea = new T[size.x, size.y];
        newArea.iterate((x, i) =>
        {
            newArea.set(i, grid.get(pos + i));
        });
        return newArea;
    }
    public static bool isMohit<T>(this T[,] grid, Vector2Int v)
    {
        var size = grid.size();
        return v.x == 0 || v.y == 0 || v.x == size.x - 1 || v.y == size.y - 1;
    }

    public static void iterateMohit<T>(this T[,] grid, Action<T, Vector2Int> m, Vector2Int? _start = null, Vector2Int? _size = null)
    {
        var size = _size.HasValue ? _size.Value : grid.size();
        var start = _start.HasValue ? _start.Value : Vector2Int.zero;
        Vector2Int v = start;
        int dir = 0;
        bool dir1 = false;

        for (int i = 0; i < size.x * size.y; i++)
        {
            var g = grid.get(v);
            if (g == null)
                continue;


            if (!dir1)
            {
                if (v.x < size.x)
                {
                    v.x++;
                    m(g, v);
                }
                else if (v.y < size.y)
                {

                    v.y++;
                    m(g, v);
                }
                else
                {
                    dir1 = true;
                }
            }
            if (dir1)
            {
                if (v.x > start.x)
                {
                    v.x--;
                    m(g, v);
                }
                else if (v.y > start.y)
                {
                    v.y--;
                    m(g, v);
                }
                else
                {
                    break;
                }
            }
        }
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var x = grid[i, j];
                var n = new Vector2Int(i, j);
                m(x, n);
            }
        }
    }
    public static void iterate<T>(this T[,] array, Action<T, Vector2Int> m)
    {
        var s = array.size();

        for (int i = 0; i < s.x; i++)
        {
            for (int j = 0; j < s.y; j++)
            {
                var n = new Vector2Int(i, j);
                m(array.get(n), n);
            }
        }
    }
    public static void iterateBlock<T>(this T[,] array, Vector2Int start, Vector2Int size, Action<T, Vector2Int> m)
    {

        var s = size;
        //var offset = s + start;

        for (int i = 0; i < s.x; i++)
        {
            for (int j = 0; j < s.y; j++)
            {
                var ni = new Vector2Int(i, j);

                var n = ni + start;
                if (!array.contains(n))
                    continue;

                m(array.get(n), ni);
            }
        }
    }


    public static bool contains<T>(this T[,] array, Vector2Int v)
    {
        var s = array.size();
        return v.x >= 0 & v.x < s.x
                     && v.y >= 0 && v.y < s.y;
    }

    public static T[,] toArray<T>(this Vector2Int v) => new T[v.x, v.y];
    public static void set<T>(this T[,] array, Vector2Int i, T va) => array[i.x, i.y] = va;
    public static T get<T>(this T[,] grid, Vector2Int i) => grid[i.x, i.y];
    public static Vector2Int size<T>(this T[,] grid) =>
            new Vector2Int(grid.GetLength(0), grid.GetLength(1));
    public static Vector3 centerContactPoint(this Collision collision)
    {
        var totalPoint = new Vector3();
        var count = collision.contactCount;
        for (int i = 0; i < count; i++)
        {
            var con = collision.GetContact(i).point;
            totalPoint += con;
        }
        totalPoint /= count;
        return totalPoint;
    }
    public static bool checkIndex<T>(this IList<T> array, int index)
    {
        if (array == null)
            return false;
        return index < array.Count && index >= 0;
    }

    public static void writeToArray(this Vector3 v, IList<float> f, int startAt = 0, int length = 3)
    {
        for (int i = 0; i < length; i++)
        {
            var j = startAt + i;
            if (j < f.Count)
                f[j] = v[i];
        }
    }
    public static void writeToArray(this Vector2 v, IList<float> f, int startAt = 0, int length = 2)
    {
        for (int i = 0; i < length; i++)
        {
            var j = startAt + i;
            if (j < f.Count)
                f[j] = v[i];
        }
    }
    public static Vector2 toVector2(this IList<float> f, int startAt = 0)
    {
        if (f == null || f.Count == 0)
            return Vector2.zero;
        int len = f.Count;
        Vector2 v = new Vector2();
        v.x = len > startAt ? f[startAt] : 0;
        v.y = len > startAt + 1 ? f[startAt + 1] : 0;

        return v;
    }
    public static Vector3 toVector3(this IList<float> f, int startAt = 0)
    {
        if (f == null || f.Count == 0)
            return Vector3.zero;
        int len = f.Count;
        Vector3 v = new Vector3();
        v.x = len > startAt ? f[startAt] : 0;
        v.y = len > startAt + 1 ? f[startAt + 1] : 0;
        v.z = len > startAt + 2 ? f[startAt + 2] : 0;

        return v;
    }
    public static float[] toFloatArray(this Vector2 v)
    {
        return new[] { v.x, v.y };
    }
    public static float[] toFloatArray(this Vector3 v)
    {
        return new[] { v.x, v.y, v.z };
    }
    public static int sign(this int x) => x >= 0 ? 1 : -1;
    public static int abs(this int x) => x < 0 ? -x : x;

    public static float pingPongFactor(this float t, float totalPeriodLength)
    {
        bool b;
        return pingPongFactor(t, totalPeriodLength, out b);
    }
    /// <summary>
    /// Returns Value from 0 to 1 as ping-pong 
    /// </summary>
    /// <param name="t">the time [0, infinity]</param>
    /// <param name="totalPeriodLength">specified period length in seconds</param>
    /// <param name="isReverse">returns true if the output decreasing as (t) increasing, false if the output increasing</param>
    /// <returns></returns>
    public static float pingPongFactor(this float t, float totalPeriodLength, out bool isReverse)
    {
        var halfPeriod = totalPeriodLength * .5f;
        if (t <= halfPeriod)
        {
            isReverse = false;
            return (t / halfPeriod).clamp01();
        }
        else
        {
            isReverse = true;
            return (t - halfPeriod) / halfPeriod;
        }
    }
    public static float clampLength(this float f, float maxLength)
    {
        var abs = f.abs();
        var sign = f / abs;
        var m = abs.clamp(0, maxLength);
        return m * sign;


    }
    /// <summary>
    /// Has no output, The output of summation of correpsonding a,b indexes will sent to callback
    /// </summary>
    /// <param name="a">array 1</param>
    /// <param name="b">array 2</param>
    /// <param name="callback">(index, sum_result) use this to handle the sum_result value </param>
    public static void sumTo(this float[] a, float[] b, Action<int, float> callback)
    {
        //var callbackIsNull = callback == null;
        for (int i = 0; i < a.Length; i++)
        {
            if (i >= b.Length)
                break;
            // if(!callbackIsNull)
            callback(i, a[i] + b[i]);
        }
    }

    public static void multTo(this float[] a, float[] b, Action<int, float> func)
    {
        for (int i = 0; i < a.Length; i++)
        {
            if (i >= b.Length)
                break;
            func(i, a[i] + b[i]);
        }
    }

    public static Vector3 getVector3(this float[] array, int startIndex)
    {
        Vector3 v = new Vector3();
        for (int i = 0; i < 3; i++)
        {
            var j = i + startIndex;
            if (j < array.Length)
            {
                v[i] = array[j];
            }
        }

        return v;
    }
    public static void setVector2(this float[] array, int startIndex, Vector2 v)
    {
        for (int i = 0; i < 3; i++)
        {
            var j = i + startIndex;
            if (j < array.Length)
            {
                array[j] = v[i];
            }
        }
    }
    public static void setVector3(this float[] array, int startIndex, Vector3 v)
    {
        for (int i = 0; i < 3; i++)
        {
            var j = i + startIndex;
            if (j >= array.Length)
            {
                array[j] = v[i];
            }
        }
    }
    public static byte[] SerializeToByteArray(object obj)
    {
        if (obj == null)
        {
            return null;
        }
        var bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static T Deserialize<T>(this byte[] byteArray) where T : class
    {
        if (byteArray == null)
        {
            return null;
        }
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(byteArray, 0, byteArray.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = (T)binForm.Deserialize(memStream);
            return obj;
        }
    }
    public static IEnumerable<AngleQuart> IterateRegion_AngleQuarts(AngleQuart from, AngleQuart to)
    {
        if (from <= to)
        {
            for (int i = (int)from; i <= (int)to; i++)
            {
                yield return (AngleQuart)i;
            }
        }
        else
        {
            for (int i = (int)from; i < 4; i++)
            {
                yield return (AngleQuart)i;
            }

            for (int i = 0; i <= (int)to; i++)
            {
                yield return (AngleQuart)i;
            }
        }

    }
    public static float maxF(this AngleQuart a) => a.minF() + 90;
    public static float clamp360f(this float f) => Mathf.Clamp(f, 0, 360);

    public static void prefixFDT() => Time.fixedDeltaTime = 0.02f * Time.timeScale;
    public static bool betw(this float f, float min, float max) => f >= min & f <= max;
    public static bool between360(this float f) => f.betw(0, 360);

    /// <summary>
    /// exclusive between min [include] max [exlide]
    /// </summary>
    /// <param name="f"></param>
    /// <param name="min"></param>
    /// <param name="maxEX"></param>
    /// <returns></returns>
    public static bool betwEx(this float f, float min, float maxEX) => f >= min && f < maxEX;
    public static bool _betweenAngle360(this float f, AngleQuart from, AngleQuart to)
    {
        if (from <= to)
            return f.betwEx(from.minF(), to.maxF());
        else
        {

            var fA = from;
            var fB = to;
            foreach (var x in IterateRegion_AngleQuarts(fA, fB))
            {
                if (x.isWithinQuart(f))
                    return true;
            }
        }

        return false;

    }
    [System.Obsolete]
    public static bool _betweenAngle360(this float f, float from, float to)
    {
        if (!from.between360())
            throw new Exception(from.ToString());
        if (!to.between360())
            throw new Exception(to.ToString());

        if (from <= to)
            return f.betw(from, to);
        else
        {

            var fA = from.getAngleQuart();
            var fB = to.getAngleQuart();
            foreach (var x in IterateRegion_AngleQuarts(fA, fB))
            {
                if (x.isWithinQuart(f))
                    return true;
            }
        }

        return false;

    }
    public static float closely90Angle(this float f)
    {
        return f - f.getAngleQuart().minF();
    }
    public static AngleQuart clamp(this AngleQuart a) => ((AngleQuart)((int)a % 4));
    public static AngleQuart increaseByOne(this AngleQuart a)
    {
        return (++a).clamp();
    }
    public static float minF(this AngleQuart a)
    {
        switch (a)
        {
            case AngleQuart.first: return 0;
            case AngleQuart.second: return 90;
            case AngleQuart.third: return 180;
            case AngleQuart.Fourth: return 270;
        }

        throw new Exception();
    }
    public static AngleQuart inverse(this AngleQuart a)
    {
        switch (a)
        {
            case AngleQuart.third: return AngleQuart.first;
            case AngleQuart.second: return AngleQuart.Fourth;
            case AngleQuart.Fourth:
                return AngleQuart.second;
            case AngleQuart.first:
                return AngleQuart.third;
        }

        throw new Exception();
    }
    public static bool isWithinQuart(this AngleQuart a, float x360)
    {
        try
        {
            return x360.getAngleQuart() == a;
        }
        catch (Exception e)
        {
            throw e;
        }

        /*switch (a)
        {
                case AngleQuart.first:
                        return x360 >= 0 && x360 < 90;
                case AngleQuart.second:
                        return x360 >= 90 && x360 < 180;
                case AngleQuart.fourth:
                        return x360 >= 270 && x360 < 360;
                        case AngleQuart.third:
                                return x360 >= 180 && x360 < 270;
        }

        throw new Exception("unknown angle quart " + a);*/
    }
    public static AngleQuart getAngleQuart(this float x360)
    {
        if (x360 >= 0 && x360 < 90) return AngleQuart.first;
        if (x360 >= 90 && x360 < 180) return AngleQuart.second;
        if (x360 >= 180 && x360 < 270) return AngleQuart.third;
        if (x360 >= 270 && x360 < 360) return AngleQuart.Fourth;
        if (x360.approx(360f)) return AngleQuart.first;
        throw new Exception("angle out " + x360);
    }

    public static float angleInverse360(this float f)
    {
        var q = f.getAngleQuart();
        switch (q)
        {
            case AngleQuart.first:
            case AngleQuart.second:
                f += 180;
                break;
            case AngleQuart.Fourth:
            case AngleQuart.third:
                f -= 180;
                break;
        }

        return f;
    }
    public static float atan2AngleTo360(this float atanAngle)
    {
        if (atanAngle < 0)
            return 360 + atanAngle;
        return atanAngle;
    }


    public static bool notNullOrEmpty(this string str)
    {
        return !string.IsNullOrEmpty(str);
    }

    public static bool equalElemnts<T>(this T[] a, T[] b)
    {
        if (a.Length != b.Length)
            return false;
        for (int i = 0; i < a.Length; i++)
        {
            var x = a[i];
            var y = b[i];
            if (!Equals(x, y
            ))
                return false;
        }

        return true;
    }
    public static byte[] ComputeSha256Hash(this byte[] bytes)
    {
        // Create a SHA256   
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            //      byte[]
            bytes = sha256Hash.ComputeHash(bytes);

            // Convert byte array to a string   
            // StringBuilder builder = new StringBuilder();  
            return bytes;
        }
    }
    public static byte[] EncryptStringToBytes_Aes(this byte[] plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        byte[] encrypted;

        // Create an AesManaged object
        // with the specified key and IV.
        using (AesManaged aesAlg = new AesManaged())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    public static byte[] DecryptStringFromBytes_Aes(this byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an AesManaged object
        // with the specified key and IV.
        byte[] bytes;
        using (AesManaged aesAlg = new AesManaged())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {

                    bytes = new byte[csDecrypt.Length];
                    csDecrypt.Read(bytes, 0, (int)csDecrypt.Length);
                    ///using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    //{

                    // Read the decrypted bytes from the decrypting stream
                    // and place them in a string.
                    // plaintext = srDecrypt.ReadToEnd();

                    //bytes = srDecrypt.Read()
                    //}
                }
            }
        }

        return
                bytes;
        //ASCIIEncoding.ASCII.GetBytes(plaintext);
    }
    public static float directionReverse(this bool b) => b ? -1f : 1f;
    public static bool toggle(this GameObject go)
    {
        var b = !go.activeSelf;
        go.SetActive(b);
        return b;
    }
    public static void myActive(this GameObject go)
    {
        go.SetActive(false);
        go.SetActive(true);
    }
    public static Vector2 clamp(this Vector2 v, Vector2 min, Vector2 max)
    {
        v.x = v.x.clamp(min.x, max.x);
        v.y = v.y.clamp(min.y, max.y);
        return v;
    }
    public static Rect MinMaxRect(Vector2 min, Vector2 max)
    {
        Rect r = new Rect();
        r.min = min;
        r.max = max;
        return r;
    }
    public static Rect clamp(this Rect r, Rect container)
    {
        var min = r.min;
        var max = r.max;

        var from = container.min;
        var to = container.max;

        min = min.clamp(from, to);
        max = max.clamp(from, to);


        return MinMaxRect(min, max);
    }

    public static void reverse(ref bool b) => b = !b;


    public static int sign(this bool b) => b ? 1 : -1;
    public static float evaluate(this EaseType2 x, float t)
    {
        t = t.clamp01();
        if (x == 0)
            return t;
        return Easing2.GetEase(x, 0, 1, t);
    }
    /*
    public static Vector2 getTouchPosition()
    {
            if (Input.touchCount > 0)
            {
                    return Input.touches[0].position;
            }

            return new Vector2();
    }*/
    public static bool isStay(this TouchPhase p)
    {
        return p == TouchPhase.Stationary | p == TouchPhase.Moved;
    }
    public static Vector2 getMousePosition()
    {
        if (Input.touchCount > 0)
        {
            return Input.touches[0].position;
        }

        return Input.mousePosition;
    }
    public static bool isFin(this WWW ww)
    {
        return ww.isDone && string.IsNullOrEmpty(ww.error);
    }
    public static Transform Find(this Transform transform, string name, bool includeInActive)
    {
        foreach (Transform t in transform)
        {

            if (includeInActive || t.gameObject.activeSelf)
            {
                if (t.name == name)
                    return t;
            }
        }

        return null;
    }

    public static float lerp(this Vector2 v, float t) => Mathf.Lerp(v.x, v.y, t);
    public static Camera mainCamera => _mainCamera ? _mainCamera : _mainCamera = Camera.main;
    private static Camera _mainCamera;
    public static byte toByte(this bool b) => (byte)b.toInt();
    public static bool toBool(this int i) => i != 0;
    public static int toInt(this bool b) => b ? 1 : 0;
    public static
            void _prefixCollider(this CapsuleCollider2D col, Vector2 newScale)
    {
        //      int i;i.toBool()
        if (newScale.x > newScale.y)
            col.direction = CapsuleDirection2D.Horizontal;
        else
        {
            col.direction = CapsuleDirection2D.Vertical;
        }

        col.transform.localScale = newScale;
    }

    //   public static Vector3 clamp()
    public static float cos(this float x, float frequency, float ampl = 1)
    {
        return Mathf.Cos(x * frequency * Mathf.PI) * ampl;
    }

    public static float sin(this float x, float frequency, float ampl = 1)
    {
        return Mathf.Sin(x * frequency * Mathf.PI) * ampl;
    }


    public static Rect orthoGraphicCameraWorld(this Camera c)
    {
        Rect r = new Rect();
        r.center = c.transform.position;
        r.size
                = 2 * Extensions2.screenAspectRatio * c.orthographicSize;
        return r;
    }
    public static Vector2 screenVector => new Vector2(Screen.width, Screen.height);
    [System.Obsolete]
    public static Vector2 screenRatio => Vector2.Scale(screenVector.maxVector().inverse(), screenVector);
    public static Vector2 screenAspectRatio => new Vector2(screenVector.x / screenVector.y, 1);

    public static Vector2 mult(this Vector2 v, Vector2 o)
    {
        return Vector2.Scale(v, o);
    }
    public static Vector2 minRatioVector(this Vector2 v)
    {
        var h = v.x;
        if (v.y < v.x)
            h = v.y;
        return v / h;
    }
    public static Vector2 maxRatioVector(this Vector2 v)
    {
        var h = v.x;
        if (v.y > v.x)
            h = v.y;
        return v / h;
    }
    public static Vector2 ratio(this Vector2 v)
    {
        return Vector2.Scale(v, v.maxVector().inverse());
    }
    public static Vector2 maxVector(this Vector2 v) => new Vector2(v.max(), v.max());
    public static Vector2 minVector(this Vector2 v) => new Vector2(v.min(), v.min());

    public static float max(this Vector2 v) => v.x >= v.y ? v.x : v.y;
    public static float min(this Vector2 v) => v.x <= v.y ? v.x : v.y;

    public static Vector3 getLocalComplex(this Transform t) => t.localPosition.setZ(t.localEulerAngles.z);

    public static void setLocalComplex(this Transform t, Vector3 v)
    {
        t.localPosition = (Vector2)v;
        t.localEulerAngles = t.localEulerAngles.setZ(v.x);

    }
    public static Transform cachedChild(this Transform t, int index, ref Transform cache)
    {
        if (cache)
            return cache;
        return cache = t.GetChild(index);
    }
    public static Transform cachedChild(this Transform t, string name, ref Transform cache)
    {
        if (cache)
            return cache;
        return cache = t.Find(name);
    }
    public static Transform cachedChild(this Transform t, string name, ref Transform cache, ref bool notExist)
    {
        if (cache || notExist)
            return cache;
        cache = t.Find(name);
        if (!cache)
            notExist = true;
        return cache;
    }

    public static Color copyAlpha(this Color c, Color other)
    {
        return c.alpha(other.a);
    }

    public static Color multiply(this Color c, Color o)
    {
        return new Color(
    c.r * o.r,
    c.g * o.g,
    c.b * o.b,
    c.a * o.a
        );
    }
    public static Color alpha(this Color c, float f)
    {
        return new Color(c.r, c.g, c.b, f);
    }

    public static Vector3 flat(this Vector3 v, float y = 0)
    {
        return new Vector3(v.x, y, v.z);
    }
    public static Vector3 mPosChecked(this Transform tr, Vector3 point) => tr ? tr.position : point;

    public static T getOrDefault<K, T>(this Dictionary<K, T> d, K key, T toAdd = default(T))
    {
        T t;
        if (!d.TryGetValue(key, out t))
        {

            return toAdd;
        }
        return t;
    }
    public static T getOrAddNew<K, T>(this Dictionary<K, T> d, K key, T toAdd = default(T))
    {
        T t;
        if (!d.TryGetValue(key, out t))
        {

            t = toAdd;
            d.Add(key, t);
        }
        return t;
    }
    public static float sqrt(this float x) => Mathf.Sqrt(x);
    public static float clamp01(this float c) => Mathf.Clamp01(c);
    /// <summary>
    /// 0.5 -> 0.35 | 0.9 -> 0.9 | 0.1 -> 0.05 
    /// </summary>
    /// <param name="t01"></param>
    /// <returns></returns>
    public static float smoothExponentialPeriod(this float t01)
    {

        var pow = Mathf.Exp(t01 * E * 0.5f) - 1;
        return pow / E;
    }
    public static K[] cast<T, K>(this T[] from, Func<T, K> caster)
    {
        var k = new K[from.Length];
        for (int i = 0; i < from.Length; i++)
        {
            k[i] = caster(from[i]);
        }
        return k;
    }

    public static int roundToInt(this float f) => Mathf.RoundToInt(f);

    public static void iterate<T>(this T[,,] array, Action<T, Vector3Int> m)
    {
        var s = array.size();

        for (int i = 0; i < s.x; i++)
        {
            for (int j = 0; j < s.y; j++)
            {
                for (int k = 0; k < s.z; k++)
                {
                    var n = new Vector3Int(i, j, k);
                    m(array.get(n), n);
                }
            }
        }
    }
    public static void iterateBlock<T>(this T[,,] array, Vector3Int start, Vector3Int size, Action<T, Vector3Int> m)
    {

        var s = size;
        //var offset = s + start;

        for (int i = start.x; i < s.x; i++)
        {
            for (int j = start.y; j < s.y; j++)
            {
                for (int k = start.z; k < s.z; k++)
                {

                    var n = new Vector3Int(i, j, k);
                    if (!array.contains(n))
                        continue;

                    m(array.get(n), n);
                }
            }
        }
    }


    public static bool contains<T>(this T[,,] array, Vector3Int v)
    {
        var s = array.size();
        return v.x >= 0 & v.x < s.x
                     && v.y >= 0 && v.y < s.y &&
                     v.z >= 0 & v.z < s.z;
    }

    public static T[,,] toArray<T>(this Vector3Int v) => new T[v.x, v.y, v.z];
    public static void set<T>(this T[,,] array, Vector3Int i, T va) => array[i.x, i.y, i.z] = va;
    public static T get<T>(this T[,,] grid, Vector3Int i) => grid[i.x, i.y, i.z];
    public static Vector3Int size<T>(this T[,,] grid) =>
            new Vector3Int(grid.GetLength(0), grid.GetLength(1), grid.GetLength(2));
    public static Vector3Int roundToInt(this Vector3 v)
    {
        return new Vector3Int(v.x.roundToInt(), v.y.roundToInt(), v.z.roundToInt());
    }
    public static void drawGizmos(this Bounds b) => Gizmos.DrawWireCube(b.center, b.size);

    public static void drawGizmos(this Bounds b, Color c)
    {
        Gizmos.color = c;
        b.drawGizmos();
    }
    public static Vector2 randomPoint2(this Bounds b)
    {
        var v = Vector2.zero.randomVector(Vector2.one);

        v = Vector2.Scale(v, b.size);



        return v + (Vector2)b.min;
    }

    public static float randomRange(this Vector2 v) => Random.Range(v.x, v.y);

    public static Vector2 randomVector(this Vector2 min, Vector2 max)
    {
        return new Vector2(

                URandom.Range(min.x, max.x),
                URandom.Range(min.y, max.y)
                );


    }

    public static Vector3 inverse(this Vector3 v)
    {
        return new Vector3(1 / v.x, 1 / v.y
        , 1 / v.z);
    }
    public static Vector2 inverse(this Vector2 v)
    {
        return new Vector3(1 / v.x, 1 / v.y);
    }
    public static Vector3 toVector3(this Vector2 v) => (Vector3)v;
    public static Vector2 toVector2(this Vector3 v) => (Vector2)v;
    /*public static int getLayer(this UnitTeam t, int vari) {
            if (t == UnitTeam.player)
                    return PLAYER + vari * TEAM_DIS;
            if (t == UnitTeam.enemy)
                    return PLAYER + ENEMY_DIS + vari * TEAM_DIS;
            return 0;
    }*/
    public static void drawGizmos(this Vector2 v, Color color = default(Color), float width = 0.15f)
    {
        if (color != default(Color))
            Gizmos.color = color;
        Gizmos.DrawWireSphere(v, width);
    }
    public static void drawGizmos(this Vector3 v, Color color = default(Color), float width = 0.15f)
    {
        if (color != default(Color))
            Gizmos.color = color;
        Gizmos.DrawWireSphere(v, width);
    }
    public static void SnapTo(this ScrollRect scrollRect, RectTransform target)
    {
        var contentPanel = scrollRect.content;
        Canvas.ForceUpdateCanvases();

        contentPanel.anchoredPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }
    public static void drawGizmos(this Rect r, Color color = default(Color))
    {
        if (color != default(Color))
            Gizmos.color = color;
        Gizmos.DrawWireCube(r.center, r.size.toVector3().setZ(1));
    }
    public static bool between(this int i, int min, int max)
    {
        return i >= min && i < max;
    }
    public static bool between(this Vector2Int v, Vector2Int min, Vector2Int max)
    {
        return v.x.between(min.x, max.x) && v.y.between(min.y, max.y);
    }
    public static Transform getNonInixTransform(this Transform tr)
    {
        var p = tr;
        while (p)
        {
            if (!p.CompareTag(INIXTAG))
                break;
            p = p.parent;
        }
        return p;
    }
    public static void orient(this Transform t, Transform other, bool makeParent = false)
    {
        t.position = other.position;
        t.rotation = other.rotation;
        if (makeParent)
            t.parent = other;

    }
    /*public static WaitForSeconds waitForSeconds
    {
            get
            {

                    return _waitForSeconds;
            }
    }
    private static WaitForSeconds _waitForSeconds;*/

    //public static void loge(this Exception e, UnityEngine.Object obj)
    //{//

    //}
    public static T getOrDefault<T>(this IReadOnlyList<T> list, int index, T def = default(T))
    {
        if (index < list.Count && index >= 0)
            return list[index];
        return def;
    }
    public static WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    public static WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

    public static T getComponentInParentNotthis<T>(this GameObject go)
    {
        var parnet = go.transform.parent;
        if (parnet)
            return parnet.GetComponentInParent<T>();
        /*Transform parent = go.transform.parent;
        while (parent)
        {
                var comp = parent.GetComponent<T>();
                if (comp != null)
                        return comp;
                else
                {
                        parent = parent.parent;
                }

        }*/

        return default(T);

    }

    public static List<T> getComponentsInInChildrenDepth<T>(this GameObject go, int depth, bool includeRoot)
    {
        List<T> list = new List<T>();
        go.getComponentsInInChildrenDepth<T>(depth, includeRoot);
        return list;
    }
    static void getComponentsInInChildrenDepth<T>(this GameObject go, int depth, bool includeRoot, List<T> mList)
    {
        //    List<T> list = new List<T>();

        if (includeRoot)
        {
            var t = go.GetComponent<T>();
            if (t != null)
                mList.checkCapacityAdd(t, go.transform.childCount);
        }

        //int mDepth = 0;
        if (depth > 0)
            foreach (Transform x in go.transform)
            {
                //   int mDepth = 0;
                /*var t = x.GetComponent<T>();
                if(t!=null)
                        mList.addCheckCapacity(t, x.childCount);*/

                // if (mDepth < depth)
                //   {
                getComponentsInInChildrenDepth<T>(x.gameObject, depth - 1, true, mList);
                //   }

            }

        //  return mList;
    }
    public static List<T> getComponentsInInChildrenNotThis<T>(this GameObject go)
    {

        List<T> list = new List<T>();
        foreach (Transform t in go.transform)
        {
            var mArray = t.GetComponentsInChildren<T>();
            list.checkCapacity(mArray.Length);


            foreach (var x in mArray)
            {
                //list.addCheckCapacity();
                list.checkCapacityAdd(x, mArray.Length);
            }
        }

        return list;
    }
    public static T ResourcesLoad<T>(string fileName, ref T target) where T : UnityEngine.Object
    {
        if (target)
            return target;
        return target = Resources.Load<T>(fileName);
    }
#if TRUE_SYNC
    public static FP clamp01(this FP value) => TSMath.Clamp(value, 0, 1);
#endif

    public static Vector3 setX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);
    public static Vector3 setY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
    public static Vector3 setZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);
    public static Vector2 setX(this Vector2 v, float x) => new Vector2(x, v.y);
    public static Vector2 setY(this Vector2 v, float y) => new Vector2(v.x, y);

    public static int roundNext(this int index, int length, int delta = 1)
    {
        var di = index + delta;
        //if (di >= length)
        di %= length;
        return di;
    }

    public static float maxComp(this Vector3 v)
    {
        return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
    }

    public static float minComp(this Vector3 v)
    {
        return Mathf.Min(v.x, Mathf.Min(v.y, v.z));

    }

    public static float maxOfAll(this Vector3 a, Vector3 b)
    {
        return Mathf.Max(a.maxComp(), b.maxComp());
    }

    public static Vector3 abs(this Vector3 v)
    {
        return new Vector3(v.x.abs(), v.y.abs(), v.z.abs());
    }
    public static float abs(this float f)
    {
        return Mathf.Abs(f);
    }

    public static float max(this float a, float b)
    {
        return Mathf.Max(a, b);
    }

    public static float min(this float a, float b)
    {
        return Mathf.Min(a, b);
    }

    public static float clamp(this float a, float min, float max)
    {
        return Mathf.Clamp(a, min, max);
    }

    //public static float clamp01(this float a) => Mathf.Clamp01(a);

    public static float fastMagnitude(this Vector2 v)
    {
        var x = v.x;
        var y = v.y;

        // Get absolute value of each vector
        var ax = abs(x);
        var ay = abs(y);

        // Create a ratio
        var ratio = 1 / max(ax, ay);
        ratio = ratio * (1.29289f - (ax + ay) * ratio * 0.29289f);
        GameObject go;

        return ratio;
        // Multiply by ratio
        //x = x * ratio;
        //y = y * ratio;
    }
    [Obsolete("not qualifed yet")]
    public static float fastMagnitude(this Vector3 v)
    {
        var x = v.x;
        var y = v.y;
        var z = v.z;

        // Get absolute value of each vector
        var ax = abs(x);
        var ay = abs(y);
        var az = abs(z);

        // Create a ratio
        var ratio = 1 / max(az, max(ax, ay));
        ratio = ratio * (1.29289f - (ax + ay + az) * ratio * 0.29289f);

        return ratio;
        // Multiply by ratio
        //x = x * ratio;
        //y = y * ratio;
    }
    public static bool approx(this Vector3 v, Vector3 other)
    {
        return Mathf.Approximately(v.x, other.x) && Mathf.Approximately(v.y, other.y) &&
                     Mathf.Approximately(v.z, other.z);
    }

    public static float powerTimes(this float f, int x)
    {
        var m = 1f;
        for (int i = 0; i < x; i++)
        {
            m *= f;
        }

        return m;
    }
    public static float squared(this float f) => f * f;
    public static bool approx(this float f, float o) => Mathf.Approximately(f, o);
    public static bool higherOrApprox(this float f, float targetValue)
    {
        return f >= targetValue || Mathf.Approximately(targetValue, f);
    }

    public static bool lowerOrApprox(this float f, float targetValue)
    {
        return f <= targetValue || Mathf.Approximately(targetValue, f);
    }
    public static Vector3 toGrid(this Vector2 v, float yValue)
    {
        return new Vector3(v.x, yValue, v.y);
    }
    // [System.Obsolete("use toGrid instead")]
    public static Vector3 toVector3(this Vector2 v, float zValue)
    {
        return new Vector3(v.x, v.y, zValue);
    }

    public static T cachedComponent<T>(this Component c, ref T field)
    {
        if (field != null && !field.Equals(null))
            return field;
        else
            return (field = c.GetComponent<T>());
    }

    public static T cachedComponentInParent<T>(this Component c, ref T field, ref bool notExist)
    {
        if (notExist)
            return field;
        else if (field == null)
        {
            notExist = c.cachedComponentInParent(ref field) == null;
        }

        return field;
    }
    public static T cachedComponent<T>(this Component c, ref T field, ref bool notExist)
    {
        if (notExist)
            return field;
        else if (field == null)
        {
            notExist = c.cachedComponent(ref field) == null;
        }

        return field;
    }
    public static T cachedComponentInChild<T>(this Component c, ref T field, ref bool notExist)
    {
        if (notExist)
            return field;
        else if (field == null)
        {
            notExist = c.cachedComponentInChild(ref field) == null;
        }

        return field;
    }

    public static T cachedComponentInParent<T>(this Component c, ref T field)
    {
        if (field != null)
            return field;
        else
            return (field = c.GetComponentInParent<T>());
    }
    public static T cachedComponentInParentNotThis<T>(this Component c, ref T field)
    {
        if (field != null)
            return field;
        else
            return (field = c.gameObject.getComponentInParentNotthis<T>());
    }
    public static T cachedComponentInChild<T>(this Component c, ref T field)
    {
        if (field != null)
            return field;
        else
            return (field = c.GetComponentInChildren<T>());
    }
    public static T cachedComponent<T>(this GameObject go, ref T field)
    {
        return go.transform.cachedComponent(ref field);
    }

    public static Vector3 WorldToNormalizedViewportPoint(this Camera camera, Vector3 point)
    {
        // Use the default camera matrix to normalize XY, 
        // but Z will be distance from the camera in world units
        point = camera.WorldToViewportPoint(point);

        if (camera.orthographic)
        {
            // Convert world units into a normalized Z depth value
            // based on orthographic projection
            point.z = (2 * (point.z - camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)) - 1f;
        }
        else
        {
            // Convert world units into a normalized Z depth value
            // based on perspective projection
            point.z = ((camera.farClipPlane + camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane))
                    + (1 / point.z) * (-2 * camera.farClipPlane * camera.nearClipPlane / (camera.farClipPlane - camera.nearClipPlane));
        }

        return point;
    }
    public static Vector3 NormalizedViewportToWorldPoint(this Camera camera, Vector3 point)
    {
        if (camera.orthographic)
        {
            // Convert normalized Z depth value into world units
            // based on orthographic projection
            point.z = (point.z + 1f) * (camera.farClipPlane - camera.nearClipPlane) * 0.5f + camera.nearClipPlane;
        }
        else
        {
            // Convert normalized Z depth value into world units
            // based on perspective projection
            point.z = ((-2 * camera.farClipPlane * camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)) /
                    (point.z - ((camera.farClipPlane + camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)));
        }

        // Use the default camera matrix which expects normalized XY but world unit Z 
        return camera.ViewportToWorldPoint(point);
    }

    public static void checkCapacity<T>(this List<T> list, int increment)
    {
        if (list.Count >= list.Capacity)
            list.Capacity = list.Count + increment;
    }

    public static void checkCapacityAdd<T>(this List<T> list, T item, int capacity)
    {
        checkCapacity(list, capacity);
        list.Add(item);
    }

    public static void increaseSizeToMatch<T>(this List<T> list, int targetIndex)
    {
        var targetSize = targetIndex + 1;
        if (list.Count >= targetSize)
            return;
        if (list.Capacity < targetSize)
            list.Capacity = targetSize;
        for (int i = list.Count; i < targetSize; i++)
        {
            list.Add(default(T));
        }
    }

    public static bool hasFlag(this sbyte s, sbyte f)
    {
        return (s & f) != 0;
    }

    public static sbyte setFlag(this sbyte s, sbyte f, bool toSet)
    {
        if (toSet)
        {
            return (sbyte)(s | f);
        }
        else
        {
            return (sbyte)(s & ~f);
        }
    }

    public static void copyTo<T>(this T[] array, ref T[] target, bool rescale, int startIndex = 0)
    {
        if (target == null)
            target = new T[array.Length];
        if (rescale)
        {
            if (target.Length != array.Length)
                target = new T[array.Length];
        }
        array.CopyTo(target, startIndex);

    }

    public static T[] getInBothChildrenAndParents<T>(this Component c, bool dontSearchInChildren = false
    )
    {
        var gameObject = c.gameObject;

        var par = gameObject.GetComponentsInParent<T>();
        if (dontSearchInChildren)
            return par;
        var chil = gameObject.GetComponentsInChildren<T>();

        par.copyTo(ref chil, true, 0);

        return chil;
    }

}

public class TagSelectorAttribute : PropertyAttribute
{
    public bool UseDefaultTagFieldDrawer = false;

    public TagSelectorAttribute()
    {

    }

    public TagSelectorAttribute(bool x)
    {
        UseDefaultTagFieldDrawer = x;
    }
}

#if UNITY_EDITOR
namespace UnityEditor
{

    [CustomPropertyDrawer(typeof(TagSelectorAttribute))]
    public class TagSelectorPropertyDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);

                var attrib = this.attribute as TagSelectorAttribute;

                if (attrib.UseDefaultTagFieldDrawer)
                {
                    property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
                }
                else
                {
                    //generate the taglist + custom tags
                    List<string> tagList = new List<string>();
                    tagList.Add("<NoTag>");
                    tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);
                    string propertyString = property.stringValue;
                    int index = -1;
                    if (propertyString == "")
                    {
                        //The tag is empty
                        index = 0; //first index is the special <notag> entry
                    }
                    else
                    {
                        //check if there is an entry that matches the entry and get the index
                        //we skip index 0 as that is a special custom case
                        for (int i = 1; i < tagList.Count; i++)
                        {
                            if (tagList[i] == propertyString)
                            {
                                index = i;
                                break;
                            }
                        }
                    }

                    //Draw the popup box with the current selected index
                    index = EditorGUI.Popup(position, label.text, index, tagList.ToArray());

                    //Adjust the actual string value of the property based on the selection
                    if (index == 0)
                    {
                        property.stringValue = "";
                    }
                    else if (index >= 1)
                    {
                        property.stringValue = tagList[index];
                    }
                    else
                    {
                        property.stringValue = "";
                    }
                }

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}
#endif

[System.Serializable]
public struct SingleUnityLayer
{
    [SerializeField] private int m_LayerIndex;
    public int LayerIndex
    {
        get { return m_LayerIndex; }
    }

    public void Set(int _layerIndex)
    {
        if (_layerIndex > 0 && _layerIndex < 32)
        {
            m_LayerIndex = _layerIndex;
        }
    }

    public int Mask
    {
        get { return 1 << m_LayerIndex; }
    }
}
#if UNITY_EDITOR

namespace UnityEditor
{

    using UnityEngine;
    using UnityEditor;
    using System.Linq;

    [CustomPropertyDrawer(typeof(SingleUnityLayer))]
    public class SingleUnityLayerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginProperty(_position, GUIContent.none, _property);
            SerializedProperty layerIndex = _property.FindPropertyRelative("m_LayerIndex");
            _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
            if (layerIndex != null)
            {
                layerIndex.intValue = EditorGUI.LayerField(_position, layerIndex.intValue);
            }

            EditorGUI.EndProperty();
        }
    }

    // Don't forget to put this file inside an Editor folder!
    [CustomPropertyDrawer(typeof(NamedArrayAttribute))]
    public class LabeledArrayDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            try
            {
                var path = property.propertyPath;
                int pos = int.Parse(path.Split('[').LastOrDefault().TrimEnd(']'));
                EditorGUI.PropertyField(rect, property,
                        new GUIContent(ObjectNames.NicifyVariableName(((NamedArrayAttribute)attribute).names[pos])), true);
            }
            catch (Exception e)
            {
                EditorGUI.PropertyField(rect, property, label, true);
                Debug.LogException(e);
            }

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(NamedAttribute))]
    public class NamedDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            try
            {
                //int pos = int.Parse(property.propertyPath);

                if (property.propertyPath.Contains("["))
                    throw new Exception();
                EditorGUI.PropertyField(rect, property, new GUIContent(((NamedAttribute)attribute).name));
            }
            catch
            {
                EditorGUI.PropertyField(rect, property, label);
            }
        }
    }


}

#endif


public class NamedAttribute : PropertyAttribute
{
    public string name;
    public NamedAttribute(string n) { name = n; }
}
public class NamedArrayAttribute : PropertyAttribute
{
    public string[] names
    {
        get
        {
            if (_names != null)
                return _names;
            if (_type != null)
            {
                _names = Enum.GetNames(_type);
                /*if (_maxCount > -1)
				{
						string[] temp = new string[_maxCount];
						_names.CopyTo(temp, 0);
						_names = temp;
				}*/
            }
            else
            {
                if (_names == null)
                    _names = new string[0];
            }
            return _names;
        }
        set
        {
            _names = value;
        }
    }
    private string[] _names;
    private Type _type;
    private int _maxCount;
    public NamedArrayAttribute(params string[] names) => this._names = names;

    public NamedArrayAttribute(Type enumType, int maxCount = -1)
    {
        _type = enumType;
        _maxCount = maxCount;
    }
}


[System.Serializable]
public struct CosWave
{
    public float amplitude;
    public float frequency;

    public float calculate(float t)
    {
        return Mathf.Sin(t * Mathf.PI * frequency) * amplitude;
    }

    public float calculateCos(float t)
    {
        return Mathf.Cos(t * Mathf.PI * frequency) * amplitude;

    }

    public float calculateTime() => calculate(Time.time);
    public float calculateTimeCos() => calculateCos(Time.time);
}

public delegate void DIterate3(Vector3Int index);