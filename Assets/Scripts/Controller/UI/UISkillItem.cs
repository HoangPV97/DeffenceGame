using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UISkillItem : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI txtSkillName, txtMana, txtCoolDown, txtDes1, txtDes2, txtLevel;
    public GameObject Unlock;
    public TextMeshProUGUI txtUnlockTier;
    public UIButton BtnUpgrade;
    public UIButton Buy;
    public GameObject Active, Pasive;
    public TextMeshProUGUI txtDesPasive;
    public SkillData skillData;
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
    // Start is called before the first frame update
    public void SetUpdata(string SkillID)
    {
        //WEAPON_WIND_SKILL_1
        skillData = JsonUtility.FromJson<SkillData>(ConectingFireBase.Instance.GetTextSkill(SkillID));
        txtSkillName.text = Language.GetKey("Name_" + skillData.SkillID);
        SaveGameTierLevel sgtl = DataController.Instance.GetGameSkillData(SkillID);
        if (sgtl.Level == 0)
        {
            BtnUpgrade.gameObject.SetActive(false);
            int skill4 = int.Parse(SkillID.Split('_')[3]);
            if (skill4 < 4)
            {
                txtUnlockTier.text = string.Format("Tier - {0} \n Unlock", skill4);
                Buy.gameObject.SetActive(false);
                Unlock.gameObject.SetActive(true);
            }
            else
            {
                Buy.gameObject.SetActive(true);
                Unlock.gameObject.SetActive(false);
            }
        }
        else
        {
            BtnUpgrade.gameObject.SetActive(true);
            Buy.gameObject.SetActive(false);
            Unlock.gameObject.SetActive(false);
        }
    }
}
