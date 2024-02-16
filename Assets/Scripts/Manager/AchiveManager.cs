using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class AchiveManager 
        : Singleton<AchiveManager>
    {
        [System.Serializable]
        public enum Achive
        { 
            None,
            KillScore,
            DeadCount,
            WinCount,
            MaxLevel,

            END
        };

        [System.Serializable]
        struct AchiveCondition
        {
            public Achive   achiveType;
            public int      cnt;
        }

        [System.Serializable]
        struct LockData
        {
            public string               name;
            public int                  characterIndex;
            public bool                 isUnlock;
            public AchiveCondition      condition;
        }

    class AchiveList
    {
        private List<KeyValuePair<string, int>> countList = new List<KeyValuePair<string, int>>();

        public void Add(string _Name, int _Count)
        {
            countList.Add(new KeyValuePair<string, int>(_Name, _Count));
        }

        public KeyValuePair<string, int> Pop()
        {
            if (0 == countList.Count)
                return new KeyValuePair<string, int>("NoData", 0);

            KeyValuePair<string, int> result = countList[0];
            countList.RemoveAt(0);

            return result;
        }

        public void Sort()
        {
            countList.Sort(new Comparison<KeyValuePair<string, int>>((pair1, pair2) => 
            {
                return pair1.Value.CompareTo(pair2.Value);
            }));
        }
    }


    [SerializeField]
    private LockData[]                     characterDataInfo;
    private List<GameObject>               selectUIList      = new List<GameObject>();
    private Dictionary<Achive, AchiveList> reservationAchive = new Dictionary<Achive, AchiveList>();

    protected override void Initialize()
    { 
        base.Initialize();

        if (PlayerPrefs.HasKey("MyData"))
        {
            for (int i = 0; i < characterDataInfo.Length; ++i)
            {
                if (1 == PlayerPrefs.GetInt(characterDataInfo[i].name))
                {
                    characterDataInfo[i].isUnlock = true;
                }
            }

            return;
        }

        PlayerPrefs.SetInt("MyData", 1);

        foreach (LockData elem in characterDataInfo)
        {         
            PlayerPrefs.SetInt(elem.name, (elem.isUnlock) ? (1) : (0));
        }
    }

    protected override void Initialized()
    {
        base.Initialized();

        ReservationAchive();
    }

    public void Init(CharacterSlotGroup SlotGroup)
    {
        foreach (LockData elem in characterDataInfo)
        {
            Transform   uiTr   = SlotGroup.transform.GetChild(elem.characterIndex).Find("Character");
            Image       image  = uiTr.GetComponent<Image>();

            if (!elem.isUnlock)
                image.color = Color.black;

            selectUIList.Add(uiTr.gameObject);

            //PlayerPrefs.SetInt(elem.name, 1);
        }
    }

    public bool CheckAchive(int _Idx)
    {
        foreach (LockData elem in characterDataInfo)
        {
            if (elem.characterIndex == _Idx)
            {
                return elem.isUnlock;
            }
        }

        return false;
    }

    private void ReservationAchive()
    {
        foreach (LockData elem in characterDataInfo)
        {
            AchiveCondition condition = elem.condition;

            if (Achive.None == condition.achiveType)
                continue;

            if (!reservationAchive.ContainsKey(condition.achiveType))
                reservationAchive.Add(condition.achiveType, new AchiveList());

            reservationAchive[condition.achiveType].Add(elem.name, condition.cnt);
        }

        foreach (KeyValuePair<Achive, AchiveList> elem in reservationAchive)
            elem.Value.Sort();
    }

    public void BindAchiveCondition()
    {
        /*for (int i = 0; i < characterDataInfo.Length; ++i)
        {
            AchiveCondition condition = characterDataInfo[i].condition;

            if (0 == PlayerPrefs.GetInt(characterDataInfo[i].name))
                BindAchiveCondition(condition.achiveType, condition.cnt, characterDataInfo[i].name);
        }*/

        foreach (KeyValuePair<Achive, AchiveList> elem in reservationAchive)
        {
            KeyValuePair<string, int> data = elem.Value.Pop();

            if ("NoData" == data.Key)
                return;

            BindAchiveCondition(elem.Key, data.Value, data.Key);
        }
    }

    private void BindNextAchiveCondition(Achive AchiveType)
    {
        KeyValuePair<string, int> data = reservationAchive[AchiveType].Pop();

        if ("NoData" == data.Key)
            return;

        BindAchiveCondition(AchiveType, data.Value, data.Key);
    }

    private void BindAchiveCondition(Achive AchiveType, int Count, string AchiveName)
    {
        Unit player = GameManager.Instance.Player;

        switch (AchiveType)
        {
            case Achive.KillScore:
            {
                player.Subscribe("OnKill", new OnTrrigerDeleteObserver(()=> 
                {
                    if (Count <= PlayerPrefs.GetInt("KillScore"))
                    {
                        Debug.Log("Achive [" + AchiveName + "] (KillScore) : " + Count);

                        PlayerPrefs.SetInt(AchiveName, 1);
                        BindNextAchiveCondition(AchiveType);

                        return true;
                    }

                    return false;

                }, false));
            }
            break;

            case Achive.DeadCount:
            {
                player.Subscribe("OnDead", new OnTrrigerObserver(() =>
                {
                    if (Count <= PlayerPrefs.GetInt("DeadCount"))
                    {
                        PlayerPrefs.SetInt(AchiveName, 1);
                        BindNextAchiveCondition(AchiveType);
                    }

                }, false));
            }
            break;

            case Achive.WinCount:
            {
                player.Subscribe("OnWin", new OnTrrigerObserver(() =>
                {
                    if (Count <= PlayerPrefs.GetInt("WinScore"))
                    {
                        PlayerPrefs.SetInt(AchiveName, 1);
                        BindNextAchiveCondition(AchiveType);
                    }

                }, false));
            }
            break;

            case Achive.MaxLevel:
            {
                player.Subscribe("OnLevelUp", new OnTrrigerObserver(() =>
                {
                    if (Count == PlayerPrefs.GetInt("MaxLevel"))
                    {
                        PlayerPrefs.SetInt(AchiveName, 1);
                        BindNextAchiveCondition(AchiveType);
                    }

                }, false));
            }
            break;
        }
    }
}
