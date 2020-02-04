using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class UiUpgradeSkill : BaseUIView
{
    public Animator Anim;
    public TextMeshProUGUI txtDes, txtLevel, txtGemCost, txtSkillName;
    public TextMeshProUGUI[] txtItemNumber, txtAttribute, txtAttributeValue;
    public Image[] ItemIcon;
    public Image SkillIcon;
    public Sprite NonItems;
    public int GemCost;
    public bool canUprage = true;
    public UIButton BtnUpgrade, BtnClose;
    [SerializeField]
    SkillData skillData;
    SaveGameTierLevel saveGameTierLevel;

    private void Start()
    {
        BtnUpgrade.SetUpEvent(OnBtnUpgradeClick);
        BtnClose.SetUpEvent(() =>
        {
            OnHide();
        });
    }

    void OnBtnUpgradeClick()
    {
        if (canUprage)
        {
            DataController.Instance.Gem -= GemCost;
            DataController.Instance.SaveLastPlayGacha();
            var ListItem = skillData.GetUpgradeItems(saveGameTierLevel.Tier, saveGameTierLevel.Level);
            for (int i = 0; i < ListItem.Count; i++)
            {
                DataController.Instance.AddItemQuality(ListItem[i].Type, -ListItem[i].Quality);
            }
            DataController.Instance.AddSkillLevel(skillData.SkillID);
            OnHide();
            if (MenuController.Instance.UIPanelHeroAlliance.gameObject.activeSelf)
                MenuController.Instance.UIPanelHeroAlliance.ReSetUISkill(skillData.SkillID);
            else if (MenuController.Instance.UIPanelForstress.gameObject.activeSelf)
                MenuController.Instance.UIPanelForstress.ReSetUISkill(skillData.SkillID);
            DataController.Instance.Save();
        }
        else
            MenuController.Instance.GachafailUpgrade = true;
    }


    public void SetUpData(SkillData skillData, SaveGameTierLevel saveGameTierLevel)
    {
        OnShow();
        this.skillData = skillData;
        this.saveGameTierLevel = saveGameTierLevel;
        canUprage = true;
        GemCost = 0;
        txtSkillName.text = Language.GetKey("Name_" + skillData.SkillID);
        txtLevel.text = "Lv." + saveGameTierLevel.Level;
        var ListItem = skillData.GetUpgradeItems(saveGameTierLevel.Tier, saveGameTierLevel.Level);
        for (int i = 0; i < ItemIcon.Length; i++)
        {
            txtItemNumber[i].gameObject.SetActive(false);
            ItemIcon[i].sprite = NonItems;
            txtAttribute[i].transform.parent.gameObject.SetActive(false);
        }
        if (skillData.SkillType == 0)
        {
            txtDes.text = Language.GetKey("Des1_" + skillData.SkillID + "_" + saveGameTierLevel.Tier);
            txtAttribute[0].transform.parent.gameObject.SetActive(true);
            txtAttribute[0].text = "Damage: ";
            txtAttributeValue[0].text = string.Format("{0}<color=#65FF00FF>({1})</color>", skillData.GetDamage(saveGameTierLevel.Tier, saveGameTierLevel.Level), skillData.GetDamage(saveGameTierLevel.Tier, saveGameTierLevel.Level + 1));
            if (skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes != null)
            {
                if (skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes.Count >= 1)
                {
                    txtAttribute[1].transform.parent.gameObject.SetActive(true);
                    txtAttribute[1].text = Language.GetKey(skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[0].Attribute);
                    txtAttributeValue[1].text = string.Format("{0}<color=#65FF00FF>({1})</color>", skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[0].Value[saveGameTierLevel.Level - 1], skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[0].Value[saveGameTierLevel.Level]);
                }
                if (skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes.Count >= 2)
                {
                    txtAttribute[2].transform.parent.gameObject.SetActive(true);
                    txtAttribute[2].text = Language.GetKey(skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[1].Attribute);
                    txtAttributeValue[2].text = string.Format("{0}<color=#65FF00FF>({1})</color>", skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[1].Value[saveGameTierLevel.Level - 1], skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[1].Value[saveGameTierLevel.Level]);
                }
            }
        }
        else
        {
            int Level = saveGameTierLevel.Level;
            if (saveGameTierLevel.Level == 0)
                Level = 1;
            if (skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes != null)
            {
                if (skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes.Count >= 1)
                {
                    if (skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes.Count == 1)
                        txtDes.text = string.Format(Language.GetKey("Des1_" + skillData.SkillID), skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[0].Value[Level - 1]);
                    txtAttribute[0].transform.parent.gameObject.SetActive(true);
                    txtAttribute[0].text = Language.GetKey(skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[0].Attribute);
                    txtAttributeValue[0].text = string.Format("{0}<color=#65FF00FF>({1})</color>", skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[0].Value[saveGameTierLevel.Level - 1], skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[0].Value[saveGameTierLevel.Level]);
                }
                if (skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes.Count >= 2)
                {
                    if (skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes.Count == 2)
                        txtDes.text = string.Format(Language.GetKey("Des1_" + skillData.SkillID), skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[1].Value[Level - 1], skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[1].Value[Level - 1]);
                    txtAttribute[1].transform.parent.gameObject.SetActive(true);
                    txtAttribute[1].text = Language.GetKey(skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[1].Attribute);
                    txtAttributeValue[1].text = string.Format("{0}<color=#65FF00FF>({1})</color>", skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[1].Value[saveGameTierLevel.Level - 1], skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[1].Value[saveGameTierLevel.Level]);
                }
                if (skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes.Count >= 3)
                {
                    if (skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes.Count == 3)
                        txtDes.text = string.Format(Language.GetKey("Des1_" + skillData.SkillID), skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[2].Value[Level - 1], skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[2].Value[Level - 1], skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[2].Value[Level - 1]);
                    txtAttribute[2].transform.parent.gameObject.SetActive(true);
                    txtAttribute[2].text = Language.GetKey(skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[2].Attribute);
                    txtAttributeValue[2].text = string.Format("{0}<color=#65FF00FF>({1})</color>", skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[2].Value[saveGameTierLevel.Level - 1], skillData.baseSkills[saveGameTierLevel.Tier - 1].SkillAttributes[2].Value[saveGameTierLevel.Level]);
                }
            }
        }

        //Setup Item;
        for (int i = 0; i < ListItem.Count; i++)
        {
            if (ListItem[i].Type != ITEM_TYPE.gem)
            {
                var Idb = DataController.Instance.GetItemDataBase(ListItem[i].Type);
                ItemIcon[i].sprite = DataController.Instance.DefaultData.GetSpriteItem(ListItem[i].Type);
                int current = DataController.Instance.GetGameItemData(ListItem[i].Type).Quality;
                txtItemNumber[i].gameObject.SetActive(true);
                if (current < ListItem[i].Quality)
                    canUprage = false;
                txtItemNumber[i].text = current >= ListItem[i].Quality ? string.Format("<color=#65FF00FF>{0}</color>/{1}", current, ListItem[i].Quality) : string.Format("<color=#FF0000FF>{0}</color>/{1}", current, ListItem[i].Quality);

                GemCost += Idb.UseGemCost;
            }
            else
                GemCost += ListItem[i].Quality;
        }
        txtGemCost.text = GemCost.ToString();
        SkillIcon.sprite = DataController.Instance.DefaultData.LoadSprite("ICON_" + skillData.SkillID + "_" + saveGameTierLevel.Tier);
    }
    public override void OnHide()
    {
        base.OnHide();
        Anim.SetTrigger("HidePanelSetting");
        DOVirtual.DelayedCall(0.3f, () =>
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
