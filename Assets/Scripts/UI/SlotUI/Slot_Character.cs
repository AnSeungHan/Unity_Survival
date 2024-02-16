using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Character : SlotUI
{
    private Transform targetmesh;

    protected override void Initialize()
    {
        base.Initialize();

        targetmesh = GameManager.Instance.Player.transform.Find("Mesh");
    }

    protected override void Notify_OnClick()
    {
        int cnt = targetmesh.childCount - 1;
        
        for (int i = 0; i < cnt; ++i)
        {
            Transform child = targetmesh.GetChild(i);

            if (i == slotIdx)
            {
                child.gameObject.SetActive(true);
                PlayerPrefs.SetInt("PlayerMeshIndex", i);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
