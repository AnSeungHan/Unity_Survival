using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Unit 
    : ObjectBase
{
    [SerializeField]
    private SoundData[] soundDatas;

    protected Rigidbody  rigidBody;
    protected Animator   animator;
    protected Collider   collider;

    private List<Gadget> gadgetSlot = new List<Gadget>();

    [SerializeField]
    private float                   maxHealth = 100f;
    [SerializeField]
    private float                   curHealth;
    [SerializeField]
    private bool                    isDead = false;
    [SerializeField]
    private MonsterLevelDataSheet   levelData;
    private WaitForFixedUpdate      wait;

    public bool                  IsDead       { get { return isDead;    } }
    public float                 CurHealth    { get { return curHealth; } set { curHealth = (curHealth + value <= maxHealth) ? (curHealth + value) : (maxHealth); } }
    public float                 MaxHealth    { get { return maxHealth; } set { maxHealth = value; curHealth = maxHealth; } }
    public List<Gadget>          Gadgets      { get { return gadgetSlot; } }
    public MonsterLevelDataSheet LevelData    { get { return levelData; } }

    protected override void Initialize()
    {
        base.Initialize();

        rigidBody = GetComponent<Rigidbody>();
        animator  = GetComponent<Animator>();
        collider  = GetComponent<Collider>();
        wait      = new WaitForFixedUpdate();

        offsetTransform  = transform.Find("GadgetPos");

        if (!offsetTransform)
            offsetTransform = transform;

        curHealth = maxHealth;     

        /*for (int i = 0; i < offsetTransform.childCount; ++i)
        {
            Transform childTr   = offsetTransform.GetChild(i);
            Gadget    newGadget = childTr.GetComponent<Gadget>();

            if (null == newGadget)
                continue;

            gadgetSlot.Add(newGadget);
            newGadget.Equip();
        }*/

        switch(tag)
        {
            case "Monster":
            {
                Subscribe("OnDead", new OnTrrigerObserver(()=>
                {
                    int daedCnt = PlayerPrefs.GetInt("DeadCount");
                    PlayerPrefs.SetInt("DeadCount", daedCnt + 1);

                    GameManager.Instance.AddExp(10);
                    GameManager.Instance.AddKillScore(1);

                    isDead = true;
                    rigidBody.isKinematic = true;
                    collider.enabled = false;

                    StopCoroutine(KnockBack());

                    foreach (AttackStrategy elem in attackStrategy)
                        elem.enabled = false;

                    if (movementStrategy)
                        movementStrategy.enabled = false;

                    DeleteObsject();
                }
                , false));

                Subscribe("OnImpact", new OnTrrigerObserver(() =>
                {
                    if (!isDead)
                        StartCoroutine(KnockBack());
                }
                , false));
            }
            break;

            case "User":
            {
                if (!GameManager.Instance.isNoDeathMode)
                { 
                    Subscribe("OnDead", new OnTrrigerObserver(() =>
                    {
                        foreach (AttackStrategy elem in attackStrategy)
                            elem.enabled = false;

                        if (movementStrategy)
                            movementStrategy.enabled = false;

                        GameManager.Instance.GameOver();
                    }
                    , false));
                }
             
                Subscribe("OnUnitLevelUp", new OnTrrigerObserver(() =>
                {
                    UIManager.Instance.ItemSlotActivate(true);
                }
                , false));

                if ("Start Scene" != SceneManager.GetActiveScene().name)
                    AchiveManager.Instance.BindAchiveCondition();

                GameManager.Instance.Player = this;
                MeshSetting();
            }
            break;
        }
    }

    protected override void Initialized()
    {
        base.Initialized();

        for (int i = 0; i < offsetTransform.childCount; ++i)
        {
            Transform childTr = offsetTransform.GetChild(i);
            Gadget newGadget = childTr.GetComponent<Gadget>();

            if (null == newGadget)
                continue;

            gadgetSlot.Add(newGadget);
            newGadget.BeginPlay();
            newGadget.Equip();

            if ("User" == tag)
                UIManager.Instance.EquipItemSlot.AddItems(newGadget);
        }

        /*foreach (Gadget elem in gadgetSlot)
        { 
            elem.BeginPlay();

            if ("User" == tag)
                UIManager.Instance.EquipItemSlot.AddItems(elem);
        }*/

        Notify("OnEquip");
    }

    public override void TakeDamage(float _Damage)
    {
        float totalDamage = _Damage;

        Notify("OnImpact");
        Notify("OnTakeDamage", ref totalDamage);
        Activate_Sound("OnImpact", SFXType.SFX_HIT);       

        curHealth -= totalDamage;

        new DefaultBuilder("DamageText (TMP)", "UIPrefab/Unit")
            .Set_Position(transform.position)
            .Set_Rotation(Quaternion.Euler(45f, 0f, 0f))
            .Set_Parent(null)
            .Build()
            .GetComponent<SHud_Damage>()
            .BindUI(totalDamage, ("User" != tag) ? (Color.yellow) : (Color.red));

        if (0 >= curHealth)
        {
            Dead();
            return;
        }
    }

    public void AddGadget(GameObject _NewGadgetPrefab)
    {
        Gadget gadget = gadgetSlot.Find(gadget => (gadget.PrefabName == _NewGadgetPrefab.name));

        if (gadget)
        {
            gadget.LevelUp(gadget.CurLevel + 1);
            Notify("OnLevelUp");
        
            return;
        }

        Gadget newGadget = new Builder<Gadget>(_NewGadgetPrefab)
            .Set_Parent(offsetTransform)
            .Set_Position(offsetTransform.transform.position)
            .Build()
            .GetComponent<Gadget>();

        gadgetSlot.Add(newGadget);
        newGadget.Equip();
        Notify("OnEquip");
    }

    public void UnitLevelUp()
    {
        Notify("OnUnitLevelUp");
    }

    public void Win()
    {
        int winCnt = PlayerPrefs.GetInt("WinScore");
        PlayerPrefs.SetInt("WinScore", winCnt + 1);

        Notify("OnWin");
    }

    public void Lose()
    {
        Notify("OnLose");
    }

    public void Kill(int _AddScore)
    {
        int killCnt = PlayerPrefs.GetInt("KillScore");
        PlayerPrefs.SetInt("KillScore", killCnt + 1);

        Notify("OnKill");
    }

    public void Reserrection()
    {
        Notify("OnReserrection");
    }

    public void ReadyStart()
    {
        Notify("OnReadyStart");
    }

    public void StartGame()
    {
        Notify("OnStartGame");
    }

    protected void Activate_Sound(string _NotifyTag, SFXType _Type = SFXType.NORMAL)
    {
        if (null == soundDatas)
            return;

        foreach (SoundData elem in soundDatas)
        {
            if (elem.notifyTag != _NotifyTag)
                continue;

            SoundManager.Instance.PlaySFX(elem.soundName, _Type);
        }
    }

    public int GadgetLevel(GameObject _NewGadgetPrefab)
    {
        Gadget gadget = gadgetSlot.Find(gadget => (gadget.PrefabName == _NewGadgetPrefab.name));

        return (gadget)
            ? (gadget.CurLevel + 1)
            : (0);
    }

    private void MeshSetting()
    {
        Transform   targetmesh  = transform.Find("Mesh");
        int         cnt         = targetmesh.childCount - 1;
        int         meshIdx     = PlayerPrefs.GetInt("PlayerMeshIndex");

        for (int i = 0; i < cnt; ++i)
        {
            Transform child = targetmesh.GetChild(i);
            child.gameObject.SetActive((i == meshIdx));
        }
    }

    void OnEnable()
    {
        isDead    = false;
        curHealth = maxHealth;

        rigidBody.isKinematic = false;
        collider.enabled      = true;
    }

    IEnumerator KnockBack()
    {
        movementStrategy.enabled = false;
        float startTime = Time.time;

        Vector3 PlayerPos = GameManager.Instance.Player.transform.position;
        Vector3 Dir       = transform.position - PlayerPos;

        while (Time.time < startTime + 0.05f)
        {                 
            rigidBody.AddForce(Dir.normalized * 8f, ForceMode.Impulse);

            yield return wait;
        }

        movementStrategy.enabled = true;
    }
}