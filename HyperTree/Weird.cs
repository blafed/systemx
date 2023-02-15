using UnityEngine;
using System;
using System.Collections.Generic;

[DefaultExecutionOrder(1000)]
public class Weird : MonoBehaviour
{
    public Wache wache => _wache == null ? _wache = new Wache { weird = this } : _wache;
    Wache _wache;
    IntelligentListener onKint = new IntelligentListener();
    IntelligentListener onKintInternal = new IntelligentListener();
    private void Start()
    {
        init();
        emit(k: Kint.init);
    }

    void OnDestroy()
    {
        emit(Kint.destoryed);
    }
    protected virtual void init() { }

    public void emit(int k)
    {
        print(k);
        KintArg.global = null;
        KintArg.obj = this;
        KintArg.kint = k;
        onEmitInternal?.Invoke(k);
        onEmit?.Invoke(k);
    }

    public event Action<int> onEmit;
    public event Action<int> onEmitInternal;

    // public void listen(int k, Action a, bool isInternal = false) => listen((int)k, a, isInternal);
    // public void unlisten(int k, Action a, bool isInternal = false) => unlisten((int)k, a, isInternal);

    public void listen(int k, Action a, bool isInternal = false)
    {
        if (isInternal)
        {
            onKintInternal.listen(k, a);
        }
        else
            onKint.listen(k, a);
    }
    public void unlisten(int k, Action a, bool isInternal = false)
    {
        if (isInternal)
        {
            onKintInternal.unlisten(k, a);
        }
        else
            onKint.unlisten(k, a);
    }

    class IntelligentListener
    {
        public List<Action> actions = new List<Action>();

        public void invoke(int i)
        {
            if (i >= actions.Count)
                return;
            actions[i]?.Invoke();
        }
        public void listen(int k, Action a)
        {
            if (k >= actions.Count)
            {
                actions.Capacity = k;
                for (int i = actions.Count - 1; i < k; i++)
                {
                    actions.Add(null);
                }
            }
            actions[k] += a;
        }
        public void unlisten(int k, Action a)
        {
            actions[k] -= a;
        }
    }

}

public partial struct Kint
{
    public const int none = 0,

    init = 1,
    enableSketch = 2,
    disableSketch = 3,
    destoryed = 4,
    physx = 5;
    // public const int dummyPartBreak = 55;
    // public const int dummyPartCollision = 56;

    public const int _complete = 6;

}

public partial struct KintArg
{
    public static Sketch sketch;
    public static int kint;
    public static object global;
    public static Weird obj;
    public static PhysxCauseFunction causeFunction;
    public static PhysxDimension causeDimension;
    public static PhysxCauseMode causeMode;
    public static Collider physxCollider;
    public static Collision physxCollision;
}
[DefaultExecutionOrder(900)]
public class Sketch : MonoBehaviour
{
    public Wache wache => this.weird.wache;
    public Weird weird => _weird ? _weird :
    _weird = this.getComponentCachedInParent(ref _weird) ? _weird : _weird = gameObject.AddComponent<Weird>();
    Weird _weird;

    private void OnEnable()
    {
        KintArg.sketch = this;
        weird.emit(Kint.enableSketch);
        onEnabledChange(true);
    }
    private void OnDisable()
    {
        KintArg.sketch = this;
        weird.emit(Kint.disableSketch);
        onEnabledChange(false);

    }
    private void Start()
    {
        listen(Kint.init, init);
    }

    protected void emit(int k)
    {
        weird.emit(k);
    }
    protected void listen(int k, Action a)
    {
        weird.listen(k, a, true);
    }
    protected void unlisten(int k, Action a)
    {
        weird.unlisten(k, a, true);
    }

    protected virtual void init()
    {

    }
    protected virtual void onEnabledChange(bool enabled) { }


}

public partial class Wache
{
    public Weird weird;
}


public class KintAttribute : UnityEngine.PropertyAttribute
{
    public Type customType;
    public KintAttribute(Type customType)
    {
        this.customType = customType;
    }
    public KintAttribute()
    {

    }
}

#if UNITY_EDITOR
namespace UnityEditor
{
    using System.Reflection;
    [CustomPropertyDrawer(typeof(KintAttribute))]
    public class KintDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var attr = (KintAttribute)base.attribute;
            try
            {
                //int pos = int.Parse(property.propertyPath);
                var targetType = attr.customType ?? typeof(Kint);
                var fields = targetType.GetFields(BindingFlags.Static);
                var listOfFields = new string[fields.Length - 1];
                var values = new int[fields.Length - 1];
                var typeofInt = typeof(int);
                foreach (var f in fields)
                {
                    var name = f.Name;
                    if (name.StartsWith("_"))
                        continue;
                    if (f.FieldType != typeofInt)
                        continue;
                    var value = (int)f.GetValue(null);
                    listOfFields[value] = name;
                    values[value] = value;
                }
                property.intValue = EditorGUILayout.IntPopup(label.text, property.intValue, listOfFields, values);
                //                EditorGUI.PropertyField(rect, property, new GUIContent(((NamedAttribute)attribute).name));
            }
            catch
            {
                EditorGUI.PropertyField(rect, property, label);
            }
        }
    }
}
#endif