using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMvt_KeyInput
    : MovementStrategy
{
    private Rigidbody       ownerRigidbody;
    private Vector3         inputVec;
    private Transform       meshTransform;

    [SerializeField]
    private VirtualJoystick joystick;

    protected override void Initialized()
    {
        base.Initialized();

        ownerRigidbody = ownerBase.GetComponent<Rigidbody>();
        meshTransform  = ownerBase.transform.Find("Mesh");
    }

    void Update()
    {
        /*if (!joystick)
        {
            inputVec.x = Input.GetAxis("Horizontal");
            inputVec.z = Input.GetAxis("Vertical");
        }
        else
        {
            if (Input.GetKey("Horizontal") || Input.GetKey("Vertical"))
            {
                inputVec.x = Input.GetAxis("Horizontal");
                inputVec.z = Input.GetAxis("Vertical");
            }
            else
            {
                inputVec.x = joystick.InputDirection.x;
                inputVec.z = joystick.InputDirection.y;               
            }
        }*/

        if (!joystick)
            return;

        inputVec.x = joystick.InputDirection.x;
        inputVec.z = joystick.InputDirection.y;
    }

    void FixedUpdate()
    {
        Vector3 Dir       = inputVec.normalized;
        Vector3 NextMove  = Dir * moveSpeed * Time.fixedDeltaTime;
        ownerBase.MoveDir = Dir;

        ownerRigidbody.MovePosition(ownerRigidbody.position + NextMove);
        ownerRigidbody.velocity = Vector3.zero;

        if (Dir == Vector3.zero)
            return;

        meshTransform.rotation = Quaternion.LookRotation(Dir);
        //transform.rotation = Quaternion.LookRotation(Dir);
    }
}
