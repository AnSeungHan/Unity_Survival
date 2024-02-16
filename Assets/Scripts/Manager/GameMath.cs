using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMath
{
    public static void RandomMix<T>(ref T[] _List, int _Length)
    {
        for (int i = 0; i < _Length - 1; i++)
        {
            Swap<T>(ref _List[i], ref _List[Random.Range(i, _Length)]);
        }
    }

    public static void RandomMix<T>(ref List<T> _List, int _Length)
    {
        for (int i = 0; i < _Length - 1; i++)
        {
            int randIdx = Random.Range(i, _Length);

            T val1 = _List[i];
            T val2 = _List[randIdx];

            Swap<T>(ref val1, ref val2);
            _List[i]       = val2;
            _List[randIdx] = val1;
        }
    }

    public static void Swap<T>(ref T _Value1, ref T _Value2)
    {
        T temp  = _Value1;
        _Value1 = _Value2;
        _Value2 = temp;
    }
}
