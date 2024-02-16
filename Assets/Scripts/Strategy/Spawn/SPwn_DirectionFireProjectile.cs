using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPwn_DirectionFireProjectile : SpawnStrategy
{
    void Update()
    {
        if (!projecctilePrefab)
            return;

        curTime += Time.deltaTime;

        if (curTime < spawnCycle)
            return;

        curTime = 0f;
        Fire();
    }

    private void Fire()
    {
        GameObject obj = new Builder<Projectile>(projecctilePrefab)
            .Set_Layer(projectileLayer)
            .Set_Transform(ownerBase.OffsetTransform.position, ownerBase.OffsetTransform.rotation)
            .Set_Direction(ownerBase.transform.rotation.eulerAngles)
            .Set_Strategy(ownerBase.GetComponents<AttackStrategy>() , false)
            .Set_Strategy(ownerBase.GetComponent<MovementStrategy>(), false)
            .Build();

        if (null == obj)
            Debug.LogError("SPwn_DirectionFireProjectile::Fire() Err : [ Projectile Create Fail ]");
    }
}
