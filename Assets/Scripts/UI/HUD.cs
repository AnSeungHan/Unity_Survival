using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private HudStrategy hudStrategy;

    void Awake()
    {
        hudStrategy = GetComponent<HudStrategy>();
    }

    void LateUpdate()
    {
        hudStrategy.Action();
    }
}
