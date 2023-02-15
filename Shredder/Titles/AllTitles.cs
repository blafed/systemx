using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shredder.Titles
{
    public class _Rigidbody : TitleComponent<Rigidbody>
    {
        public override TitleName name => TitleName.Rigidbody;
        

        Vector3 velocity;
        Vector3 angularVelocity;
        bool isKinematic;
        RigidbodyConstraints constraints;
        float mass;
        float drag;
        float angularDrag;
        CollisionDetectionMode detectionMode;
        float maxAngularVelocity;

        public override void field(ScopeMode mode)
        {
            if (mode == ScopeMode.listFields)
            {
                ScopeArgs.fields = new string[]
                {
                    "velocity", "angular-velocity", "kinematic", "constraints", "mass", "drag", "angular-drag",
                    "detection-mode", "max-angular-velocity"
                };
            }
            switch (ScopeArgs.fieldName)
            {
                case "velocity":
                    velocity = ScopeArgs._vector3;
                    break;
                case "angularVelocity":
                    case "angular-velocity":
                    angularVelocity = ScopeArgs._vector3;
                    break;
                
            }
        }

        public override void read(Rigidbody x)
        {
            velocity = x.velocity;
            angularVelocity = x.angularVelocity;
            isKinematic = x.isKinematic;
            mass = x.mass;
            drag = x.drag;
            angularDrag = x.angularDrag;
            constraints = x.constraints;
            detectionMode = x.collisionDetectionMode;
            maxAngularVelocity = x.maxAngularVelocity;
        }
        public override void write(Rigidbody x)
        {
            x.velocity = velocity;
            x.angularVelocity = angularVelocity;
            x.isKinematic = isKinematic;
            x.constraints = constraints;
            x.mass = mass;
            x.drag = drag;
            x.angularDrag = angularDrag;
            x.collisionDetectionMode = detectionMode;
            x.maxAngularVelocity = maxAngularVelocity;
        }
    }


    public class _Transform : TitleComponent<Transform>
    {
        public override TitleName name => TitleName.Transform;
        Vector3 localPos;
        Quaternion localRot;
        Vector3 localScale;
        public override void write(Transform t)
        {
            t.localPosition = localPos;
            t.localRotation = localRot;
            t.localScale = localScale;
        }

        public override void read(Transform t)
        {
            localPos = t.localPosition;
            localRot = t.localRotation;
            localScale = t.lossyScale;
        }
    }

    public class _GameObject : Title<GameObject>
    {
        public override TitleName name => TitleName.GameObject;
        bool active, _static;
        string _name;
        public override Object create(Object context)
        {
            var go = new GameObject();
            if (context is GameObject parent)
                go.transform.parent = parent.transform;
            return go;
        }
        public override void read(GameObject t)
        {
            active = t.activeSelf;
            _name = t.name;
            _static = t.isStatic;
        }

        public override void write(GameObject t)
        {
            t.SetActive(active);
            t.name = _name;
            t.isStatic = _static;
        }
    }

    public class _Collider<T> : TitleComponent<Collider> where T : Collider
    {
        public override Type type => typeof(T);
        public bool isTrigger;
        public override sealed void read(Collider t)
        {
            isTrigger = t.isTrigger;
            read(x: (T)t);
        }
        public override sealed void write(Collider t)
        {
            t.isTrigger = isTrigger;
            write(x: (T)t);
        }
        public virtual void read(T x)
        {
        }
        public virtual void write(T x)
        {
        }
    }
    public class _SphereCollider : _Collider<SphereCollider>
    {
        public override TitleName name => TitleName.SphereCollider;
        public Vector3 center;
        public float radius;
        public override void write(SphereCollider x)
        {
            base.write(x);
            x.radius = radius;
            x.center = center;
        }
        public override void read(SphereCollider x)
        {
            base.read(x);
            radius = x.radius;
            center = x.center;
        }
    }
    public class _CapsuleCollider : _Collider<CapsuleCollider>
    {
        public override TitleName name => TitleName.CapsuleCollider;

        public int direction;
        public Vector3 center;
        public float height;
        public float radius;
        public override void write(CapsuleCollider x)
        {
            x.radius = radius;
            x.direction = direction;
            x.height = height;
            x.center = center;
        }
        public override void read(CapsuleCollider x)
        {
            radius = x.radius;
            direction = x.direction;
            height = x.height;
            center = x.center;
        }
    }

    public class _BoxCollider : _Collider<BoxCollider>
    {
        public override TitleName name => TitleName.BoxCollider;
        public Vector3 size;
        public Vector3 center;

        public override void write(BoxCollider x)
        {
            x.size  = size;
            x.center = center;
        }

        public override void read(BoxCollider x)
        {
            size = x.size;
            center = x.center;
        }
    }

    public class _FixedJoint : TitleComponent<FixedJoint>
    {
        public override TitleName name => TitleName.FixedJoint;
        public IDependencyRef connectedBody;
        public Vector3 anchor, connectedAnchor, axis;
        public bool autoConfigureConnectedAnchor;
        public float breakForce, breakTorque;
        public override void write(FixedJoint x)
        {
            x.connectedBody = connectedBody.obj as Rigidbody;
            x.anchor = anchor;
            x.connectedAnchor = connectedAnchor;
            x.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
            x.breakForce = breakForce;
            x.breakTorque = breakTorque;
            x.axis = axis;
        }

        public override void read(FixedJoint x)
        {
            connectedBody = ShredderFactory.current.getDependencyRef(x);
        }
    }
}