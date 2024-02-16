using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supporter : Gadget
{
    [Header("======Servant Info ======")]
    [SerializeField]
    private Servant             servantPrefab;
    [SerializeField]
    private int                 maxUnitCount = 1;
    private List<GameObject>    unitList = new List<GameObject>();

    protected override void Initialized()
    {
        base.Initialized();
    }

    public override void Clear()
    {
        base.Clear();

        foreach (GameObject elem in unitList)
            DestroyImmediate(elem);
        unitList.Clear();

        IStrategy[] strategys = GetComponents<IStrategy>();

        foreach (IStrategy elem in strategys)
        {
            if (false == elem.IsActivate())
                continue;

            DestroyImmediate(elem);
        }
    }

    public override List<AttackStrategy> GetAttackStrategys()
    {
        List<AttackStrategy> atk = new List<AttackStrategy>();

        foreach (GameObject elem in unitList)
        {
            AttackStrategy[] servantAtks = elem.GetComponents<AttackStrategy>();

            foreach (AttackStrategy elem_atk in servantAtks)
                atk.Add(elem_atk);
        }

        return atk;
    }

    protected override void LevelChange(int _NextLevel)
    {
        if (null == servantPrefab)
            return;

        LevelUpData data = new LevelUpData();
        if (!levelDatatSheet.GetData(_NextLevel, ref data))
        {
            Notify("OnLevelMax");
            return;
        }

        CurLevel = _NextLevel;

        foreach (LevelDataDB db in data.DataInfo)
        {
            if ("Supporter" == db.strategyName)
            {
                switch (db.valueName)
                {
                    case "maxUnitCount" :
                        maxUnitCount = int.Parse(db.value);
                        RepositionPrefabs();
                        break;
                }
            }
            else
            {
                Component[] strategys = GetComponents(System.Type.GetType(db.strategyName));

                foreach (Component elem in strategys)
                {
                    IStrategy strategy = elem as IStrategy;

                    if (null == strategy)
                        continue;

                    strategy.LevelUp(db.valueName, db.value);
                }
            }
        }

        if (1 == levelDatatSheet.MaxLevel() || curLevel >= maxLevel)
        {
            Notify("OnLevelMax");
        }
    }

    private void RepositionPrefabs()
    {
        float Angle = 360f / maxUnitCount;

        for (int i = 0; i < maxUnitCount; ++i)
        {
            Vector3 pos = transform.position + Quaternion.AngleAxis(i * Angle, Vector3.up) * Vector3.forward;

            if (i < unitList.Count)
            {
                unitList[i].transform.position = pos;
                unitList[i].GetComponent<Servant>().LevelUp();

                // 전략들이 변경된 것에 대해서 리구릅이 되어야 할거 같은데 흠...
            }
            else
            {
                GameObject obj = new Builder<Servant>(servantPrefab.gameObject)
                    .Set_Parent(transform)
                    .Set_Position(pos)
                    .Set_Strategy(GetComponents<AttackStrategy>()  , false)
                    .Set_Strategy(GetComponents<MovementStrategy>(), false)
                    .Set_Strategy(GetComponents<SpawnStrategy>()   , false)
                    .Build();

                unitList.Add(obj);
            }          
        }
    }
}