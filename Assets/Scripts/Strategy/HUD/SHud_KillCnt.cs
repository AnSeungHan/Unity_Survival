using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHud_KillCnt : HudStrategy
{
    private Text        killScoreText;

    protected override void Initialize()
    {
        base.Initialize();

        killScoreText = GetComponent<Text>();
    }

    public override void Action()
    {
        killScoreText.text = string.Format("{0:F0} ", GameManager.Instance.KillScore);
    }
}
