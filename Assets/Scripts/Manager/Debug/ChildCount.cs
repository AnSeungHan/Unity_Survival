using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCount : MonoBehaviour
{
    public int count = 0;

    public void UpdateCount()
    {
        count = transform.childCount;
    }

}
