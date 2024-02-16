using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotGroup : UIGroup
{
    protected override void Initialize()
    {
        base.Initialize();

        UIManager.Instance.SlotGroup = this;
    }

    public override void UIGroupEnable()
    {
        base.UIGroupEnable();

        GameManager.Instance.MakeRandomItemSlot();
        GameManager.Instance.PauseGame(true);
    }

    public override void UIGroupDisable()
    {
        base.UIGroupDisable();

        GameManager.Instance.PauseGame(false);
    }
}
