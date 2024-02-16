using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EquipItem : MonoBehaviour
{
    private Dictionary<string, GameObject> slotList = new Dictionary<string, GameObject>();

    [SerializeField]
    GameObject slotPrefab;

    void Awake()
    {
        UIManager.Instance.EquipItemSlot = this;
    }

    public void AddItems(Gadget _AddItemPrefab, Sprite _UITex)
    {
        if (slotList.ContainsKey(_AddItemPrefab.name))
        {
            Text txt = slotList[_AddItemPrefab.name].transform.Find("Text").GetComponent<Text>();
            txt.text = (int.Parse(txt.text) + 1).ToString();
        }
        else 
        {
            GameObject newSlot = new DefaultBuilder(slotPrefab)
                                    .Set_Parent(transform)
                                    .Build();

            newSlot.GetComponent<RectTransform>()
                .localScale = Vector3.one;

            newSlot.GetComponent<Image>()
                .sprite = _UITex;

            newSlot.transform.Find("Text").GetComponent<Text>()
                .text = "1";

            slotList.Add(_AddItemPrefab.name, newSlot);
        }
    }

    public void AddItems(Gadget _AddItemPrefab)
    {
        if (slotList.ContainsKey(_AddItemPrefab.name))
        {
            Text txt = slotList[_AddItemPrefab.name].transform.Find("Text").GetComponent<Text>();
            txt.text = (int.Parse(txt.text) + 1).ToString();
        }
        else
        {
            GameObject newSlot = new DefaultBuilder(slotPrefab)
                                    .Set_Parent(transform)
                                    .Build();

            newSlot.GetComponent<RectTransform>()
                .localScale = Vector3.one;

            newSlot.GetComponent<Image>()
                .sprite = _AddItemPrefab.LevelDatatSheet.UITexture;

            newSlot.transform.Find("Text").GetComponent<Text>()
                .text = "1";

            slotList.Add(_AddItemPrefab.name, newSlot);
        }
    }
}
