using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget : MonoBehaviour
{
    [SerializeField]
    private Transform       targetTransform;
    [SerializeField]
    private Vector3         offset;

    void Update()
    {
        Vector3 nextPos = targetTransform.position;
        nextPos -= offset;

        transform.position = nextPos;
    }
}
