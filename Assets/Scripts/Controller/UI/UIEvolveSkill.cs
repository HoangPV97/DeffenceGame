using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIEvolveSkill : BaseUIView
{
    public Animator Anim;
    public TextMeshProUGUI txtSkillName1, txtSkillName2;
    public TextMeshProUGUI txtDes1, txtDes2;
    public TextMeshProUGUI txtLevel1, txtLevel2;
    public TextMeshProUGUI GemCost;
    public Transform ItemContain;
    public UIButton BtnEvolve, BtnClose;
    public Image Icon1, Icon2;
    public UiItem pfUIItem;
    int Gem = 0;
    [SerializeField]
    bool canEvolve = true;
    List<Item> listItem;
    SkillData skillData;
    [SerializeField]
    SaveGameTierLevel saveGameTierLevel;
    // Start is called before the first frame update
    private void Start()
    {
        BtnClose.SetUpEvent(OnBtnCloseClick);
        BtnEvolve.SetUpEvent(OnBtnEvolveClick);
    }

    private void OnBtnEvolveClick()
    {
        if (canEvolve)
        {
            DataController.Instance.AddSkillTier(skillData.SkillID);

            DataController.Instance.Gem -= Gem;
            for (int i = 0; i < listItem.Count; i++)
            {
                if (listItem[i].Type != ITEM_TYPE.None)
                {
                    DataController.Instance.AddItemQuality(listItem[i].Type, -listItem[i].Quality);
                }
            }
            MenuController.Instance.UIPanelHeroAlliance.SetupUIHero();
            DataController.Instance.Save();
            OnBtnCloseClick();
            // SetUpData(heroElemental);
        }
    }

    private void OnBtnCloseClick()
    {
        OnHide();
    }

    public void SetUpData(SkillData skillData, SaveGameTierLevel saveGameTierLevel)
    {
        OnShow();
        canEvolve = true;
        Gem = 0;
        this.skillData = skillData;
        this.saveGameTierLevel = saveGameTierLevel;
        txtSkillName1.text = Language.GetKey("Name_" + skillData.SkillID) + "." + ToolHelper.ToRoman(saveGameTierLevel.Tier);
        txtSkillName2.text = Language.GetKey("Name_" + skillData.SkillID) + "." + ToolHelper.ToRoman(saveGameTierLevel.Tier + 1);
        txtLevel1.text = "Lv." + saveGameTierLevel.Level + "/" + skillData.baseSkills[saveGameTierLevel.Tier - 1].MaxLevel;
        txtLevel2.text = "Lv." + "1/" + skillData.baseSkills[saveGameTierLevel.Tier].MaxLevel;

        if (skillData.SkillType == 0)
        {
            txtDes1.text = Language.GetKey("Des1_" + skillData.SkillID + "_" + saveGameTierLevel.Tier);
            txtDes2.text = Language.GetKey("Des1_" + skillData.SkillID + "_" + (saveGameTierLevel.Tier + 1));
        }

        foreach (Transform child in ItemContain)
        {
            Destroy(child.gameObject);
        }
        listItem = skillData.baseSkills[saveGameTierLevel.Tier - 1].EvolutionItems;
        for (int i = 0; i < listItem.Count; i++)
        {
            var it = Instantiate(pfUIItem.gameObject, Vector3.zero, Quaternion.identity);
            it.SetActive(true);
            it.transform.SetParent(ItemContain);
            it.transform.SetDefaultTransform();
            var itInInventory = DataController.Instance.GetGameItemData(listItem[i].Type);
            it.GetComponent<UiItem>().SetUpData(listItem[i], itInInventory.Quality, 2);
            var itDataBase = DataController.Instance.GetItemDataBase(listItem[i].Type);
            Gem += listItem[i].Quality * itDataBase.UseGemCost;
            if (listItem[i].Quality > itInInventory.Quality)
                canEvolve = false;
        }
        if (Gem > DataController.Instance.Gem)
            canEvolve = false;
        GemCost.text = Gem.ToString();
    }
    public override void OnHide()
    {
        base.OnHide();
        Anim.SetTrigger("HidePanelSetting");
        DG.Tweening.DOVirtual.DelayedCall(0.3f, () =>
        {
            gameObject.SetActive(false);
        }, false);
    }
    public override void OnShow()
    {
        gameObject.SetActive(true);
        Anim.SetTrigger("ShowPanelSetting");
    }
}
