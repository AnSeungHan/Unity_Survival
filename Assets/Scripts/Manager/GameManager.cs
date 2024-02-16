using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager 
    : Singleton<GameManager>
{
    [Header("======= GameMode ======")]
    public bool isNoLevelUpMode = false;
    public bool isNoDeathMode   = false;

    [Header("======= User ======")]
    [SerializeField]
    private Unit            player;
    [SerializeField]
    private Camera          playerFollowCamera;
    [SerializeField]
    private GameObject      enemyCleaner;

    public Unit   Player                
    { 
        get { return player; }
        set 
        { 
            player = value;

            Transform cleaner = player.transform.Find("EnumyCleaner");
            if (cleaner)
                enemyCleaner = cleaner.gameObject;

            Transform camera = player.transform.Find("Camera");
            if (camera)
                playerFollowCamera = camera.GetComponent<Camera>();
        } 
    }
    public Camera PlayerFollowCamera    { get { return playerFollowCamera; } }

    [SerializeField]
    private Gadget[]        gadgetList;
    [SerializeField]
    private Gadget[]        lastItemList;
    private List<Gadget>    activateGadgetList = new List<Gadget>();
    private int[]           randomIdx;

    [SerializeField]
    private float   maxGameTime = 120f;
    private float   curGameTime;
    private int     killScore = 0;
    private float   curExp;
    private float   nextExp;
    private bool    isLive;
    private bool    isResurrection = false;

    public float        MaxGameTime           { get { return maxGameTime; } }
    public float        CurGameTime           { get { return curGameTime; } }
    public int          KillScore             { get { return killScore; } }
    public float        CurExp                { get { return curExp;    } }
    public float        NextExp               { get { return nextExp;   } }
    public bool         IsLive                { get { return isLive; } }
    public bool         IsResurrection        { get { return isResurrection; } }

    private Queue<float> expSheet = new Queue<float>();

    public void AddKillScore(int _AddScore)
    {
        killScore += _AddScore;

        player.Kill(_AddScore);
    }

    public void AddExp(float _AddData)
    {
        if (!isLive)
            return;

        if (!Player)
            return;

        if (0 >= expSheet.Count)
            return;

        curExp += _AddData;

        while (curExp >= nextExp)
        {
            curExp -= nextExp;
            nextExp = expSheet.Dequeue();

            player.CurLevel = player.CurLevel + 1;

            if (!isNoLevelUpMode)
                player.UnitLevelUp();
        }

        if (0 >= expSheet.Count)
            curExp = nextExp;
    }

    public void MakeRandomItemSlot()
    {
        int[] randomSlot = new int[activateGadgetList.Count];

        for (int i = 0; i < activateGadgetList.Count; ++i)
            randomSlot[i] = i;

        GameMath.RandomMix<int>(ref randomSlot, randomSlot.Length);
        int maxLength = Mathf.Min(UIManager.Instance.SelectSlot.Count, activateGadgetList.Count);

        for (int i = 0; i < maxLength; ++i)
        {
            randomIdx[i] = randomSlot[i];
        }
    }

    public Gadget Get_GadgetInfo(int _Idx)
    {
        if (activateGadgetList.Count <= randomIdx[_Idx])
        {
            Debug.Log("Check");
        }

        return (activateGadgetList.Count <= randomIdx[_Idx])
            ? (null)
            : (activateGadgetList[randomIdx[_Idx]]);
    }

    public void ChoiceGadget(int _Idx)
    {
        Gadget newGadget = Get_GadgetInfo(_Idx);
        UIManager.Instance.EquipItemSlot.AddItems
        (
            newGadget,
            newGadget.LevelDatatSheet.UITexture
        );
        UIManager.Instance.ItemSlotActivate(false);

        Player.AddGadget(Get_GadgetInfo(_Idx).gameObject);
    }

    public void PauseGame(bool _Action)
    {
        if (_Action)
        {
            Time.timeScale = 0;
        }
        else 
        {
            Time.timeScale = 1f;
        }
    }

    public void RemoveGadgetList(string _GadgetPrefabName)
    {
        Gadget iter_find = activateGadgetList.Find((Gadget gadget) => 
        {
            return (gadget.name == _GadgetPrefabName);
        });

        if (null == iter_find)
            return;

        Debug.Log("RemoveGadgetList : [ " + iter_find.name + " ]");
        activateGadgetList.Remove(iter_find);
    }

    public void Reserrection()
    {
        player.Reserrection();
    }

    public void GameStart()
    {
        isLive = true;
        PauseGame(false);

        player.StartGame();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(2f);

        player.Lose();
        UIManager.Instance.Lose();

        PauseGame(true);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(2f);

        player.Win();
        UIManager.Instance.Win();

        PauseGame(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void LoadScene(string _SceneTag)
    {
        SceneManager.LoadScene(_SceneTag);
    }

    protected override void Initialize()
    {
        base.Initialize();

        //Application.targetFrameRate = 60;

        for (int i = 1; i <= 41; ++i)
            expSheet.Enqueue(100 + (i * 100));

        nextExp = expSheet.Dequeue();

        foreach (Gadget elem in gadgetList)
            activateGadgetList.Add(elem);
    }

    protected override void Initialized()
    {
        base.Initialized();

        isLive = true;
        player.MaxLevel = expSheet.Count;
        player.StartGame();

        randomIdx = new int[UIManager.Instance.SelectSlot.Count];
    }

    void Update()
    {
        if (!isLive)
            return;

        curGameTime += Time.deltaTime;

        if (curGameTime > maxGameTime)
        {
            curGameTime = maxGameTime;
            GameVictory();
        }
    }
}
