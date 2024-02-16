using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPwn_TargetAttach : SpawnStrategy
{
    [SerializeField]
    private Transform   attackTarget;

    void Update()
    {
        if (!projecctilePrefab)
            return;

        if (!attackTarget)
            return;

        Fire();
        enabled = false;
    }

    private void Fire()
    {
        GameObject obj = new Builder<Projectile>(projecctilePrefab)
            .Set_Layer(projectileLayer)
            .Set_Parent(attackTarget)
            .Set_Position(attackTarget.position)
            .Set_Strategy(ownerBase.GetComponents<AttackStrategy>(), false)
            .Set_Scale(Vector3.one * spawnRange)           
            .Build();
    }
}
