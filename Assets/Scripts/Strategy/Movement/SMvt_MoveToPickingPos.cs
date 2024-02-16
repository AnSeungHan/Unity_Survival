using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SMvt_MoveToPickingPos : MovementStrategy
{
    private Transform           ownerTransform;
    private Rigidbody           owneRigidbody;
    private NavMeshAgent        ownerNavMeshAgent;
    private Camera              mainCamera;

    protected override void Initialized()
    {
        base.Initialize();

        ownerTransform     = gameObject.GetComponent<Transform>();
        owneRigidbody      = gameObject.GetComponent<Rigidbody>();
        ownerNavMeshAgent  = gameObject.GetComponent<NavMeshAgent>();

        Camera[] camerasInScene = FindObjectsOfType<Camera>();

        if (camerasInScene.Length > 0)
            mainCamera = camerasInScene[0];
    }

    void Update()
    {
        if (!Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.Space))
            return;
   
        RaycastHit RayHit;
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RayHit))
        {
            Vector3 Dest = RayHit.point;

            if (NavMesh.SamplePosition(Dest, out NavMeshHit NavHit, 1.0f, NavMesh.AllAreas))
            {
                ownerNavMeshAgent.SetDestination(NavHit.position);

                Debug.Log(name + " : " + NavHit.position);
            }
        }
        
    }
}
