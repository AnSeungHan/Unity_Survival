using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPwn_TargetBurstFire : SpawnStrategy
{
    [SerializeField]
    private int         burstCnt;
    [SerializeField]
    private float       burstDelay;
    private bool        isBustDone = true;
    private Transform   target;

    public override void Clone(IStrategy _Other)
    {
        base.Clone(_Other);

        SPwn_TargetBurstFire other = _Other as SPwn_TargetBurstFire;

        if (null == other)
            return;

        burstCnt   = other.burstCnt;
        burstDelay = other.burstDelay;
    }

    void Update()
    {
        if (!projecctilePrefab)
            return;

        if (!isBustDone)
            return;

        curTime += Time.deltaTime;

        if (curTime < spawnCycle)
            return;

        Transform targetTransform = Find_Nearest(MakeRaycastHit(spawnRange, targetLayer));

        if (!targetTransform)
            return;

        target     = targetTransform;
        curTime    = 0f;
        isBustDone = false;

        StartCoroutine(FireProjectile());
    }

    private void Fire()
    {
        GameObject obj = new Builder<Projectile>(projecctilePrefab)
            .Set_Layer(projectileLayer)
            .Set_Transform(ownerBase.OffsetTransform.position, ownerBase.OffsetTransform.rotation)
            .Set_Direction(target.position - ownerBase.transform.position)
            .Set_Strategy(ownerBase.GetComponents<AttackStrategy>() , false)
            .Set_Strategy(ownerBase.GetComponent<MovementStrategy>(), false)
            .Build();

        Active_SpawnEffect("OnFire");
        Activate_Sound("OnFire");

        if (null == obj)
            Debug.LogError("SPwn_BurstFireProjectile::Fire() Err : [ Projectile Create Fail ]");
    }

    IEnumerator FireProjectile()
    {
        for (int i = 0; i < burstCnt; ++i)
        {
            yield return new WaitForSeconds(burstDelay);

            Fire();
        }

        isBustDone = true;
        target     = null;

        yield break;
    }
}
