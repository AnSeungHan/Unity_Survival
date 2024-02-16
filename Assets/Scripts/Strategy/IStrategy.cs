using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
struct SoundData
{
    public string notifyTag;
    public string soundName;
}

public abstract class IStrategy : MonoBehaviour
{
    [SerializeField]
    private bool isActivate = true;

    [Header("====== Effect Info ======")]
    [SerializeField]
    private SoundData[] soundDatas;

    public bool IsActivate()
    {
        return isActivate;
    }

    public void SetActivate(bool _Enable)
    {
        isActivate = _Enable;
        enabled    = _Enable;
    }

    protected void Activate_Sound(string _NotifyTag)
    {
        if (null == soundDatas)
            return;

        foreach (SoundData elem in soundDatas)
        {
            if (elem.notifyTag != _NotifyTag)
                continue;

            SoundManager.Instance.PlaySFX(elem.soundName);
        }
    }

    protected virtual void Initialize()             { }
    protected virtual void Initialized()            { }
    public    virtual void BeginPlay()              { }

    public virtual void Clone(IStrategy _Other)     
    {
        soundDatas = _Other.soundDatas;
    }

    public virtual void LevelUp(string _ValueName, string _Value) { }
    public virtual void Equip()   { }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        Initialized();
    }
}


// ============================================

public abstract class ObjectBaseStrategy : IStrategy
{
    protected ObjectBase    ownerBase;
    protected float         curTime;

    protected override void Initialize()
    {
        ownerBase = gameObject.GetComponent<ObjectBase>();
    }

    protected RaycastHit[] MakeRaycastHit(float _Range, int _LayerMask)
    {
        return Physics.SphereCastAll(transform.position, _Range, Vector3.forward, 0f, _LayerMask);
    }

    protected Transform Find_Nearest(RaycastHit[] _TargetList)
    {
        Transform Result = null;
        float     Dist   = float.MaxValue;

        foreach (RaycastHit elem in _TargetList)
        {
            Vector3 TargetPos = elem.transform.position;
            Vector3 MyPos     = transform.position;
            float   CurDist   = Vector3.Distance(MyPos, TargetPos);

            if (Dist > CurDist)
            {
                Dist   = CurDist;
                Result = elem.transform;
            }
        }

        return Result;
    }

    protected Transform Find_Nearest(RaycastHit[] _TargetList, Vector3 _ExceptionDir)
    {
        Transform Result = null;
        float     Dist   = float.MaxValue;

        foreach (RaycastHit elem in _TargetList)
        {
            Vector3 TargetPos = elem.transform.position;
            Vector3 MyPos     = transform.position;
            Vector3 Pos       = TargetPos - MyPos;
            Pos.y = 0f;
            Vector3 Dir       = Pos.normalized;

            if (_ExceptionDir == Dir)
                continue;

            float CurDist = Vector3.Distance(MyPos, TargetPos);

            if (Dist > CurDist)
            {
                Dist   = CurDist;
                Result = elem.transform;
            }
        }

        return Result;
    }

    protected Transform Find_Nearest(RaycastHit[] _TargetList, int _Idx)
    {
        if (_TargetList.Length <= _Idx)
            return null;

        Array.Sort(_TargetList, (a, b) => a.distance.CompareTo(b.distance));

        return _TargetList[_Idx].transform;
    }

    protected void TargetLayer(ref int _TargetLayer, ref int _ProjectileLayer)
    {
        switch (gameObject.tag)
        {
            case "Gadget"  :
            case "Servant" :
            {
                Transform parent = transform;

                while (null != parent)
                {
                    if (parent.GetComponent<Unit>())
                        break;

                    parent = parent.parent;
                }                       

                if ("User" == parent.tag)
                {
                    _TargetLayer     = 128; // Monster
                    _ProjectileLayer = 8;   // AllyAttack
                }
                else if ("Monster" == parent.tag)
                {
                    _TargetLayer     = 64; // User
                    _ProjectileLayer = 9;  // EnumyAttack 
                }
            }
            break;

            case "Projectile":
            {
                _ProjectileLayer = transform.gameObject.layer;

                if (8 == _ProjectileLayer)
                {
                    _TargetLayer = 128; // Monster
                }
                else if (9 == _ProjectileLayer)
                {
                    _TargetLayer = 64; // User
                }
            }
            break;
        }
    }
}

// ============================================

public abstract class AttackStrategy : ObjectBaseStrategy
{
    [System.Serializable]
    public struct AtkEffect
    {
        public string          notifyTag;
        public GameObject      effectPrefab;
        public float           lifeTime;
    }

    [SerializeField]
    protected AtkEffect[]   EffectData;

    [Header("====== Attack Info ======")]
    [SerializeField]
    protected float         impactDamage = 100f;

    protected int           targetLayer;
    protected int           projectileLayer;

    public float            Damage{ get { return impactDamage; } }

    public override void LevelUp(string _ValueName, string _Value)
    {
        switch (_ValueName)
        {
            case "impactDamage" : impactDamage = float.Parse(_Value); break;
        }
    }

    public override void Clone(IStrategy _Other)
    {
        base.Clone(_Other);

        AttackStrategy other = _Other as AttackStrategy;

        if (null == other)
            return;

        impactDamage    = other.impactDamage;
        targetLayer     = other.targetLayer;
        projectileLayer = other.projectileLayer;
        EffectData      = other.EffectData;
    }

    protected override void Initialized()
    {
        base.Initialized();

        TargetLayer(ref targetLayer, ref projectileLayer);       
    }

    public override void BeginPlay()
    {
        base.BeginPlay();

        ownerBase.Subscribe("OnCrash", new OnCrashObserver(this.OnCrash, true));
    }

    protected virtual void OnCrash(ref float _TotalDamage)
    {
        _TotalDamage += impactDamage;

        Activate_Sound("OnCrash");
    }

    protected void Active_AttackEffect(string _NotifyTag)
    {
        if (null == EffectData)
            return;

        foreach (AtkEffect elem in EffectData)
        {
            if (elem.notifyTag != _NotifyTag)
                continue;

            new DefaultBuilder(elem.effectPrefab)
                    .Set_Parent(null)
                    .Set_Position(transform.position)
                    .Set_DeleteTime(elem.lifeTime)
                    .Build();
        }
    }
}

public abstract class MovementStrategy : ObjectBaseStrategy
{
    [Header("====== Movement Info ======")]
    [SerializeField]
    protected float         moveSpeed = 5f;

    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    public override void LevelUp(string _ValueName, string _Value)
    {
        switch (_ValueName)
        {
            case "moveSpeed": moveSpeed = float.Parse(_Value); break;
        }
    }

    public override void Clone(IStrategy _Other)
    {
        base.Clone(_Other);

        MovementStrategy other = _Other as MovementStrategy;

        if (null == other)
            return;

        moveSpeed = other.moveSpeed;
    }
}

public abstract class SpawnStrategy : ObjectBaseStrategy
{
    [System.Serializable]
    public struct SpawnEffect
    {
        public string       notifyTag;
        public GameObject   effectPrefab;
        public float        lifeTime;
    }

    [SerializeField]
    protected SpawnEffect[]     EffectData;

    [Header("====== Spawn Info ======")]
    [SerializeField]
    protected GameObject        projecctilePrefab;
    [SerializeField]
    protected float             spawnRange;
    [SerializeField]
    protected float             spawnCycle;
    protected int               targetLayer;
    protected int               projectileLayer;

    public float SpawnCycle { get { return spawnCycle; } set { spawnCycle = (value > 0f) ? (value) : (0f); } }

    public override void LevelUp(string _ValueName, string _Value)
    {
        switch (_ValueName)
        {
            case "spawnRange" : spawnRange = float.Parse(_Value); break;
            case "spawnCycle" : spawnCycle = float.Parse(_Value); break;
        }
    }

    public override void BeginPlay()
    {
        base.BeginPlay();

        ownerBase.Subscribe("OnLevelUp", new OnTrrigerObserver(() =>
        {
            curTime = 0;       
        }, 
        true));
    }

    public override void Clone(IStrategy _Other)
    {
        base.Clone(_Other);

        SpawnStrategy other = _Other as SpawnStrategy;

        if (null == other)
            return;

        projecctilePrefab = other.projecctilePrefab;
        spawnRange        = other.spawnRange;
        spawnCycle        = other.spawnCycle;
        projectileLayer   = other.projectileLayer;
        EffectData        = other.EffectData;
    }

    protected override void Initialized()
    {
        base.Initialized();

        TargetLayer(ref targetLayer, ref projectileLayer);
    }

    protected void Active_SpawnEffect(string _NotifyTag)
    {
        foreach (SpawnEffect elem in EffectData)
        {
            new DefaultBuilder(elem.effectPrefab)
                    .Set_Parent(ownerBase.OffsetTransform)
                    .Set_Position(ownerBase.OffsetTransform.position)
                    .Set_DeleteTime(elem.lifeTime)
                    .Build();
        }
    }
}

// ============================================

public abstract class PassiveStrategy : IStrategy
{
    protected Gadget    ownerGadet;
    protected Unit      ownerParenttUnit;

    protected override void Initialize()
    {
        ownerGadet = gameObject.GetComponent<Gadget>();
    }

    protected override void Initialized()
    {
        //ownerParenttUnit = ownerGadet.transform.parent.parent.GetComponent<Unit>();
    }

    public override void BeginPlay()
    {
        ownerParenttUnit = ownerGadet.transform.parent.parent.GetComponent<Unit>();
    }
}

// ============================================

public abstract class HudStrategy : IStrategy
{
    protected HUD ownerHud;

    protected override void Initialize()
    {
        ownerHud = gameObject.GetComponent<HUD>();

        UIManager.Instance.AddHUD(gameObject.name, this);
    }

    public abstract void Action();
}

// ============================================