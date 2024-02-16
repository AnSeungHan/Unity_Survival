using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAtk_Diffusion : AttackStrategy
{
    [SerializeField]
    private float       diffusionRange;

    void Update()
    {
        RaycastHit[] ray = MakeRaycastHit(diffusionRange, targetLayer);

        foreach (RaycastHit elem in ray)
        {
            ObjectBase objBase = elem.transform.gameObject.GetComponent<ObjectBase>();

            if (null == objBase)
                continue;

            objBase.TakeDamage(Damage);
        }

        enabled = false;
    }
}
