using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "DataSheet/ScriptableObject/Data")]
public class LevelDataSheet : ScriptableObject
{
    public Sprite           UITexture;
    public LevelUpData[]    data;

    public bool GetData(int _NextLevel, ref LevelUpData _Data)
    {
        if (_NextLevel >= data.Length)
            return false;

        _Data = data[_NextLevel];

        return true;
    }

    public string NextLevelInfo(int _NextLevel)
    {
        if (_NextLevel >= data.Length)
            return "[ out of range ]";

        LevelUpData nextData = data[_NextLevel];

        string Result = "";
        foreach (LevelDataDB db in nextData.DataInfo)
            Result += db.valueName + " : " + db.value + "\n";

        return Result;
    }

    public virtual int MaxLevel()
    {
        return (null != data)
            ? (data.Length)
            : (0);
    }
}

[System.Serializable]
public struct LevelDataDB
{
    public string strategyName;
    public string valueName;
    public string value;
}

[System.Serializable]
public struct LevelUpData
{
    public LevelDataDB[] DataInfo;
}
