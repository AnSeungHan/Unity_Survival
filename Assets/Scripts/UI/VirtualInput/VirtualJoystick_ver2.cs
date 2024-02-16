using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick_ver2
    : MonoBehaviour
{
    private RectTransform   laver;
    private RectTransform   rectTransform;

    void Awake()
    {
        rectTransform   = GetComponent<RectTransform>();
        laver           = transform.Find("Stick").GetComponent<RectTransform>();

        UIManager.Instance.Joystick = this;
    }

    public Vector3 InputDirection()
    {
        Vector3 InputVec = laver.position - rectTransform.position;
        //Debug.Log(InputVec);

        return new Vector3(InputVec.x, 0, InputVec.y);
    }
}
