using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servant : ObjectBase
{
    protected override void Initialize()
    {
        base.Initialize();

        offsetTransform = transform.Find("AimPos");

        if (!offsetTransform)
            offsetTransform = transform;
    }

    protected override void Initialized()
    {
        base.Initialized();
    }

    public override void BeginPlay()
    {
        foreach (IStrategy elem in attackStrategy)
            elem.SetActivate(false);          
       
        base.BeginPlay();

        if (transform.parent)
        {
            ObjectBase ownerBase = transform.parent.GetComponent<ObjectBase>();

            ownerBase.Subscribe("OnChangeStrategy", new OnTrrigerObserver(() =>
            {
                ParentStrategyClone(transform.parent.GetComponents<AttackStrategy>());
                ParentStrategyClone(transform.parent.GetComponents<MovementStrategy>());
                ParentStrategyClone(transform.parent.GetComponents<SpawnStrategy>());

                attackStrategy   = GetComponents<AttackStrategy>();
                movementStrategy = GetComponent<MovementStrategy>();

                foreach (IStrategy elem in attackStrategy)
                {
                    elem.SetActivate(false);               
                }
            }
            , false));
        }

        tag = transform.parent.tag;
    }

    public override void Clear()
    {
        base.Clear();

        IStrategy[] strategys = GetComponents<IStrategy>();

        foreach (IStrategy elem in strategys)
        {
            if (false == elem.IsActivate())
                continue;

            DestroyImmediate(elem);
        }    
    }

    public void LevelUp()
    {
        Notify("OnLevelUp");
    }

    private void ParentStrategyClone(IStrategy[] _Strategys)
    {
        List<IStrategy> myStrategies = new List<IStrategy>(GetComponents<IStrategy>());

        foreach (IStrategy elem in _Strategys)
        {
            if (false != elem.IsActivate())
                continue;

            IStrategy iter_find = myStrategies.Find((IStrategy _strategy) => { return (_strategy.GetType() == elem.GetType()); });

            if (iter_find)
            {
                iter_find.Clone(elem);
            }
            else 
            {
                IStrategy com = gameObject.AddComponent(elem.GetType()) as IStrategy;
                com.Clone(elem);
            }
        }
    }
}


