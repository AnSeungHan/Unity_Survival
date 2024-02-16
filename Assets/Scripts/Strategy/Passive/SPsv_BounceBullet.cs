using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPsv_BounceBullet : PassiveStrategy
{
    [SerializeField]
    private int addPenetratingCnt = 1;
    [SerializeField]
    private BounceMode mode = BounceMode.Random;

    public override void BeginPlay()
    {
        base.BeginPlay();

        ownerParenttUnit.Subscribe("OnEquip"  , new OnTrrigerObserver(this.Change, true));
        ownerParenttUnit.Subscribe("OnLevelUp", new OnTrrigerObserver(this.Change, true));
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

            if (elem.GetComponent<SAtk_Bounce>())
            {
                elem.GetComponent<SAtk_Bounce>().Clone(this);
            }
            else 
            {
                SAtk_Bounce newCom = elem.gameObject.AddComponent<SAtk_Bounce>();
                newCom.Clone(this);
                newCom.Mode = mode;
                newCom.SetActivate(atk.IsActivate());
            }

            atk.PenetrationCnt = atk.PenetrationCnt + addPenetratingCnt;
            elem.ChangeStrategy();
        }
    }
}
