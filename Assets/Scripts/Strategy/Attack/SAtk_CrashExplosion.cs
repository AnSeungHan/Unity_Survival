using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAtk_CrashExplosion : AttackStrategy
{
    [SerializeField]
    private float explosionRange;

    public override void Clone(IStrategy _Other)
    {
        base.Clone(_Other);

        SAtk_CrashExplosion other = _Other as SAtk_CrashExplosion;

        if (null == other)
            return;

        explosionRange = other.explosionRange;
    }

    public override void LevelUp(string _ValueName, string _Value)
    {
        base.LevelUp(_ValueName, _Value);

        switch (_ValueName)
        {
            case "explosionRange": explosionRange = float.Parse(_Value); break;
        }
    }

    protected override void OnCrash(ref float _TotalDamage)
    {
        base.OnCrash(ref _TotalDamage);

        RaycastHit[] ray = MakeRaycastHit(explosionRange, targetLayer);

        foreach (RaycastHit elem in ray)
        {
            ObjectBase objBase = elem.transform.gameObject.GetComponent<ObjectBase>();

            if (null == objBase)
                continue;

            objBase.TakeDamage(impactDamage);
        }

        Active_AttackEffect("OnCrash");
        Activate_Sound("OnCrash");
    }
}

