using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPsv_ChangeAtkStrategy : PassiveStrategy
{
    [SerializeField]
    private float Damage;

    public override void BeginPlay()
    {
        base.BeginPlay();

        ownerParenttUnit.Subscribe("OnEquip", new OnTrrigerObserver(this.Change, true));
    }

    public override void LevelUp(string _ValueName, string _Value)
    {
        base.LevelUp(_ValueName, _Value);    
    }

    private void Change()
    {
        List<Gadget> gadgets = ownerParenttUnit.Gadgets;

        foreach (Gadget elem in gadgets)
        {
            List<AttackStrategy> strategy = elem.GetAttackStrategys();

            if (0 == strategy.Count)
                continue;

            SAtk_Penetrating atk = elem.gameObject.GetComponent<SAtk_Penetrating>();

            if (null == atk)
                continue;

            if (3 > atk.PenetrationCnt)
                atk.PenetrationCnt = 3;
        }
    }
}
