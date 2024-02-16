using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBuffer : MonoBehaviour
{
    void OnEnable()
    {
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();

        if (trailRenderer)
            trailRenderer.Clear();
    }
}
