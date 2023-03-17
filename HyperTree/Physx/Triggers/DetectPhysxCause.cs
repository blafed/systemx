using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPhysxCause : MonoBehaviour
{
    public PhysxDimension dimension;
    public PhysxCauseMode mode;
    public PhysxCauseFunction function;
    [Kint]
    public int emit = Kint.physx;

    Weird _weird;
    public Weird weird => this.cachedComponentInParent(ref _weird);
    Subript subript => this.cachedComponent(ref _subript);
    Subript _subript;
    // Start is called before the first frame update

    public void setup()
    {
        if (subript)
            if (subript.function != function || subript.mode != mode || subript.dimension != this.dimension)
            {
                Destroy(subript);
            }
            else
            {
                return;
            }
        if (mode == PhysxCauseMode.collision)
            switch (function)
            {
                case PhysxCauseFunction.enter:
                    gameObject.AddComponent<_DetectCollisionEnter>();
                    break;
                case PhysxCauseFunction.exit:
                    gameObject.AddComponent<_DetectCollisionExit>();
                    break;
                case PhysxCauseFunction.stay:
                    gameObject.AddComponent<_DetectCollisionStay>();
                    break;
            }
        if (mode == PhysxCauseMode.trigger)
            switch (function)
            {
                case PhysxCauseFunction.enter:
                    gameObject.AddComponent<_DetectTriggerEnter>();
                    break;
                case PhysxCauseFunction.exit:
                    gameObject.AddComponent<_DetectTriggerExit>();
                    break;
                case PhysxCauseFunction.stay:
                    gameObject.AddComponent<_DetectTriggerStay>();
                    break;
            }
    }

    private void Awake()
    {
        setup();
    }

    public abstract class Subript : MonoBehaviour
    {
        public new Collider collider { get; set; }
        public Collision collision { get; set; }
        public virtual PhysxDimension dimension => PhysxDimension._3D;
        public abstract PhysxCauseMode mode { get; }
        public abstract PhysxCauseFunction function { get; }
        public DetectPhysxCause component => this.cachedComponent(ref _component);
        DetectPhysxCause _component;

        void doKint()
        {
            if (component.emit > 0)
            {
                if (component.weird)
                {
                    KintArg.causeMode = this.mode;
                    KintArg.causeDimension = this.dimension;
                    KintArg.causeFunction = this.function;
                    KintArg.physxCollision = this.collision;
                    KintArg.physxCollider = this.collider;
                    component.weird.emit(component.emit);
                }
            }
        }
        public void doCollision(Collision c)
        {
            this.collision = c;
            doKint();

        }
        public void doTrigger(Collider c)
        {
            this.collider = c;
            doKint();
        }
    }
}


public enum PhysxDimension
{
    _3D,
    _2D
}

public enum PhysxCauseMode
{
    trigger,
    collision,
}
public enum PhysxCauseFunction
{
    enter,
    exit,
    stay,
}
