using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBuilder
{
    GameObject  prefab;
    GameObject  subject;

    public DefaultBuilder(GameObject _Prefab)
    {
        prefab       = _Prefab;
        subject      = ObjectPool.Instance.GetObject(prefab, false);
        subject.name = _Prefab.name;
    }

    public DefaultBuilder(string _PrefabTag, string _Path)
    {
        prefab       = Resources.Load<GameObject>(_Path + "/" + _PrefabTag);
        subject      = ObjectPool.Instance.GetObject(prefab, false); ;
        subject.name = prefab.name;
    }

    public DefaultBuilder Set_Parent(Transform _Parent)
    {
        subject.transform.parent = _Parent;

        return this;
    }

    public DefaultBuilder Set_Position(Vector3 _Pos)
    {
        subject.transform.position = _Pos;

        return this;
    }

    public DefaultBuilder Set_Scale(Vector3 _Siz)
    {
        subject.transform.localScale = _Siz;

        return this;
    }

    public DefaultBuilder Set_Rotation( Quaternion _Rot)
    {
        subject.transform.rotation = _Rot;

        return this;
    }

    public DefaultBuilder Set_DeleteTime(float _DeleteTime)
    {
        ObjectPool.Instance.ReturnObject(prefab, subject, _DeleteTime);

        return this;
    }

    public GameObject Build()
    {
        subject.SetActive(true);

        if (null == subject.transform.parent)
        {
            Transform tagTransform = ObjectPool.Instance.ObjectGroup.transform.Find(subject.name);

            if (tagTransform)
            {
                subject.transform.parent = tagTransform;
            }
            else 
            {
                GameObject newGroup       = new GameObject();
                newGroup.transform.parent = ObjectPool.Instance.ObjectGroup.transform;
                newGroup.name             = subject.name;

                subject.transform.parent = newGroup.transform;

                newGroup.AddComponent<ChildCount>().UpdateCount();
            }
        }

        return subject;
    }
}

public class Builder<T>
    where T : ObjectBase
{
    public Builder(GameObject _Prefab)
    {
        prefab       = _Prefab;
        subject      = ObjectPool.Instance.GetObject(prefab, false);
        subject.name = prefab.name;
        mainScript   = subject.GetComponent<T>();

        mainScript.PrefabName = prefab.name;
        mainScript.Clear();
    }

    public Builder(string _PrefabTag, string _Path)
    {
        prefab       = Resources.Load<GameObject>(_Path + "/" + _PrefabTag);
        subject      = ObjectPool.Instance.GetObject(prefab, false);
        subject.name = prefab.name;
        mainScript   = subject.GetComponent<T>();

        mainScript.PrefabName = prefab.name;
    }

    public Builder<T> Set_DeleteTime(float _DeleteTime)
    {
        if (0f < _DeleteTime)
        { 
            ObjectPool.Instance.ReturnObject
            (
                prefab,
                subject,
                _DeleteTime
            );     
        }

        return this;
    }

    public Builder<T> Set_Layer(int _Layer)
    {
        subject.layer = _Layer;

        return this;
    }

    public Builder<T> Set_Tag(string _Tag)
    {
        subject.tag = _Tag;

        return this;
    }

    public Builder<T> Set_Parent(Transform _Parent)
    {
        subject.transform.parent = _Parent;

        return this;
    }

    public Builder<T> Set_Position(Vector3 _Pos)
    {
        subject.transform.position = _Pos;

        return this;
    }

    public Builder<T> Set_PositionBaseOffset(Vector3 _Pos)
    {
        subject.transform.position = subject.transform.position + _Pos;

        return this;
    }

    public Builder<T> Set_Scale(Vector3 _Siz)
    {
        subject.transform.localScale = _Siz;

        return this;
    }

    public Builder<T> Set_Rotation(Quaternion _Rot)
    {
        subject.transform.rotation = _Rot;

        return this;
    }

    public Builder<T> Set_Transform(Vector3 _Pos, Quaternion _Rot)
    {
        subject.transform.position = _Pos;
        subject.transform.rotation = _Rot;

        return this;
    }

    public Builder<T> Set_Transform(Vector3 _Pos, Vector3 _Siz)
    {
        subject.transform.position   = _Pos;
        subject.transform.localScale = _Siz;

        return this;
    }

    public Builder<T> Set_Transform(Vector3 _Pos, Quaternion _Rot, Vector3 _Siz)
    {
        subject.transform.position   = _Pos;
        subject.transform.rotation   = _Rot;
        subject.transform.localScale = _Siz;

        return this;
    }

    public Builder<T> Set_Direction(Vector3 _Dir)
    {
        Vector3 Dir = _Dir;
        Dir.y = 0f;
        Dir   = Dir.normalized;

        mainScript.MoveDir = (Vector3.zero == Dir)
            ? (subject.transform.forward)
            : (Dir);

        return this;
    }

    public Builder<T> Set_Strategy(IStrategy[] _Strategys)
    {
        foreach (IStrategy elem in _Strategys)
        {
            IStrategy com = subject.AddComponent(elem.GetType())
                 as IStrategy;
            com.Clone(elem);
        }

        return this;
    }

    public Builder<T> Set_Strategy(IStrategy[] _Strategys, bool _Enable)
    {
        foreach (IStrategy elem in _Strategys)
        {
            if (_Enable != elem.IsActivate())
                continue;

            IStrategy com = subject.AddComponent(elem.GetType())
                as IStrategy;
            com.Clone(elem);
        }

        return this;
    }

    public Builder<T> Set_Strategy(IStrategy _Strategy)
    {
        if (null == _Strategy)
            return this;

        IStrategy com = subject.AddComponent(_Strategy.GetType()) as IStrategy;
        com.Clone(_Strategy);

        return this;
    }

    public Builder<T> Set_Strategy(IStrategy _Strategy, bool _Enable)
    {
        if (null == _Strategy)
            return this;

        if (_Enable != _Strategy.IsActivate())
            return this;

        IStrategy com = subject.AddComponent(_Strategy.GetType()) as IStrategy;
        com.Clone(_Strategy);

        return this;
    }

    public GameObject Build()
    {
        subject.SetActive(true);
        mainScript.BeginPlay();

        if (null == subject.transform.parent)
        {
            Transform tagTransform = ObjectPool.Instance.ObjectGroup.transform.Find(subject.name);

            if (tagTransform)
            {
                subject.transform.parent = tagTransform;

                tagTransform.GetComponent<ChildCount>().UpdateCount();
            }
            else
            {
                GameObject newGroup       = new GameObject();
                newGroup.transform.parent = ObjectPool.Instance.ObjectGroup.transform;
                newGroup.name             = subject.name;

                subject.transform.parent = newGroup.transform;

                newGroup.AddComponent<ChildCount>().UpdateCount();
            }
        }

        return subject;
    }

    T               mainScript;
    GameObject      subject;
    GameObject      prefab;
}


