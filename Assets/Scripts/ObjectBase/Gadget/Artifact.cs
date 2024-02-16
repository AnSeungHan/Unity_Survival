using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : Gadget
{
    public override List<AttackStrategy> GetAttackStrategys()
    {
        List<AttackStrategy> atk = new List<AttackStrategy>();

        AttackStrategy[] servantAtks = GetComponents<AttackStrategy>();

        foreach (AttackStrategy elem_atk in servantAtks)
            atk.Add(elem_atk);

        return atk;
    }
}