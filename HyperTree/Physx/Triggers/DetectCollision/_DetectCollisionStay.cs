using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DetectCollisionStay : DetectPhysxCause.Subript
{
    public override PhysxCauseMode mode => PhysxCauseMode.collision;

    public override PhysxCauseFunction function => PhysxCauseFunction.stay;

    private void OnCollisionStay(Collision other)
    {
        doCollision(other);
    }

}
