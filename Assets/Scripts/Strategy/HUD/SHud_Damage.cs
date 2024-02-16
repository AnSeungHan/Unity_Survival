using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SHud_Damage : MonoBehaviour
{
    private TextMeshPro     damageText;

    [SerializeField]
    private float       startFontHight = 2f;
    private float       startFontSize;
    private float       addFontHight;
    private float       addFontSize;

    private static GameObject  prefab;

    void Awake()
    {
        damageText    = GetComponent<TextMeshPro>();
        prefab        = Resources.Load<GameObject>("UIPrefab/Unit/DamageText (TMP)");
        startFontSize = damageText.fontSize;
    }

    void Update()
    {
        transform.Translate(0f, addFontHight, 0f);
        damageText.fontSize += addFontSize;

        addFontHight += 0.01f * Time.deltaTime;
        addFontSize  += 0.05f * Time.deltaTime;

        if (10f <= damageText.fontSize)
        {
            ObjectPool.Instance.ReturnObject(prefab, this.gameObject);
        }
    }

    private void Clear()
    {
        transform.position      = transform.position + new Vector3(0, startFontHight, 0);
        addFontHight            = addFontSize = 0;
        damageText.fontSize     = startFontSize;
    }

    public void BindUI(float _Damage, Color _Color)
    {
        damageText.text  = _Damage.ToString();
        damageText.color = _Color;

        Clear();
    }
}
