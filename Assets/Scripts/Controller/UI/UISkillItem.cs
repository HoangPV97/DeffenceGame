using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UISkillItem : MonoBehaviour
{
    public Image Icon;
    public GameObject Background;
    public TextMeshProUGUI txtSkillName, txtMana, txtCoolDown, txtDes1, txtLevel;
    public GameObject[] Unlock;
    public TextMeshProUGUI[] txtUnlockTier;
    public TextMeshProUGUI[] txtDes2;
    public UIButton BtnSelect;
    public GameObject[] Selected;
    public UIButton BtnUpgrade;
    public UIButton BtnEvolve;
    public UIButton Buy;
    public GameObject Active, Pasive;
    public TextMeshProUGUI txtDesPasive;
    public SkillData skillData;
    public TextMeshProUGUI[] txtAttribute, txtAttributeValue;
    [SerializeField]
    SaveGameTierLevel sgtl;
    [SerializeField]
    bool TypeUI2 = false;
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
        BtnSelect.SetUpEvent(OnBtnSelectClick);
    }

    public void OnUnSelect()
    {
        if (!TypeUI2)
        {
            Background.SetActive(false);
            Selected[0].SetActive(false);
            Selected[1].SetActive(false);
            var rt = BtnSelect.gameObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y);
        }
    }

    public void OnBtnSelectClick()
    {
        Background.SetActive(true);
        Selected[0].SetActive(true);
        Selected[1].SetActive(true);
        var rt = BtnSelect.gameObject.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(20, rt.anchoredPosition.y);
        MenuController.Instance.UIPanelHeroAlliance.OnSelectSkill(this);

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
        if (MenuController.Instance.UIPanelHeroAlliance.gameObject.activeSelf)
            MenuController.Instance.UIPanelHeroAlliance.OnUpgradeSkill(skillData, sgtl);
        else if (MenuController.Instance.UIPanelForstress.gameObject.activeSelf)
            MenuController.Instance.UIPanelForstress.OnUpgradeSkill(skillData, sgtl);
    }

    // Start is called before the first frame update
    public void SetUpdata(string SkillID, bool typeUI2 = false)
    {
        TypeUI2 = typeUI2;
        OnUnSelect();
        Unlock[0].gameObject.SetActive(false);
        Unlock[1].gameObject.SetActive(false);
        Buy.gameObject.SetActive(false);
        BtnEvolve.gameObject.SetActive(false);
        BtnUpgrade.gameObject.SetActive(false);
        Active.gameObject.SetActive(false);
        Pasive.gameObject.SetActive(false);
        //WEAPON_WIND_SKILL_1
        skillData = JsonUtility.FromJson<SkillData>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        sgtl = DataController.Instance.GetGameSkillData(SkillID);
        for (int i = 0; i < txtAttribute.Length; i++)
        {
            txtAttribute[i].transform.parent.gameObject.SetActive(false);
        }

        if (sgtl.Level == 0)
        {
            txtLevel.gameObject.SetActive(false);
            int skill4 = int.Parse(SkillID.Split('_')[3]);
            if (skill4 < 4)
            {
                txtUnlockTier[0].text = string.Format("Tier - {0} \n Unlock", skill4);
                txtUnlockTier[1].text = string.Format("Tier - {0} \n Unlock", skill4);
                Unlock[0].gameObject.SetActive(true);
                Unlock[1].gameObject.SetActive(true);
            }
            else
            {
                txtUnlockTier[0].text = string.Format("Buy to\n Unlock", skill4);
                txtUnlockTier[1].text = string.Format("Buy to\n Unlock", skill4);
                Unlock[0].gameObject.SetActive(true);
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
            int Level = sgtl.Level;
            if (sgtl.Level == 0)
                Level = 1;
            Active.gameObject.SetActive(true);
            txtMana.text = "Mana: " + skillData.GetManaCost(sgtl.Tier, Level);
            txtCoolDown.text = "Cool Down: " + skillData.GetCoolDown(sgtl.Tier, Level) + "s";
            txtDes1.text = Language.GetKey("Des1_" + skillData.SkillID + "_" + sgtl.Tier);
            txtAttribute[0].transform.parent.gameObject.SetActive(true);
            txtAttribute[0].text = "Damage: ";
            txtAttributeValue[0].text = skillData.GetDamage(sgtl.Tier, Level).ToString();
            if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes != null)
            {
                if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count >= 1)
                {
                    txtAttribute[1].transform.parent.gameObject.SetActive(true);
                    txtAttribute[1].text = Language.GetKey(skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Attribute);
                    txtAttributeValue[1].text = skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Value[Level - 1].ToString();
                    if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Attribute.Contains("Percent") || skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Attribute.Contains("Chance"))
                    {
                        txtAttributeValue[1].text += "%";
                    }
                }
                if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count >= 2)
                {
                    txtAttribute[2].transform.parent.gameObject.SetActive(true);
                    txtAttribute[2].text = Language.GetKey(skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Attribute);
                    txtAttributeValue[2].text = skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Value[Level - 1].ToString();
                    if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Attribute.Contains("Percent") || skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Attribute.Contains("Chance"))
                    {
                        txtAttributeValue[2].text += "%";
                    }
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
                if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count >= 1)
                {
                    if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count == 1)
                        txtDesPasive.text = string.Format(Language.GetKey("Des1_" + skillData.SkillID), skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Value[Level - 1]);
                    txtAttribute[0].transform.parent.gameObject.SetActive(true);
                    txtAttribute[0].text = Language.GetKey(skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Attribute);
                    txtAttributeValue[0].text = skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Value[Level - 1].ToString();
                    if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Attribute.Contains("Percent") || skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Attribute.Contains("Chance"))
                    {
                        txtAttributeValue[0].text += "%";
                    }
                }
                if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count >= 2)
                {
                    if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count == 2)
                        txtDesPasive.text = string.Format(Language.GetKey("Des1_" + skillData.SkillID), skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Value[Level - 1], skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Value[Level - 1]);
                    txtAttribute[1].transform.parent.gameObject.SetActive(true);
                    txtAttribute[1].text = Language.GetKey(skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Attribute);
                    txtAttributeValue[1].text = skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Value[Level - 1].ToString();
                    if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Attribute.Contains("Percent") || skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Attribute.Contains("Chance"))
                    {
                        txtAttributeValue[1].text += "%";
                    }
                }
                if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count >= 3)
                {
                    if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes.Count == 3)
                        txtDesPasive.text = string.Format(Language.GetKey("Des1_" + skillData.SkillID), skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[0].Value[Level - 1], skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[1].Value[Level - 1], skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[2].Value[Level - 1]);
                    txtAttribute[2].transform.parent.gameObject.SetActive(true);
                    txtAttribute[2].text = Language.GetKey(skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[2].Attribute);
                    txtAttributeValue[2].text = skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[2].Value[Level - 1].ToString();
                    if (skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[2].Attribute.Contains("Percent") || skillData.baseSkills[sgtl.Tier - 1].SkillAttributes[2].Attribute.Contains("Chance"))
                    {
                        txtAttributeValue[2].text += "%";
                    }
                }
            }
        }
    }
}
