using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Gadget : SlotUI
{
    public delegate void OnEquip();
    
    private Text    txTitle;
    private Text    txLv;
    private Text    txContent;
    private Image   ImgItem;
    private Gadget  targetGadget;
    private int     targetLevel;
    //private OnEquip onEquipItem;

    static private Sprite  nullSprite;

    protected override void Initialize()
    {
        base.Initialize();

        txTitle     = transform.Find("Tx_Title").GetComponent<Text>();
        txLv        = transform.Find("Tx_Lv").GetComponent<Text>();
        txContent   = transform.Find("Tx_Content").GetComponent<Text>();
        ImgItem     = transform.Find("Img_Item").GetComponent<Image>();
        nullSprite  = Resources.Load<Sprite>("Texture/UI/Gadget/SelectFrame/T_JobSelect_SelectedCheck");
    }

    protected override void Notify_OnClick()
    {
        UIManager.Instance.UpgradeSlotDisable();      

        StartCoroutine(OnClickRoutine());
    }

    IEnumerator OnClickRoutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        GameManager.Instance.ChoiceGadget(slotIdx);
    }

    public override void UIEnable()
    {
        base.UIEnable();

        Gadget gadgetPrefab = GameManager.Instance.Get_GadgetInfo(slotIdx);
        targetGadget = gadgetPrefab;

        if (!gadgetPrefab)
            return;

        gameObject.SetActive(true);

        Sprite texUI = gadgetPrefab.LevelDatatSheet.UITexture;
        int curLevel = GameManager.Instance.Player.GadgetLevel(gadgetPrefab.gameObject);
        int maxLevel = (gadgetPrefab.LevelDatatSheet) ? (gadgetPrefab.LevelDatatSheet.MaxLevel() - 1) : (0);
        targetLevel = curLevel;

        string TitleTex   = gadgetPrefab.name;
        string LvTex      = "[ Lv. " + curLevel + " / " + maxLevel + " ]";
        string ContentTex = gadgetPrefab.NextLevelInfo((0 == curLevel) ? (0) : (curLevel));

        txTitle.text   = TitleTex;
        txLv.text      = LvTex;
        txContent.text = ContentTex;
        ImgItem.sprite = (texUI) ? (texUI) : (nullSprite);
    }

}
