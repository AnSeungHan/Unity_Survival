using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHud_Timer : HudStrategy
{
    private Text      timeText;

    protected override void Initialize()
    {
        base.Initialize();

        timeText = GetComponent<Text>();
    }

    public override void Action()
    {
        float remainTime = GameManager.Instance.MaxGameTime - GameManager.Instance.CurGameTime;
        int min = Mathf.FloorToInt(remainTime / 60f);
        int sec = Mathf.FloorToInt(remainTime % 60f);

        timeText.text = string.Format("{0:D2} : {1:D2} ", min, sec);
    }
}
