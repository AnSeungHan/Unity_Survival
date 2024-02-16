using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHud_ExpText : HudStrategy
{
    private Text text;

    protected override void Initialize()
    {
        base.Initialize();

        text = GetComponent<Text>();
    }

    public override void Action()
    {
        string tx 
            = GameManager.Instance.CurExp
            + " / "
            + GameManager.Instance.NextExp;

        text.text = tx;
    }
}
