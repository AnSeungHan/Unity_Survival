using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
    : Singleton<UIManager>
{
    private delegate void DefaultDelegate();

    [Header("======= GameMode ======")]
    [SerializeField]
    public bool isStartActivateSlotMode = false;

    [Header("======= Upgrad UI ======")]
    [SerializeField]
    private UIGroup         slotGroup;
    [SerializeField]
    private List<SlotUI>    selectSlot = new List<SlotUI>();
    [SerializeField]
    private EquipItem       equipItemSlot;

    public UIGroup SlotGroup
    {
        get { return slotGroup; }
        set 
        {
            if (null != selectSlot)
                selectSlot.Clear();

            slotGroup = value;
            FindSlots(slotGroup.gameObject);

            slotGroup.gameObject.SetActive(false);
        } 
    }
    public List<SlotUI> SelectSlot      { get { return selectSlot; } }
    public EquipItem    EquipItemSlot   { get { return equipItemSlot; } set { equipItemSlot = value; } }

    [Header("======= Main UI ======")]
    private Dictionary<string, HudStrategy> hudStrateges = new Dictionary<string, HudStrategy>();
    [SerializeField]
    private Result                          resultUI;
    [SerializeField]
    private VirtualJoystick_ver2 joystick;
    //private GameObject                        joystick;

    public Result           ResultUI { get { return resultUI; } set { resultUI = value; resultUI.gameObject.SetActive(false); } }
    public VirtualJoystick_ver2  Joystick { get { return joystick; } set { joystick = value; } }
    //public GameObject Joystick { get { return joystick; } set { joystick = value; } }


    private DefaultDelegate     OnAfterStart;


    private void FindSlots(GameObject _Target)
    {
        int cnt = _Target.transform.childCount;

        for (int i = 0; i < cnt; ++i)
        {
            FindSlots(_Target.transform.GetChild(i).gameObject);

            Transform childTr = _Target.transform.GetChild(i);
            SlotUI slot = childTr.GetComponent<SlotUI>();

            if (null == slot)
                continue;

            slot.SlotIndex = selectSlot.Count;
            selectSlot.Add(slot);
        }
    }

    public void AddHUD(string _Tag, HudStrategy _HudStrategy)
    {
        hudStrateges.Add(_Tag, _HudStrategy);
    }

    public void ItemSlotActivate(bool Action)
    {
        if (Action)
        {
            slotGroup.UIGroupEnable();

            foreach (SlotUI elem in selectSlot)
                elem.UIEnable();

            if (Joystick)
            { 
                Joystick.gameObject.SetActive(false);
                //Joystick.ResetJoystick();
            }
        }
        else 
        {
            slotGroup.UIGroupDisable();

            if (Joystick)
                Joystick.gameObject.SetActive(true);
        }
    }

    public void UpgradeSlotDisable()
    {
        for (int i = 0; i < 3; ++i)
        {
            selectSlot[i].ReadyDisable();
        }
    }

    public void Lose()
    {
        resultUI.gameObject.SetActive(true);
        resultUI.Lose();
    }

    public void Win()
    {
        resultUI.gameObject.SetActive(true);
        resultUI.Lose();
    }

    protected override void Initialize()
    {
        base.Initialize();

        /*Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas)
        {
            canvas.gameObject.SetActive(true);

            int cnt = canvas.transform.childCount;
            for (int i = 0; i < cnt; ++i)
            {
                Transform child =canvas.transform.GetChild(i);
                child.gameObject.SetActive(true);
            }
        }*/

        if (resultUI)
            resultUI.gameObject.SetActive(false);
    }

    protected override void Initialized()
    {
        base.Initialized();

        if (isStartActivateSlotMode)
        { 
            OnAfterStart = () => 
            { 
                ItemSlotActivate(true); 
            };
        }

        //GameManager.Instance.Player.StartGame();
    }

    void Update()
    {
        if (null != OnAfterStart)
        { 
            OnAfterStart();
            OnAfterStart = null;
        }
    }
}
