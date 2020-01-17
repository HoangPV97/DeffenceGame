using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIItemAchievement : MonoBehaviour
{
    public ACHIEVEMENT_TYPE _TYPE;
    public TextMeshProUGUI txtName, txtDes1, txtDes2, txtDes3, txtNumber;
    public Image Icon, PGBar;
    public GameObject[] Star;
    // Start is called before the first frame update
    public void SetUpData()
    {
        int index = transform.GetSiblingIndex();
        _TYPE = (ACHIEVEMENT_TYPE)index;
        var gda = DataController.Instance.GetGameDataAchievement(_TYPE);
        var gdb = DataController.Instance.GetAchievementDatabase(_TYPE);
        Debug.Log(index + ":" + _TYPE);
        txtName.text = Language.GetKey("NAME_" + _TYPE);
        txtDes1.text = string.Format(Language.GetKey("DES1_" + _TYPE), gda.Target);
        txtDes2.text = string.Format(Language.GetKey("DES2_" + _TYPE), gdb.GetReward(gda.Level));
        txtDes3.text = string.Format(Language.GetKey("DES3_" + _TYPE), gdb.GetReward(gda.Level + 1));
        for (int i = 0; i < Star.Length; i++)
        {
            Star[i].SetActive(i < gda.Level);
        }
        txtNumber.text = string.Format("{0}/{1}", gda.Current, gda.Target);
        PGBar.fillAmount = gda.Current * 1f / gda.Target;
    }
}
