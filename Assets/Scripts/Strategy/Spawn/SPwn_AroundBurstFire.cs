using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPwn_AroundBurstFire : SpawnStrategy
{
    [SerializeField]
    private int     burstCnt;
    [SerializeField]
    private float   offsetDistance = 1f;
    [SerializeField]
    private float   stayTime;
    private float   angle;

    public override void Clone(IStrategy _Other)
    {
        base.Clone(_Other);

        SPwn_AroundBurstFire other = _Other as SPwn_AroundBurstFire;

        if (null == other)
            return;

        burstCnt = other.burstCnt;
    }

    void Update()
    {
        if (!projecctilePrefab)
            return;

        curTime += Time.deltaTime;

        if (curTime < spawnCycle)
            return;

        curTime = 0f;
        angle   = 360f / burstCnt;

        for (int i = 0; i < burstCnt; ++i)
            Fire(i);
    }

    private void Fire(int _Index)
    {
        Vector3 addPos = Quaternion.AngleAxis(_Index * angle, Vector3.up) * Vector3.forward * offsetDistance;

        GameObject obj = new Builder<Projectile>(projecctilePrefab)
            .Set_Layer(projectileLayer)
            .Set_DeleteTime(stayTime)
            .Set_Parent(ownerBase.transform)
            .Set_Position(ownerBase.OffsetTransform.position + addPos)
            .Set_Rotation(ownerBase.OffsetTransform.rotation)
            .Set_Direction(ownerBase.transform.rotation.eulerAngles)
            .Set_Strategy(ownerBase.GetComponents<AttackStrategy>() , false)
            .Set_Strategy(ownerBase.GetComponent<MovementStrategy>(), false)
            .Build();

        Active_SpawnEffect("OnFire");

        if (null == obj)
            Debug.LogError("SPwn_BurstFireProjectile::Fire() Err : [ Projectile Create Fail ]");
    }
}
