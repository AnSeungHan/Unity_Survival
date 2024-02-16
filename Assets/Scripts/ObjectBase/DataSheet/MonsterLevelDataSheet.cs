using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MonsterData", menuName = "DataSheet/ScriptableObject/Data")]
public class MonsterLevelDataSheet : ScriptableObject
{
    public MonsterData[]    data;

    public bool GetData(int _NextLevel, ref MonsterData _Data)
    {
        if (_NextLevel >= data.Length)
            return false;

        _Data = data[_NextLevel];

        return true;
    }

    public virtual int MaxLevel()
    {
        return data.Length;
    }
}

[System.Serializable]
public struct MonsterData
{
    public float HP;
    public float Speed;
}