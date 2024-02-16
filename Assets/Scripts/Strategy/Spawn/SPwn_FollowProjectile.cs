using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPwn_FollowProjectile : SpawnStrategy
{
    [SerializeField]
    private float       rangeScale = 5f;
    private Projectile  bullet;

    void Update()
    {
        if (!projecctilePrefab || bullet)
            return;

        curTime += Time.deltaTime;

        if (curTime >= spawnCycle * 2f)
        {
            Fire();
            curTime = 0f;
        }
    }

    private void Fire()
    {
        GameObject obj = new Builder<Projectile>(projecctilePrefab)
            .Set_DeleteTime(spawnCycle)
            .Set_Layer(projectileLayer)
            .Set_Parent(ownerBase.transform)
            .Set_Position(ownerBase.transform.position)
            .Set_Scale(Vector3.one * rangeScale)
            .Set_Strategy(ownerBase.GetComponents<AttackStrategy>() , false)
            .Set_Strategy(ownerBase.GetComponent<MovementStrategy>(), false)
            .Build();

        if (0 >= SpawnCycle)
        {
            bullet = obj.GetComponent<Projectile>();
            enabled = false;
        }

        Activate_Sound("OnFire");

        if (null == obj)
            Debug.LogError("SPwn_FollowProjectile::Fire() Err : [ Projectile Create Fail ]");
    }
}
