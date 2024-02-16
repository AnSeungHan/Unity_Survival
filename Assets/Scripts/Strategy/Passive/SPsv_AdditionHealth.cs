using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPsv_AdditionHealth : PassiveStrategy
{
    private float   ownerDefaultHealth;
    [SerializeField]
    private float   addHealth;

    public override void BeginPlay()
    {
        base.BeginPlay();

        ownerDefaultHealth = ownerParenttUnit.MaxHealth;

        ownerGadet.Subscribe("OnEquip"  , new OnTrrigerObserver(this.AddHealth, true));
        ownerGadet.Subscribe("OnLevelUp", new OnTrrigerObserver(this.AddHealth, true));
    }

    public override void LevelUp(string _ValueName, string _Value)
    {
        base.LevelUp(_ValueName, _Value);

        switch (_ValueName)
        {
            case "addHealth": addHealth = float.Parse(_Value); break;
        }
    }

    private void AddHealth()
    {
        ownerParenttUnit.MaxHealth = ownerDefaultHealth + addHealth;
    }
}
