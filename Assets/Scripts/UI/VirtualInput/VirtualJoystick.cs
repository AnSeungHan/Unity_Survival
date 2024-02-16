using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick
    : MonoBehaviour
    , IBeginDragHandler
    , IDragHandler
    , IEndDragHandler
{
    [SerializeField]
    private RectTransform   laver;
    private RectTransform   rectTransform;

    [SerializeField, Range(10, 150)]
    private float laverRange;

    private Vector2 inputDirection = Vector2.zero;
    private bool    isInput = false;

    public Vector2 InputDirection { get { return inputDirection; } }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        //UIManager.Instance.Joystick = this;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        laver.anchoredPosition  = Vector2.zero;
        inputDirection          = Vector2.zero;
        isInput                 = false;
    }

    private void ControlJoyStickLever(PointerEventData eventData)
    { 
        Vector2 inputPos = eventData.position - rectTransform.anchoredPosition;
        Vector2 inputVec = (inputPos.magnitude < laverRange) ? (inputPos) : (inputPos.normalized * laverRange);

        laver.anchoredPosition = eventData.position;
        inputDirection         = inputVec / laverRange;

        Debug.Log(eventData.position + " / " + inputVec);
    }

    public void ResetJoystick()
    { 
        laver.anchoredPosition  = Vector2.zero;
        inputDirection          = Vector2.zero;
        isInput                 = false;
    }
}
