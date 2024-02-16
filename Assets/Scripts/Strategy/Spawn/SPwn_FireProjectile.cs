using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPwn_FireProjectile : SpawnStrategy
{
    void Update()
    {
        if (!projecctilePrefab)
            return;

        curTime += Time.deltaTime;

        if (curTime < spawnCycle)
            return;

        Transform targetTransform = Find_Nearest(MakeRaycastHit(spawnRange, targetLayer));

        if (!targetTransform)
            return;

        curTime = 0f;
        Fire(targetTransform);
    }

    private void Fire(Transform _TargetTransform)
    {
        GameObject obj = new Builder<Projectile>(projecctilePrefab)
            .Set_Layer(projectileLayer)
            .Set_Transform(ownerBase.OffsetTransform.position, ownerBase.OffsetTransform.rotation)
            .Set_Direction(_TargetTransform.position - transform.position)
            .Set_Strategy(ownerBase.GetComponents<AttackStrategy>() , false)
            .Set_Strategy(ownerBase.GetComponent<MovementStrategy>(), false)
            .Build();

        if (null == obj)
            Debug.LogError("SPwn_FireProjectile::Fire() Err : [ Projectile Create Fail ]");
    }
}
