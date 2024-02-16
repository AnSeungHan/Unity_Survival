using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMvt_Directional : MovementStrategy
{
    private Rigidbody  ownerRigidbody;

    protected override void Initialized()
    {
        base.Initialized();

        ownerRigidbody = ownerBase.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!ownerRigidbody)
            return;

        Vector3 NextMove = ownerBase.MoveDir * moveSpeed * Time.fixedDeltaTime;
        ownerRigidbody.MovePosition(ownerRigidbody.position + NextMove);

        if (Vector3.zero != ownerBase.MoveDir)
            transform.rotation = Quaternion.LookRotation(ownerBase.MoveDir);

        /*ownerBase.transform.rotation = Quaternion.LookRotation(NextMove.normalized, Vector3.up);
        ownerBase.transform.Translate(NextMove);*/
    }
}