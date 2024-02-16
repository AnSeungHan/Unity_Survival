using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAtk_Penetrating : AttackStrategy
{
    [SerializeField]
    private int     penetrationCnt = 3;

    public int PenetrationCnt { get { return penetrationCnt; } set { penetrationCnt = value; } }

    public override void Clone(IStrategy _Other)
    {
        base.Clone(_Other);

        SAtk_Penetrating other = _Other as SAtk_Penetrating;

        if (null == other)
            return;

        penetrationCnt = other.penetrationCnt;
    }

    public override void LevelUp(string _ValueName, string _Value)
    {
        base.LevelUp(_ValueName, _Value);

        switch (_ValueName)
        {
            case "penetrationCnt" : penetrationCnt = int.Parse(_Value); break;
        }
    }

    protected override void OnCrash(ref float _TotalDamage)
    {
        base.OnCrash(ref _TotalDamage);

        --penetrationCnt;

        if (0 < penetrationCnt)
        { 
            Active_AttackEffect("OnCrash");
            return;
        }

        Active_AttackEffect("OnDead");
        ownerBase.Dead();
    }
}
