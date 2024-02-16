using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPsv_AdditionSpeed : PassiveStrategy
{
    [SerializeField]
    private float               addSpeed;

    public override void BeginPlay()
    {
        base.BeginPlay();

        ownerGadet.Subscribe("OnEquip"  , new OnTrrigerObserver(this.AddSpeed, true));
        ownerGadet.Subscribe("OnLevelUp", new OnTrrigerObserver(this.AddSpeed, true));
    }

    public override void LevelUp(string _ValueName, string _Value)
    {
        base.LevelUp(_ValueName, _Value);

        switch (_ValueName)
        {
            case "addSpeed" : addSpeed = float.Parse(_Value); break;
        }
    }

    private void AddSpeed()
    {
        MovementStrategy movement = ownerParenttUnit.GetComponent<MovementStrategy>();

        if (null == movement)
            return;

        movement.MoveSpeed = movement.MoveSpeed + addSpeed;
    }
}
