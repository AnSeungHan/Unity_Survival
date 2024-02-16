using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SMvt_KeyInput_ver2
    : MovementStrategy
{
    private Rigidbody       ownerRigidbody;
    private Vector3         inputVec;
    private Transform       meshTransform;

    protected override void Initialized()
    {
        base.Initialized();

        ownerRigidbody = ownerBase.GetComponent<Rigidbody>();
        meshTransform  = ownerBase.transform.Find("Mesh");
    }

    void Update()
    {
        Vector3 Dir       = UIManager.Instance.Joystick.InputDirection().normalized;
        Vector3 NextMove  = Dir * moveSpeed * Time.deltaTime;
        ownerBase.MoveDir = Dir;

        ownerRigidbody.MovePosition(ownerRigidbody.position + NextMove);
        ownerRigidbody.velocity = Vector3.zero;

        if (Dir == Vector3.zero)
            return;

        meshTransform.rotation = Quaternion.LookRotation(Dir);
    }

    void OnMove(InputValue _Value)
    {
        //Vector2 inputVal = _Value.Get<Vector2>();
        //Vector2 inputVal = UIManager.Instance.Joystick.GetComponent<VirtualJoystick_ver2>().InputDirection();
       /* inputVal = inputVal.normalized;

        inputVec = new Vector3(inputVal.x, 0, inputVal.y);*/


        //Debug.Log(inputVal);
    }  
}
