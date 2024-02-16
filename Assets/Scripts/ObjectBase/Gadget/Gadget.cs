using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gadget : ObjectBase
{
    [SerializeField]
    protected LevelDataSheet    levelDatatSheet;
    private   Transform         parentTransform;

    public LevelDataSheet   LevelDatatSheet { get { return levelDatatSheet; } }

    protected override void Initialize() 
    {
        base.Initialize();

        if (levelDatatSheet)
            maxLevel = levelDatatSheet.MaxLevel() - 1;

        offsetTransform = transform.Find("AimPos");
        if (!offsetTransform)
            offsetTransform = transform;

        gameObject.tag = "Gadget";

        /*Subscribe("OnLevelMax", new OnTrrigerObserver(() =>
        {
            Debug.Log("RemoveGadgetList : [ " + prefabName + " ]");

            GameManager.Instance.RemoveGadgetList(prefabName);
        },
        false));*/
    }

    public override void BeginPlay()
    {
        base.BeginPlay();

        prefabName = name;

        Subscribe("OnLevelMax", new OnTrrigerObserver(() =>
        {
            GameManager.Instance.RemoveGadgetList(prefabName);
        },
        false));
    }

    protected override void Initialized()
    {
        base.Initialized();

        foreach (IStrategy elem in attackStrategy)
        {
            if (false == elem.IsActivate())
                elem.enabled = false;
        }
    }

    public void LevelUp(int _NextLevel)   
    {
        if (!levelDatatSheet || 0 == levelDatatSheet.MaxLevel())
        {
            Notify("OnLevelMax");
            //GameManager.Instance.RemoveGadgetList(name);
            return;
        }

        LevelChange(_NextLevel);
        Notify("OnLevelUp");
    }

    public void Equip()
    {
        if (!levelDatatSheet || 0 == levelDatatSheet.MaxLevel())
        {
            Notify("OnLevelMax");
            //GameManager.Instance.RemoveGadgetList(name);
            return;
        }

        LevelChange(0);
        Notify("OnEquip");
    }

    public void ChangeStrategy()
    {
        Notify("OnChangeStrategy");
    }

    public string NextLevelInfo(int _NextLevel)
    {
        if (!levelDatatSheet)
            return "[ None Info ]";

        return levelDatatSheet.NextLevelInfo(_NextLevel);
    }

    public abstract List<AttackStrategy> GetAttackStrategys();

    protected virtual void LevelChange(int _NextLevel)
    {
        LevelUpData data = new LevelUpData();
        if (!levelDatatSheet.GetData(_NextLevel, ref data))
        {
            Notify("OnLevelMax");
            //GameManager.Instance.RemoveGadgetList(name);
            return;
        }

        curLevel = _NextLevel;

        foreach (LevelDataDB db in data.DataInfo)
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

        if (1 == levelDatatSheet.MaxLevel() || curLevel >= maxLevel)
        {
            Notify("OnLevelMax");
            //GameManager.Instance.RemoveGadgetList(name);
        }
    }
}


