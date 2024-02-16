using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : ObjectBase
{
    delegate bool OnCollitionFunction(Collider other);

    private OnCollitionFunction     onCollision;
    private static Material[]       materialSample;

    protected override void Initialize()
    {
        base.Initialize();

        gameObject.tag  = "Projectile";
        offsetTransform = transform;

        materialSample = new Material[]
        {
            Resources.Load("Material/Projetile/Test1", typeof(Material)) as Material,
            Resources.Load("Material/Projetile/Test2", typeof(Material)) as Material
        };

        Subscribe("OnDead", new OnTrrigerObserver(this.DeleteObsject, false));
    }

    protected override void Initialized()
    {
        base.Initialized();

        if (LayerMask.NameToLayer("EnumyAttack") == gameObject.layer)
        {
            onCollision = (Collider other) =>
            {
                if ("User" != other.gameObject.tag)
                    return false;

                return true;
            };       
        }
        else if (LayerMask.NameToLayer("AllyAttack") == gameObject.layer)
        {
            onCollision = (Collider other) =>
            {
                if ("Monster" != other.gameObject.tag)
                    return false;

                /*if (other.name != "Monster(Clone)")
                    Debug.Log("User Tag OnTriggerEnter() Target : " + other.tag + " / " + other.name);*/

                return true;
            };
        }
        else
        {
            Debug.Log("Prohectile::Initialized [ Layer Err ] : " + gameObject.layer);
        }

        //TestChangeMesh();
    }

    public override void Clear()
    {
        base.Clear();

        IStrategy[] strategys = GetComponents<IStrategy>();

        foreach (IStrategy elem in strategys)
        {
            if (!elem.IsActivate())
                continue;

            DestroyImmediate(elem);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (null == onCollision || !onCollision(other))
            return;

        Unit otherUnit = other.GetComponent<Unit>();

        if (null == otherUnit)
            return;

        float totalDamage = 0f;
        Notify("OnCrash", ref totalDamage);

        if (0f == totalDamage)
            return;

        Notify("OnHitEffect");
        otherUnit.TakeDamage(totalDamage);
    }

    void OnTriggerStay(Collider other)
    {
        if (null == onCollision || !onCollision(other))
            return;

        Unit otherUnit = other.GetComponent<Unit>();

        if (null == otherUnit)
            return;

        float totalDamage = 0f;
        Notify("OnCrashStay", ref totalDamage);

        if (0f == totalDamage)
            return;

        Notify("OnHitEffect");
        otherUnit.TakeDamage(totalDamage);
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    private void TestChangeMesh()
    {
        Renderer render = GetComponent<Renderer>();

        if (!render)
            return;

        /*Material[] mat =
        {
            new Material(Resources.Load("Material/Projetile/Test1", typeof(Material)) as Material),
            new Material(Resources.Load("Material/Projetile/Test2", typeof(Material)) as Material)
        };*/

        if (LayerMask.NameToLayer("EnumyAttack") == gameObject.layer)
             render.material = materialSample[1];
         else if (LayerMask.NameToLayer("AllyAttack") == gameObject.layer)
             render.material = materialSample[0];
         else
             Debug.Log("Prohectile::OnEnable [ Layer Err ] : " + gameObject.layer);
    }
}