using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPsv_ChangePrectile : PassiveStrategy
{
    [SerializeField]
    private GameObject   newProjectile;

    public override void BeginPlay()
    {
        base.BeginPlay();

        ownerGadet.Subscribe("OnEquip"  , new OnTrrigerObserver(this.Change, true));
        ownerGadet.Subscribe("OnLevelUp", new OnTrrigerObserver(this.Change, true));
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
            SpawnStrategy[] strategys = elem.GetComponents<SpawnStrategy>();

            foreach (SpawnStrategy elem_com in strategys)
            {

            }
        }
    }
}
