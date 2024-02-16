using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHud_Level : HudStrategy
{
    private Text        levelText;
    private ObjectBase  playerObjbase;

    protected override void Initialize()
    {
        base.Initialize();

        levelText     = GetComponent<Text>();
        playerObjbase = GameManager.Instance.Player.GetComponent<ObjectBase>();
    }

    public override void Action()
    {
        if (playerObjbase.CurLevel < playerObjbase.MaxLevel)
            levelText.text = string.Format("Lv.{0:F0} ", playerObjbase.CurLevel);
        else
            levelText.text = string.Format("Lv. Max({0:F0}) ", playerObjbase.CurLevel);
    }
}
