using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPsv_CoolDown : PassiveStrategy
{
    [SerializeField]
    private float               coolDown;

    protected override void Initialize()
    {
        base.Initialize();
    }

    public override void BeginPlay()
    {
        base.BeginPlay();

        ownerGadet.Subscribe("OnEquip"  , new OnTrrigerObserver(this.AddCoolDown, true));
        ownerGadet.Subscribe("OnLevelUp", new OnTrrigerObserver(this.AddCoolDown, true));
    }

    public override void LevelUp(string _ValueName, string _Value)
    {
        base.LevelUp(_ValueName, _Value);

        switch (_ValueName)
        {
            case "coolDown": coolDown = float.Parse(_Value); break;
        }
    }

    private void AddCoolDown()
    {
        List<Gadget> gadgets = ownerParenttUnit.Gadgets;
        foreach (Gadget elem_gadget in gadgets)
        {
            SpawnStrategy[] spawns = elem_gadget.GetComponents<SpawnStrategy>();
            foreach (SpawnStrategy elem_strgy in spawns)
                elem_strgy.SpawnCycle = elem_strgy.SpawnCycle - coolDown;

            elem_gadget.ChangeStrategy();
        }
    }
}
