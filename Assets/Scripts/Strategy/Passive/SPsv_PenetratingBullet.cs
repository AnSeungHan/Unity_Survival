using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPsv_PenetratingBullet : PassiveStrategy
{
    [SerializeField]
    private int addPenetratingCnt;

    public override void BeginPlay()
    {
        base.BeginPlay();

        ownerParenttUnit.Subscribe("OnEquip"  , new OnTrrigerObserver(this.Change, true));
        ownerParenttUnit.Subscribe("OnLevelUp", new OnTrrigerObserver(this.Change, true));
    }

    public override void Clone(IStrategy _Othher)
    {
        base.Clone(_Othher);

        SPsv_PenetratingBullet other = _Othher as SPsv_PenetratingBullet;

        if (null == other)
            return;

        addPenetratingCnt = other.addPenetratingCnt;
    }

    public override void LevelUp(string _ValueName, string _Value)
    {
        base.LevelUp(_ValueName, _Value);

        switch (_ValueName)
        {
            case "addPenetratingCnt": addPenetratingCnt = int.Parse(_Value); break;
        }
    }

    private void Change()
    {
        List<Gadget> gadgets = ownerParenttUnit.Gadgets;

        foreach (Gadget elem in gadgets)
        {
            SAtk_Penetrating atk = elem.GetComponent<SAtk_Penetrating>();

            if (null == atk)
                continue;

            atk.PenetrationCnt = atk.PenetrationCnt + addPenetratingCnt;
        }
    }
}
