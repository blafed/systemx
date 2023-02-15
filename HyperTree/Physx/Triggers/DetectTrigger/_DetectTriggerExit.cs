using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DetectTriggerExit : DetectPhysxCause.Subript
{
    public override PhysxCauseMode mode => PhysxCauseMode.trigger;

    public override PhysxCauseFunction function => PhysxCauseFunction.exit;

    private void OnTriggerExit(Collider other)
    {
        doTrigger(other);
    }

}
