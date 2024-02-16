using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHud_Exp : HudStrategy
{
    private Slider      expSlider;

    protected override void Initialize()
    {
        base.Initialize();

        expSlider = GetComponent<Slider>();
    }

    public override void Action()
    {
        float curExp = GameManager.Instance.CurExp / GameManager.Instance.NextExp;

        expSlider.value = curExp;
    }
}
