using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BounceMode { Random, Near, Reflex };

public class SAtk_Bounce : AttackStrategy
{   
    public delegate void OnCrashEvent();

    [SerializeField]
    private float           bounceRange;
    [SerializeField]
    private BounceMode      mode = BounceMode.Random;

    private OnCrashEvent    onCrash;

    public BounceMode Mode { get { return mode; } set { mode = value; } }

    public override void BeginPlay()
    {
        base.BeginPlay();

        switch (mode)
        {
            case BounceMode.Random : onCrash = this.RandomDir; break;
            case BounceMode.Near   : onCrash = this.NearDir;   break;
            case BounceMode.Reflex : onCrash = this.ReflexDir; break;
        }
    }

    public override void Clone(IStrategy _Other)
    {
        base.Clone(_Other);

        SAtk_Bounce other = _Other as SAtk_Bounce;

        if (null == other)
            return;

        bounceRange = other.bounceRange;
        mode        = other.mode;
    }

    public override void LevelUp(string _ValueName, string _Value)
    {
        base.LevelUp(_ValueName, _Value);

        switch (_ValueName)
        {
            case "bounceRange": bounceRange = float.Parse(_Value); break;
        }
    }

    protected override void OnCrash(ref float _TotalDamage)
    {
        onCrash();

        Active_AttackEffect("OnCrash");
    }

    private void RandomDir()
    {
        RaycastHit[] ray = MakeRaycastHit(bounceRange, targetLayer);

        if (0 == ray.Length)
            return;

        Transform nearTarget = ray[Random.Range(0, ray.Length - 1)].transform;

        if (!nearTarget)
            return;

        Vector3 pos = nearTarget.position - transform.position;
        pos.y = 0f;
        Vector3 dir = pos.normalized;
        ownerBase.MoveDir = dir;
    }

    private void NearDir()
    {
        RaycastHit[] ray = MakeRaycastHit(bounceRange, targetLayer);

        if (0 == ray.Length)
            return;

        Transform nearTarget = Find_Nearest(MakeRaycastHit(bounceRange, targetLayer), 1);

        if (!nearTarget)
            return;

        Vector3 pos = nearTarget.position - transform.position;
        pos.y = 0f;
        Vector3 dir = pos.normalized;
        ownerBase.MoveDir = dir;
    }

    private void ReflexDir()
    {
        Vector3 reflected = Vector3.Reflect(ownerBase.MoveDir, ownerBase.MoveDir.normalized);

        if (reflected == Vector3.zero)
            return;

        Quaternion rotation = Quaternion.Euler(0, Random.Range(-30f, 30f), 0);

        ownerBase.MoveDir = (rotation * reflected);
    }
}
