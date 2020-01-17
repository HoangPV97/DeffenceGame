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
        Debug.Log(index + ":" + _TYPE);
        txtName.text = Language.GetKey("NAME_" + _TYPE);
        txtDes1.text = Language.GetKey("DES1_" + _TYPE);
        txtDes2.text = Language.GetKey("DES2_" + _TYPE);
        txtDes3.text = Language.GetKey("DES3_" + _TYPE);
        var gda = DataController.Instance.GetGameDataAchievement(_TYPE);
        for (int i = 0; i < Star.Length; i++)
        {
            Star[i].SetActive(i < gda.Level);
        }
        txtNumber.text = string.Format("{0}/{1}", gda.Current, gda.Target);
        PGBar.fillAmount = gda.Current * 1f / gda.Target;
    }
}
