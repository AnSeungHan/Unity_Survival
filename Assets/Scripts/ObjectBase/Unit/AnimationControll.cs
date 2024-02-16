using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControll : MonoBehaviour
{
    private Animator    animator;
    private ObjectBase  ownerBase;

    void Awake()
    {
        animator  = GetComponent<Animator>();
        ownerBase = transform.parent.GetComponent<ObjectBase>();
    }

    void Update()
    {
        if (ownerBase.MoveDir == Vector3.zero)
        { 
            animator.SetInteger("Movement", 0);
        }
        else 
        {
            MovementStrategy str = ownerBase.GetComponent<MovementStrategy>();

            if (5f <= str.MoveSpeed)
                animator.SetInteger("Movement", 2);
            else
                animator.SetInteger("Movement", 1);
        }
    }
}
