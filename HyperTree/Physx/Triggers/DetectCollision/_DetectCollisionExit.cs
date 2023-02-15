using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DetectCollisionExit : DetectPhysxCause.Subript
{
    public override PhysxCauseMode mode => PhysxCauseMode.collision;

    public override PhysxCauseFunction function => PhysxCauseFunction.exit;

    private void OnCollisionExit(Collision other)
    {
        doCollision(other);
    }

}
