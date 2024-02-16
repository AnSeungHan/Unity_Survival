using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase 
    : Observable
{
    protected Transform             offsetTransform;
    
    protected AttackStrategy[]      attackStrategy;
    protected MovementStrategy      movementStrategy;

    [Header("======= Effect ======")]
    [SerializeField]
    protected EffectData[]          effectData;

    [Header("======= Unit Info ======")]
    [SerializeField]
    protected string                prefabName;
    [SerializeField]
    protected int                   curLevel;
    [SerializeField]
    protected int                   maxLevel;
    [SerializeField]
    protected Vector3               moveDir;

    public Vector3        MoveDir         { get { return moveDir; } set { moveDir = value; } }
    public Transform      OffsetTransform { get { return offsetTransform; }}
    public string         PrefabName      { get { return prefabName; } set { prefabName = value; } }
    public int            CurLevel        { get { return curLevel; } set { curLevel = value; } }
    public int            MaxLevel        { get { return maxLevel; } set { maxLevel = value; } } 

    protected virtual void Initialize()
    {
        prefabName = name;

        foreach (EffectData elem in effectData)
        {
            Subscribe(elem.notifyTag, new OnTrrigerObserver(() =>
            {
                new DefaultBuilder(elem.effectPrefab)
                    .Set_Parent(null)
                    .Set_Position(transform.position)
                    .Set_DeleteTime(elem.lifeTime)
                    .Build();

            }, false));
        }
    }

    protected virtual void Initialized()
    {
        attackStrategy   = GetComponents<AttackStrategy>();
        movementStrategy = GetComponent<MovementStrategy>();
    }

    public virtual void BeginPlay()
    {
        IStrategy[] strategys = GetComponents<IStrategy>();

        foreach (IStrategy elem in strategys)
        {
            if (!elem.IsActivate())
                continue;

            elem.enabled = true;
            elem.BeginPlay();
        }
    }

    public virtual void Clear()
    {
        Unsubscribe();
    }

    public virtual void TakeDamage(float _Damage)
    {
        
    }

    public void Dead()
    {
        Notify("OnDead");
    }

    public void DeleteObsject()
    {
        if (null != prefabName)
        {
            ObjectPool.Instance.ReturnObject(prefabName, gameObject);
        }
        else
        {
            string prefanTag = name;

            if (prefanTag.EndsWith(" "))
                prefanTag.Substring(0, prefanTag.IndexOf(" "));

            ObjectPool.Instance.ReturnObject(prefanTag, gameObject);
        }
    }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        Initialized();
    }

    void OnEnable()
    {
        Initialized();
    }
}

[System.Serializable]
public struct EffectData
{
    public string       notifyTag;
    public GameObject   effectPrefab;
    public float        lifeTime;
}