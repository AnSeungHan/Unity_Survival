using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Ctr : MonoBehaviour
{
    private Transform   m_Transform;
    public  Transform   m_TargetTransform;

    public  Vector3     Offset;
    public  Quaternion  Rotation;

    void Start()
    {
        m_Transform = GetComponent<Transform>();

        //Rotation = m_Transform.rotation = Quaternion.Euler(90f, 360f, 0f);
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (0.0f != h || 0.0f != v)
        {
            Vector3 Dir = new Vector3(h, 0.0f, v);
            Vector3 Pos = Dir * 10.0f * Time.deltaTime;

            m_Transform.position += Pos;
        }
    }

    void LateUpdate()
    {

    }

    void FixedUpdate()
    {
  
    }

    void OnValidate()
    {

    }
}
