using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMvt_RotateAround : MovementStrategy
{
    protected override void Initialized()
    {
        base.Initialized();
    }

    void Update()
    {
        ownerBase.transform.rotation = Quaternion.LookRotation((ownerBase.transform.position - ownerBase.transform.parent.position).normalized);
        ownerBase.transform.RotateAround(ownerBase.transform.parent.position, Vector3.up, moveSpeed * Time.deltaTime);
    }
}
