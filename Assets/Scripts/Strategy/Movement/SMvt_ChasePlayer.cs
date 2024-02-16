using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMvt_ChasePlayer : MovementStrategy
{
    private Rigidbody   playerRigidbody;
    private Rigidbody   ownerRigidbody;
    private Transform   meshTransform;

    [SerializeField]
    private float   stopDistance = 1f;

    protected override void Initialized()
    {
        base.Initialized();

        ownerRigidbody  = ownerBase.GetComponent<Rigidbody>();
        meshTransform   = ownerBase.transform.Find("Mesh");
    }

    void FixedUpdate()
    {
        playerRigidbody = GameManager.Instance.Player.GetComponent<Rigidbody>();

        Vector3 pos  = playerRigidbody.position - ownerRigidbody.position;
        float   Dist = Mathf.Abs(pos.magnitude);
        Vector3 Dir  = pos.normalized;

        if (stopDistance >= Dist)
        { 
            ownerBase.MoveDir = Vector3.zero;
            return;
        }

        Vector3 NextMove = Dir * moveSpeed * Time.fixedDeltaTime;

        ownerRigidbody.MovePosition(ownerRigidbody.position + NextMove);
        ownerRigidbody.velocity = Vector3.zero;
        ownerBase.MoveDir = Dir;

        if (Dir == Vector3.zero)
            return;

        meshTransform.rotation = Quaternion.LookRotation(Dir);
        //transform.rotation = Quaternion.LookRotation(Dir);
    }
}
