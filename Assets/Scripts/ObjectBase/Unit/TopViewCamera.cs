using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewCamera : MonoBehaviour
{
    [SerializeField]
    private Transform       targetTransform;
    [SerializeField]
    private Vector3         offset;

    void Update()
    {
        Vector3 nextPos = new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z);
        nextPos -= offset;

        transform.position = nextPos;
    }
}
