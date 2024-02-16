using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumyCleaner : MonoBehaviour
{
    [SerializeField]
    private float radius = 100f;

    void OnEnable()
    {
        RaycastHit[] ray = Physics.SphereCastAll(transform.position, radius, Vector3.forward, 0f, 128);

        foreach (RaycastHit elem in ray)
        {
            Unit unit = elem.transform.GetComponent<Unit>();

            if (!unit)
                continue;

            if ("Monster" != unit.gameObject.tag)
                continue;

            unit.DeleteObsject();
        }

    }
}
