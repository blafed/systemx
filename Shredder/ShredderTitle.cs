using UnityEngine;
using System.Collections.Generic;
namespace Shredder
{
    using Titles;

    public class ShredderFactory
    {
        public static ShredderFactory current = new ShredderFactory();
        public virtual IDependencyRef getDependencyRef(Object o)
        {
            return new DependencyRef {obj = o};
        }

        struct  DependencyRef : IDependencyRef
        {
            public Object obj { get; set; }
        }
    }
    public abstract class Title
    {
        
        public virtual TitleName name { get; }
        public virtual System.Type type { get; }
        public int index { get; internal set; }
        public virtual void write(Object o) { }
        public virtual void read(Object o) { }

        public virtual Object create(Object context)
        {
            return null;}//like add object
        
        protected  void depend(ref Object o){}
        
        public virtual void field(ScopeMode mode){}
        
        public static Title getTitle(Object obj)
        {
            var x = getTitle(getTitleName(obj));
            if (x == null)
                if (obj is ITitleProvider titleProvider)
                    return titleProvider.getTitle();
            return x;
        }

        public static TitleName getTitleName(Object obj)
        {
            if (obj is Rigidbody)
                return TitleName.Rigidbody;
            if (obj is Transform)
                return TitleName.Transform;
            if (obj is GameObject)
                return TitleName.GameObject;
            if (obj is SphereCollider)
                return TitleName.SphereCollider;
            if (obj is CapsuleCollider)
                return TitleName.CapsuleCollider;
            if (obj is BoxCollider)
                return TitleName.BoxCollider;
            if (obj is FixedJoint)
                return TitleName.FixedJoint;
            return TitleName.none;
        }
        public static Title getTitle(TitleName name)
        {
            switch (name)
            {
                case TitleName.GameObject:
                    return new _GameObject();
                case TitleName.Rigidbody:
                    return new _Rigidbody();
                case TitleName.CapsuleCollider:
                    return new _CapsuleCollider();
                case TitleName.Transform:
                    return new _Transform();
                case TitleName.SphereCollider:
                    return new _SphereCollider();
                case TitleName.BoxCollider:
                    return new _BoxCollider();
case TitleName.FixedJoint:
    return new _FixedJoint();
            }
            return null;
        }
    }

    public abstract class Title<T> : Title where T : Object
    {
        public override System.Type type => typeof(T);
        public override sealed void write(Object o)
        {
            this.write(x: (T)o);
        }
        public override sealed void read(Object o)
        {
            this.read(x: (T)o);
        }
        public virtual void write(T x)
        {
        }
        public virtual void read(T x)
        {
        }
    }

    public abstract class TitleComponent<T> : Title<T> where T : Component
    {
        public override Object create(Object context)
        {
            var go = context as GameObject;
            if (!go)
            {
                return null;
            }

            return go.AddComponent<T>();
        }
    }

    public enum TitleName
    {
        none,
        GameObject,
        Transform,
        Rigidbody,
        BoxCollider,
        SphereCollider,
        CapsuleCollider,
        FixedJoint,
    }

    public interface ITitleProvider
    {
        Title getTitle();
    }
    public interface IDependencyRef{
    Object obj { get; set; }
    }
}