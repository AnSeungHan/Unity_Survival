using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SlotUI : Observable
{
    protected Button  btnSlot;

    [SerializeField]
    protected int     slotIdx;

    public int SlotIndex { set { slotIdx = value; } }

    protected virtual void Initialize()  
    {
        btnSlot = GetComponent<Button>();

        Subscribe("OnClick", new OnTrrigerObserver(() => { Notify_OnClick(); }, false));
    }

    public virtual void UIEnable()       
    {
        btnSlot.interactable = true;
    }

    public virtual void UIDisable()
    {
        
    }

    protected virtual void Initialized()    { }
    protected virtual void Notify_OnClick() { }
    public    virtual void SettingSlot(GameObject _ItemPrefab) { }

    public virtual void ReadyDisable()
    {
        btnSlot.interactable = false;
    }

    public void OnClick()
    {
        Notify("OnClick");
    }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        Initialized();
    }
}
