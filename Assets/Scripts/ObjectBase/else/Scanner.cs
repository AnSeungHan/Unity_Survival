using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField]
    private float            ScanRange;
    [SerializeField]
    private LayerMask        TargetLayer;
    [SerializeField]
    private Transform        NearestTarget;
    private RaycastHit[]     Targets;


    void Update()
    {
        Targets       = Physics.SphereCastAll(transform.position, ScanRange, Vector3.forward, 0f, TargetLayer);
        NearestTarget = GetNearest();
    }
         
    Transform GetNearest()
    {
        Transform Result = null;
        float     Dist   = 1000f;

        foreach (RaycastHit elem in Targets)
        {
            Vector3 TargetPos = elem.transform.position;
            Vector3 MyPos     = transform.position;
            float   CurDist   = Vector3.Distance(MyPos, TargetPos);

            if (Dist > CurDist)
            {
                Dist   = CurDist;
                Result = elem.transform;
            }
        }

        return Result;
    }
}

