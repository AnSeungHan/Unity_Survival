using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour 
    where T : MonoBehaviour
{
    private static T instance;
    [SerializeField]
    private bool     isDontDestroy = false;

    public static T Instance
    {
        get 
        {
            if (!instance)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (!instance)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Initialize()
    {
        if (instance)
        {
            Object[] obj = FindObjectsOfType(typeof(T));

            if (1 == obj.Length)
                return;

            DestroyImmediate(this.gameObject);
            return;
        }
        else
        {
            instance = Instance;
        }

        //instance = Instance;

        if (!isDontDestroy)
            return;

         if (transform.parent && transform.root)
             DontDestroyOnLoad(this.transform.root.gameObject);
         else
             DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void Initialized()
    { 
    
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
