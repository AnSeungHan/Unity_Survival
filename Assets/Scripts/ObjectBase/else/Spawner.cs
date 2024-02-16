using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[]  SpawnData;
    public Transform[]  SpawnTransform;

    private int         Level = 0;
    private float       Timer;

    [SerializeField]
    private bool        DoNotCreateMode;

    void Awake()
    {
        SpawnTransform = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; ++i)
            SpawnTransform[i] = transform.GetChild(i).transform;
    }

    void Update()
    {
        if (!GameManager.Instance.IsLive)
            return;

        Timer += Time.deltaTime;
        Level  = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.CurGameTime / 10f), SpawnData.Length);

        if (DoNotCreateMode)
            return;

        if (Timer >= SpawnData[0].SpawnTime)
        {
            List<Vector3> randomList = new List<Vector3>();
            for (int i = 0; i < SpawnTransform.Length; ++i)
            {
                RaycastHit[] rayhits = Physics.SphereCastAll(SpawnTransform[i].position, 1f, Vector3.forward, 0f, LayerMask.GetMask("Enumy"));

                if (0 == rayhits.Length)
                    randomList.Add(SpawnTransform[i].position);
            }

            if (0 == randomList.Count)
                return;

            GameMath.RandomMix<Vector3>(ref randomList, randomList.Count);    
            SpawnData data = SpawnData[0];

            GameObject monsterObj = new Builder<Unit>(data.Prefab)
                .Set_Tag("Monster")
                .Set_Layer(7)
                .Set_Position(randomList[Random.Range(0, randomList.Count - 1)])
                .Build();

            if (!monsterObj)
                return;

            Unit monsterUnit = monsterObj.GetComponent<Unit>();
            MonsterData Data = new MonsterData();
            monsterUnit.LevelData.GetData(SpawnData[0].Lv, ref Data);

            monsterUnit.MaxHealth = Data.HP;
            monsterUnit.GetComponent<MovementStrategy>().MoveSpeed = Data.Speed;

            Timer = 0f;
        }
    }
}

[System.Serializable]
public struct SpawnData
{
    public GameObject   Prefab;
    public float        SpawnTime;
    public int          Lv;
}