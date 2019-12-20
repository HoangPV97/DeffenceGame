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
    public UIButton BtnUpgrade;
    // Start is called before the first frame update
    public void SetUpdata(string SkillID)
    {
        //WEAPON_WIND_SKILL_1
        var skillData = JsonUtility.FromJson<SkillData>(ConectingFireBase.Instance.GetTextWeaponSkill(SkillID));
        txtSkillName.text = Language.GetKey("Name_" + skillData.SkillID);

    }
}
