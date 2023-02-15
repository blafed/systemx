using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DetectTriggerEnter : DetectPhysxCause.Subript
{
    public override PhysxCauseMode mode => PhysxCauseMode.trigger;

    public override PhysxCauseFunction function => PhysxCauseFunction.enter;

    private void onTriggerEnter(Collider other)
    {
        doTrigger(other);
    }

}
