using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGroup : Observable
{
    protected virtual void Initialize()
    {

    }

    protected virtual void Initialized()
    {

    }

    public virtual void UIGroupEnable()  
    {
        gameObject.SetActive(true);
    }

    public virtual void UIGroupDisable() 
    {
        gameObject.SetActive(false);
    }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        Initialized();
    }
}
