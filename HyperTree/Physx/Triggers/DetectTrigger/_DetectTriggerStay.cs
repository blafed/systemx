using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DetectTriggerStay : DetectPhysxCause.Subript
{
    public override PhysxCauseMode mode => PhysxCauseMode.trigger;

    public override PhysxCauseFunction function => PhysxCauseFunction.stay;

    private void OnTriggerStay(Collider other)
    {
        doTrigger(other);
    }

}
