using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotGroup : UIGroup
{
    [SerializeField]
    private Unit            targetUnit;
    [SerializeField]
    private SlotUI          prefabUI;

    protected override void Initialize()
    {
        base.Initialize();

        Transform   targetTransform = targetUnit.transform.Find("Mesh");
        int         cnt             = targetTransform.childCount - 1;

        for (int i = 0; i < cnt; ++i)
        { 
            SlotUI uiSlot = new DefaultBuilder(prefabUI.gameObject)
                .Set_Parent(transform)
                .Set_Scale(Vector3.one)
                .Build()
                .GetComponent<SlotUI>();

            uiSlot.SlotIndex = i;

            if (!AchiveManager.Instance.CheckAchive(i))
            {
                Button btn = uiSlot.GetComponent<Button>();

                if (null == btn)
                    continue;

                btn.enabled = false;
            }
        }

        AchiveManager.Instance.Init(this);
        gameObject.transform.root.Find("Character Select").gameObject.SetActive(false);
    }

    /*public override void UIGroupEnable()
    {
        base.UIGroupEnable();
    }

    public override void UIGroupDisable()
    {
        base.UIGroupDisable();
    }*/
}
