using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPsv_RecoveryHealth : PassiveStrategy
{
    [SerializeField]
    private float   tickTime;
    [SerializeField]
    private float   addTickHealth;
    private float   time;

    public override void LevelUp(string _ValueName, string _Value)
    {
        base.LevelUp(_ValueName, _Value);

        switch (_ValueName)
        {
            case "tickTime"      : tickTime      = float.Parse(_Value); break;
            case "addTickHealth" : addTickHealth = float.Parse(_Value); break;
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time < tickTime)
            return;

        ownerParenttUnit.CurHealth = ownerParenttUnit.CurHealth + addTickHealth;
        time = 0f;
    }
}
