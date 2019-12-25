using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UISkillItem : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI txtSkillName, txtMana, txtCoolDown, txtDes1, txtLevel;
    public GameObject Unlock;
    public TextMeshProUGUI txtUnlockTier;
    public TextMeshProUGUI[] txtDes2;
    public UIButton BtnUpgrade;
    public UIButton BtnEvolve;
    public UIButton Buy;
    public GameObject Active, Pasive;
    public TextMeshProUGUI txtDesPasive;
    public SkillData skillData;
    [SerializeField]
    SaveGameTierLevel sgtl;
    public bool isHero
    {
        get
        {
            return MenuController.Instance.UIPanelHeroAlliance.isHero;
        }
    }
    public Elemental SellectedElemental
    {
        get
        {
            return MenuController.Instance.UIPanelHeroAlliance.SelectedElemental;
        }
    }
    private void Start()
    {
        BtnUpgrade.SetUpEvent(OnBtnUpgradeClick);
        Buy.SetUpEvent(OnBtnBuyClick);
        BtnEvolve.SetUpEvent(OnBtnEvolveClick);
    }

    void OnBtnEvolveClick()
    {
        MenuController.Instance.UIPanelHeroAlliance.OnEvolveSkill(skillData, sgtl);
    }

    private void OnBtnBuyClick()
    {
        throw new NotImplementedException();
    }

    private void OnBtnUpgradeClick()
    {
        MenuController.Instance.UIPanelHeroAlliance.OnUpgradeSkill(skillData, sgtl);
    }

    // Start is called before the first frame update
    public void SetUpdata(string SkillID)
    {
        Unlock.gameObject.SetActive(false);
        Buy.gameObject.SetActive(false);
        BtnEvolve.gameObject.SetActive(false);
        BtnUpgrade.gameObject.SetActive(false);
        Active.gameObject.SetActive(false);
        Pasive.gameObject.SetActive(false);
        //WEAPON_WIND_SKILL_1
        skillData = JsonUtility.FromJson<SkillData>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        sgtl = DataController.Instance.GetGameSkillData(SkillID);
        if (sgtl.Level == 0)
        {
            txtLevel.gameObject.SetActive(false);
            int skill4 = int.Parse(SkillID.Split('_')[3]);
            if (skill4 < 4)
            {
                txtUnlockTier.text = string.Format("Tier - {0} \n Unlock", skill4);
                Unlock.gameObject.SetActive(true);
            }
            else
            {
                Buy.gameObject.SetActive(true);
            }
        }
        else
        {
            /// check Max
            if (skillData.baseSkills.Count == sgtl.Tier && skillData.baseSkills[sgtl.Tier - 1].MaxLevel == sgtl.Level)
            {
                ///max mie no roi
            }
            else
            {
                /// Check Can Upgrade
                if (skillData.baseSkills[sgtl.Tier - 1].MaxLevel != sgtl.Level)
                {
                    BtnUpgrade.gameObject.SetActive(true);
                }
                else
                {
                    BtnEvolve.gameObject.SetActive(true);
                }
            }
            txtLevel.text = string.Format("Lv.{0}/{1}", sgtl.Level, skillData.baseSkills[sgtl.Tier - 1].MaxLevel);
        }
        //////////////////////////////////
        //Display Info
        txtSkillName.text = Language.GetKey("Name_" + skillData.SkillID) + "." + ToolHelper.ToRoman(sgtl.Tier);
        if (skillData.SkillType == 0)
        {
            Active.gameObject.SetActive(true);
            txtMana.text = "Mana: " + skillData.GetManaCost(sgtl.Tier, sgtl.Level);
            txtCoolDown.text = "Cool Down: " + skillData.GetCoolDown(sgtl.Tier, sgtl.Level) + "s";
            txtDes1.text = Language.GetKey("Des1_" + skillData.SkillID + "_" + sgtl.Tier);
            txtDes2[0].gameObject.SetActive(true);
            txtDes2[0].text = "Damage: " + skillData.GetDamage(sgtl.Tier, sgtl.Level);
            if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes != null)
            {
                if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count >= 1)
                {
                    txtDes2[1].gameObject.SetActive(true);
                    txtDes2[1].text = Language.GetKey(skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Attribute) + ": " + skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Value[sgtl.Level - 1];
                }
                if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count >= 2)
                {
                    txtDes2[2].gameObject.SetActive(true);
                    txtDes2[2].text = Language.GetKey(skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Attribute) + ": " + skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Value[sgtl.Level - 1];
                }
            }
        }
        else
        {
            Pasive.SetActive(true);
            int Level = sgtl.Level;
            if (sgtl.Level == 0)
                Level = 1;
            if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes != null)
            {
                if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count == 1)
                {
                    txtDesPasive.text = string.Format(Language.GetKey("Des1_" + skillData.SkillID), skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Value[Level - 1]);
                }
                if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count == 2)
                {
                    txtDesPasive.text = string.Format(Language.GetKey("Des1_" + skillData.SkillID), skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Value[Level - 1], skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Value[Level - 1]);
                }
                if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count == 3)
                {
                    txtDesPasive.text = string.Format(Language.GetKey("Des1_" + skillData.SkillID), skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Value[Level - 1], skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Value[Level - 1], skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[2].Value[Level - 1]);
                }
            }
        }
    }
}
