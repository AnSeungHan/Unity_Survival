using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool 
    : Singleton<ObjectPool>
{
    private class PrefabPool
    {
        private ObjectPool          Manager;
        private GameObject          WaitGroup;
        private GameObject          Prefab;
        private Queue<GameObject>   Pool = new Queue<GameObject>();

        public PrefabPool(GameObject _Prefab, ObjectPool _Manager)
        {
            Prefab  = _Prefab;
            Manager = _Manager;

            Transform tagTransform = Manager.ObjectGroup.transform.Find(_Prefab.name);

            if (tagTransform)
            {
                WaitGroup = tagTransform.gameObject;
            }
            else
            {
                GameObject newGroup       = new GameObject();
                newGroup.transform.parent = Manager.WaitGroup.transform;
                newGroup.name             = Prefab.name;

                WaitGroup = newGroup;
            }
        }

        public GameObject GetObject()
        {
            GameObject obj = (0 < Pool.Count)
                ? (Pool.Dequeue())
                : (CreatNewObject());

            obj.transform.SetParent(null);
            obj.SetActive(false);

            return obj;
        }

        public void ReturnObject(GameObject _Obj)
        {
            if (!_Obj)
                return;

            _Obj.SetActive(false);
            _Obj.transform.SetParent(WaitGroup.transform);
            Pool.Enqueue(_Obj);
        }

        public void ReturnObject(GameObject _Obj, float _DelayTime)
        {
            if (!_Obj)
                return;

            Manager.StartCoroutine(DelayedAction(_Obj, _DelayTime));
        }

        IEnumerator DelayedAction(GameObject _Obj, float _DelayTime)
        {
            yield return new WaitForSeconds(_DelayTime);

            ReturnObject(_Obj);
        }

        private GameObject CreatNewObject()
        {
            GameObject obj = Instantiate(Prefab, Manager.transform);
            obj.SetActive(false);

            return obj;
       }
    }

    private Dictionary<string, PrefabPool> PoolList = new Dictionary<string, PrefabPool>();


    public  GameObject                     ObjectGroup;
    public  GameObject                     WaitGroup;

    protected override void Initialize()
    {
        base.Initialize();

        if (!ObjectGroup)
        {
            ObjectGroup                  = new GameObject();
            ObjectGroup.name             = "ObjectGroup";
            ObjectGroup.transform.parent = transform;
        }

        if (!WaitGroup)
        {
            WaitGroup                  = new GameObject();
            WaitGroup.name             = "WaitGroup";
            WaitGroup.transform.parent = transform;
        }      
    }

    public GameObject GetObject(GameObject _Prefab, bool _Active)
    {
        if (!_Prefab)
            return null;

        GameObject obj = (Instance.PoolList.ContainsKey(_Prefab.name))
            ? (Instance.PoolList[_Prefab.name].GetObject())
            : (Instance.CreatNewPool(_Prefab));

        if (!obj)
            return null;

        obj.transform.SetParent(null);
        obj.SetActive(_Active);

        return obj;
    }

    public GameObject GetObject(string _PrefabTag, bool _Active)
    {
        if (!Instance.PoolList.ContainsKey(_PrefabTag))
            return null;

        GameObject obj = Instance.PoolList[_PrefabTag].GetObject();

        if (!obj)
            return null;

        obj.transform.SetParent(null);
        obj.SetActive(_Active);

        return obj;
    }

    public GameObject GetObject(string _PrefabTag, string _Path, bool _Active)
    {
        if (Instance.PoolList.ContainsKey(_PrefabTag))
        {
            return GetObject(_PrefabTag, _Active);
        }
        else 
        {
            GameObject obj = CreatNewPool(_PrefabTag, _Path);

            if (!obj)
                return null;

            obj.SetActive(_Active);

            return obj;
        }

    }

    public void ReturnObject(GameObject _Prefab, GameObject _Obj)
    {
        if (!Instance.PoolList.ContainsKey(_Prefab.name))
            CreatNewPool(_Prefab);

        Instance.PoolList[_Prefab.name].ReturnObject(_Obj);
    }

    public void ReturnObject(GameObject _Prefab, GameObject _Obj, float _Delay)
    {
        if (!Instance.PoolList.ContainsKey(_Prefab.name))
            CreatNewPool(_Prefab);

        Instance.PoolList[_Prefab.name].ReturnObject(_Obj, _Delay);
    }

    public void ReturnObject(string _PrefanTag, GameObject _Obj)
    {
        if (!Instance.PoolList.ContainsKey(_PrefanTag))
            CreatNewPool(_Obj);

        Instance.PoolList[_PrefanTag].ReturnObject(_Obj);
    }

    public void ReturnObject(string _PrefanTag, GameObject _Obj, float _Delay)
    {
        if (!Instance.PoolList.ContainsKey(_PrefanTag))
            CreatNewPool(_Obj);

        Instance.PoolList[_PrefanTag].ReturnObject(_Obj, _Delay);
    }

    private GameObject CreatNewPool(GameObject _Prefab)
    {
        if (!_Prefab)
            return null;

        PrefabPool NewPool = new PrefabPool(_Prefab, Instance);
        PoolList.Add(_Prefab.name, NewPool);

        return NewPool.GetObject();
    }

    private GameObject CreatNewPool(string _PrefabTag, string _Path)
    {
        GameObject Prefab = Resources.Load<GameObject>(_Path + "/" + _PrefabTag);

        if (!Prefab)
            return null;

        PrefabPool NewPool = new PrefabPool(Prefab, Instance);
        PoolList.Add(_PrefabTag, NewPool);

        return NewPool.GetObject();
    }
}
