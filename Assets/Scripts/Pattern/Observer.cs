using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observable : MonoBehaviour
{
    private Dictionary<string, ObserverContainer> observerContainers
        = new Dictionary<string, ObserverContainer>();

    public void Subscribe(string _EventTag, IObserver _NewObserver)
    {
        if (!observerContainers.ContainsKey(_EventTag))
            observerContainers.Add(_EventTag, new ObserverContainer());

        observerContainers[_EventTag].Subscribe(_NewObserver);
        _NewObserver.Container = observerContainers[_EventTag];
    }

    public void Notify(string _EventTag)
    {
        if (!observerContainers.ContainsKey(_EventTag))
            return;

        List<IObserver> Observers = new List<IObserver>(observerContainers[_EventTag].Observers);
        foreach (IObserver elem_Obs in Observers)
            elem_Obs.FieldChange();
    }



    protected void Notify(string _EventTag, ref float _TotalDamage)
    {
        if (!observerContainers.ContainsKey(_EventTag))
            return;

        List<IObserver> Observers = new List<IObserver>(observerContainers[_EventTag].Observers);
        foreach (IObserver elem_Obs in Observers)
            elem_Obs.FieldChange(ref _TotalDamage);
    }

    protected void Unsubscribe()
    {
        foreach (ObserverContainer elem in observerContainers.Values)
            elem.Unsubscribe();
    }

    public void Unsubscribe(string _EventTag, int _ID)
    {
        if (!observerContainers.ContainsKey(_EventTag))
            return;

        observerContainers[_EventTag].Unsubscribe(_ID);
    }
}

// ==================================

public class ObserverContainer
{
    protected List<IObserver> observers = new List<IObserver>();

    public List<IObserver> Observers { get { return observers; } }

    public void Subscribe(IObserver _NewObserver)
    {
        observers.Add(_NewObserver);
    }

    public void Unsubscribe()
    {
        List<IObserver> obsCopy = new List<IObserver>(observers);

        foreach (IObserver elem in obsCopy)
        {
            if (elem.IsClear)
                observers.Remove(elem);          
        }
    }

    public void Unsubscribe(int _ID)
    {
        IObserver iter_find = observers.Find((IObserver _Observer) =>
        { 
            return (_Observer.ObserverID == _ID); 
        });

        if (null == iter_find)
            return;

        observers.Remove(iter_find);
    }
}

// ==================================

public class IObserver
{
    public IObserver()
    {
        observerId = ++id;
    }

    private static int id = 0;

    protected ObserverContainer container;
    protected int               observerId;
    protected bool              isClear;

    public bool              IsClear       { get { return isClear; } }
    public int               ObserverID    { get { return observerId; } }
    public ObserverContainer Container     { set { container = value; } }

    public virtual void FieldChange() { }
    public virtual void FieldChange(ref float _TotalDamage) { }
    public virtual void FieldChange(Collider _Other) { }
}

public class OnCrashObserver : IObserver
{
    public delegate void OnCrashEvent(ref float _TotalDamage);
    private OnCrashEvent  onEvent;

    public OnCrashObserver(OnCrashEvent _Event, bool _IsClear)
    {
        onEvent = _Event;
        isClear = _IsClear;
    }

    public override void FieldChange(ref float _TotalDamage)
    {
        onEvent(ref _TotalDamage);
    }
}

public class OnTrrigerObserver : IObserver
{
    public delegate void OnTriggerEvent();
    private OnTriggerEvent onEvent;

    public OnTrrigerObserver(OnTriggerEvent _Event, bool _IsClear)
    {
        onEvent = _Event;
        isClear = _IsClear;
    }

    public override void FieldChange()
    {
        onEvent();
    }

    public override void FieldChange(ref float _TotalDamage)
    {
        onEvent();
    }
}

public class OnTrrigerDeleteObserver : IObserver
{
    public delegate bool OnTriggerDeleteEvent();
    private OnTriggerDeleteEvent onEvent;

    public OnTrrigerDeleteObserver(OnTriggerDeleteEvent _Event, bool _IsClear)
    {
        onEvent = _Event;
        isClear = _IsClear;
    }

    public override void FieldChange()
    {
        if (onEvent())
            container.Unsubscribe(observerId);
    }

    public override void FieldChange(ref float _TotalDamage)
    {
        if (onEvent())
            container.Unsubscribe(observerId);
    }
}

public class OnColliderObserver : IObserver
{
    public delegate void OnColliderEvent(Collider _Other);
    OnColliderEvent onEvent;

    public OnColliderObserver(OnColliderEvent _Event, bool _IsClear)
    {
        onEvent = _Event;
        isClear = _IsClear;
    }

    public override void FieldChange(Collider _Other)
    {
        onEvent(_Other);
    }
}