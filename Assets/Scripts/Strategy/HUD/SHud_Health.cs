using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHud_Health : HudStrategy
{
    private Slider healthSlider;
    private Unit playerUnit;

    protected override void Initialize()
    {
        base.Initialize();

        healthSlider = GetComponent<Slider>();
        
    }

    protected override void Initialized()
    {
        base.Initialized();

        playerUnit = GameManager.Instance.Player.GetComponent<Unit>();
    }

    public override void Action()
    {
        healthSlider.value = Mathf.Max(playerUnit.CurHealth / playerUnit.MaxHealth, 0f);
    }
}
