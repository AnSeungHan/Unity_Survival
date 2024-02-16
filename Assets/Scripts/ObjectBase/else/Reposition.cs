using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider      reposCollider;

    public float  moveDistance = 30f;

    private void Awake()
    {
        reposCollider = GetComponent<Collider>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Area"))
            return;

        Vector3 PlayerPos = GameManager.Instance.Player.transform.position;
        Vector3 MyPos     = transform.position;
        Vector3 Diff      = new Vector3(Mathf.Abs(PlayerPos.x - MyPos.x), 0f, Mathf.Abs(PlayerPos.z - MyPos.z));

        Vector3 PlayerDir = GameManager.Instance.Player.GetComponent<Unit>().MoveDir;
        Vector3 Dir       = new Vector3((0 > PlayerDir.x) ? (-1f) : (1f), 0f, (0 > PlayerDir.z) ? (-1f) : (1f));

        switch (transform.tag)
        {
            case "Ground" :
            {
                if (Diff.x > Diff.z)
                {
                    transform.Translate(Vector3.right * Dir.x * moveDistance);

                    //Debug.Log(this.gameObject.name + " (R) : " + Vector3.right * Dir.x * moveDistance);
                }
                else if (Diff.x < Diff.z)
                {
                    transform.Translate(Vector3.forward * Dir.z * moveDistance);

                    //Debug.Log(this.gameObject.name + " (F) : " + Vector3.forward * Dir.z * moveDistance);
                }
            }
            break;

            case "Monster":
            {
                if (!reposCollider.enabled)
                    return;

                Vector3 RemovePos = PlayerPos + (Dir * moveDistance + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));
                transform.position = RemovePos;

                //Debug.Log("Remove Monster : [ " + RemovePos + " ]");
            }
            break;

            case "Projectile":
            {
                ObjectBase objBase = gameObject.GetComponent<ObjectBase>();

                if (null == objBase)
                    return;

                objBase.Dead();
            }
            break;
        }
    }
}
