using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMvt_Attach : MovementStrategy
{
    [SerializeField]
    Transform       attackTarget;

    void Update()
    {
        if (!attackTarget)
            return;

        ownerBase.transform.position = attackTarget.position;
    }
}
