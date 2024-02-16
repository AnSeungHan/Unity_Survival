using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAtk_Sustained : AttackStrategy
{
    [SerializeField]
    float   attackCycle = 1f;

    public override void BeginPlay()
    {
        base.BeginPlay();

        ownerBase.Subscribe("OnCrashStay", new OnCrashObserver(this.OnCrashStay, true));
    }

    private void OnCrashStay(ref float _TotalDamage)
    {
        curTime += Time.deltaTime;

        if (curTime < attackCycle)
            return;

        base.OnCrash(ref _TotalDamage);

        curTime = 0f;
    }
}
