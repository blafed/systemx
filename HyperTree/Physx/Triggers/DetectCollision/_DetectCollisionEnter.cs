using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DetectCollisionEnter : DetectPhysxCause.Subript
{
    public override PhysxCauseMode mode => PhysxCauseMode.collision;

    public override PhysxCauseFunction function => PhysxCauseFunction.enter;

    private void OnCollisionEnter(Collision other)
    {
        doCollision(other);
    }

}
